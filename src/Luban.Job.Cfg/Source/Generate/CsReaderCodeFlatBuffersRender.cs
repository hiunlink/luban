
using Luban.Job.Cfg.Defs;
using Luban.Job.Cfg.Generate;
using Luban.Job.Common;
using Luban.Job.Common.Generate;
using Luban.Job.Common.Utils;
using System.Threading.Tasks;
using Luban.Common.Protos;
using Bright.Common;
using System.Text.RegularExpressions;

namespace Luban.Job.Cfg.Source.Generate
{
    [Render("code_reader_cs_flatbuffers")]
    class CsReaderCodeFlatBuffersRender : TemplateCodeRenderBase
    {
        string templateName = "reader";
        protected override string RenderTemplateDir => "flatbuffers";
        public override void Render(GenContext ctx)
        {
            string genType = ctx.GenType;
            ctx.Render = this;
            ctx.Lan = ELanguage.CS;
            DefAssembly.LocalAssebmly.CurrentLanguage = ctx.Lan;
            foreach (var c in ctx.ExportTypes)
            {
                if (c is DefTable)
                {
                    ctx.Tasks.Add(Task.Run(() =>
                    {
                        string body = RenderReader(ctx, c as DefTable);
                        if (string.IsNullOrWhiteSpace(body))
                        {
                            return;
                        }
                        var content = FileHeaderUtil.ConcatAutoGenerationHeader(body, ctx.Lan);
                        var file = RenderFileUtil.GetDefTypePath(c.FullName + "Reader", ctx.Lan);
                        var md5 = CacheFileUtil.GenMd5AndAddCache(file, content);
                        ctx.GenCodeFilesInOutputCodeDir.Add(new FileInfo() { FilePath = file, MD5 = md5 });
                    }));
                }
            }
        }

        private string RenderReader(GenContext ctx, DefTable table)
        {
            var template = GetConfigTemplate(templateName);
            var result = template.RenderCode(new {
                ReaderName = table.FullName + "Reader",
                Namespace = ctx.TopModule,
                Table = table,
                KeyType = table.KeyTType.TypeName,
                KeyField = Regex.Replace(table.IndexField.Name, "^[a-z]", c => c.Value.ToUpper())
            });
            return result;
        }
    }
}
