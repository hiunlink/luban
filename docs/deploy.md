[//]: # (Author: bug)
[//]: # (Date: 2020-11-23 00:21:21)

# 服务端布署

## Docker 镜像布署
``` bash
    docker pull hugebug4ever/luban:latest
    docker run --name luban-server -d -p 8899:8899 hugebug4ever/luban:latest 
```