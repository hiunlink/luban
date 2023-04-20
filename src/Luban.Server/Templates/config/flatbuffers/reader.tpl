using System.Collections.Generic;
using Google.FlatBuffers;
using UnityEngine;
using tb;

namespace Module.TableReader
{
    public static class {{x.reader_name}}
    {
        private static {{x.table.full_name}} _table;
        private static bool _isInit = false;
        private static readonly Dictionary<{{x.key_type}}, int> KeyIndexMap = new Dictionary<{{x.key_type}}, int>();

        private static void Init()
        {
            if (!_isInit)
            {
                var configPath = Application.dataPath+"/BundleResources/Tables/datas_bin/{{x.table.full_name}}.bin";
                using (var fileStream = System.IO.File.OpenRead(configPath))
                {
                    var buffer = new byte[fileStream.Length];
                    var read = fileStream.Read(buffer, 0, buffer.Length);
                    _table = {{x.table.full_name}}.GetRootAs{{x.table.full_name}}(new ByteBuffer(buffer));
                    for (int i = 0; i < _table.DataListLength; i++)
                    {
                        var dialog = _table.DataList(i);
                        if (dialog != null) KeyIndexMap[dialog.Value.{{x.key_field}}] = i;
                    }
                }

                _isInit = true;
            }
        }

        #region Public Methods

        public static int Count
        {
            get
            {
                Init();
                return _table.DataListLength;
            }
        }

        public static {{x.table.value_type}}? GetData({{x.key_type}} key)
        {
            Init();
            return _table.DataList(KeyIndexMap[key]);
        }
        
        public static {{x.table.value_type}}? GetDataByIndex(int index)
        {
            Init();
            return _table.DataList(index);
        }
        
        #endregion

    }
}