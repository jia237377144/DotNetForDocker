# 1、 DockerWeb 项目  

项目目标：可以将Dotnet项目部署到K8s上  
为什么需要K8S?  
没有K8S之前的做法：

- 搭建网站运行环境
- 发布部署
- 故障无法快速转移  
- 纵向扩容

有了K8S之后做法：

- 网站运行环境直接使用对应版本镜像
- 使用CI持续集成，DI快速部署
- 故障可以快速转移到集群其他节点
- 自动横向扩容

## Docker，Docker-Compose和K8S

### Docker

Docker本身并不是容器，它只是一个进程，它是创建容器的工具，是应用容器引擎。  
Docker组成：(镜像)Image，（容器）Container和（镜像仓）Registry，三者的关系是两两有关系，镜像和镜像仓，镜像和容器； 

- 镜像仓：镜像集中存储，每个镜像都有不同的标签(tag),仓库分类：公共仓和私有仓。
- 镜像：镜像就是一个只读模板，就像exe安装包一样；  
- 容器：就是进项运行的实例，点击exe安装包就可以运行软件；

#### 常用操作

$\color{#FF0000}{注意: 在容器中运行本地代码时，需要修改appsettings.json=>urls}$

``` bash

#帮助命令
docker --helper

#搜索镜像  https://hub.docker.com/
docker search dotnet

#拉取镜像
docker pull docker.tidebuy.net/dotnet/core/sdk:3.1

#构建镜像
docker build -t jlp_web:test .

#修改镜像tag
docker tag jlp_web:test private-docker.tidebuy.net

#推送镜像
docker push private-docker.tidebuy.net

#运行容器
docker run -it -d -p 5000:80 jlp_web:test /bin/bash
```

#### 在容器中运行本地代码

``` shell
docker run --rm -it --entrypoint=/usr/bin/dotnet --workdir=/app -p 5555:7777 -v ~/github/DotNetForDocker/app:/app docker.tidebuy.net/dotnet/core/sdk:3.1 /app/JLP.Web.dll
```

#### 构建一个新MVC项目

``` bash

#拉取dotnet镜像
docker pull microsoft/dotnet

#启动Docker，并暴露端口
docker run --rm -it -p 5555:5000 -p 5556:5001 -v ~/test:/app --workdir /app microsoft/dotnet:latest /bin/bash

#创建 mvc 项目
dotnet new mvc --no-https

#需要修改 launchSettings.json => {"urls":"http://*:5000"}
sed -i 's/localhost/*/g' Properties/launchSettings.json

#启动MVC应用服务
dotnet run

#拉取Mysql镜像
docker pull mysql:5.7

#启动Mysql应用服务并暴露端口
docker run -it -d  -e  MYSQL_ROOT_PASSWORD="123456"  -e  MYSQL_USER="admin"  -e  MYSQL_PASSWORD="123456"  -e   MYSQL_DATABASE="meshop_www"  -p 33305:3306  mysql:5.7  --character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci

#mvc连接mysql时，使用33305端口

```



### [Docker-Compose](https://docs.docker.com/compose/reference/exec/)

Compose 是用于定义和运行多容器 Docker 应用程序的工具。
通过 Compose，您可以使用 YML 文件来配置应用程序需要的所有服务。
然后，使用一个命令，就可以从 YML 文件配置中创建并启动所有服务。

Compose的命令和Docker的差不多：

``` bash

#构建
docker-compose build

#后台启动所有服务
docker-compose up -d

#停止所有服务
docker-compose stop

#停止并移除所有服务
docker-compose down

#打印日志
docker-compose logs jlp_web
```

### [YML](https://www.jianshu.com/p/a65e692edd5a)

:YAML（/ˈjæməl/，尾音类似 camel 骆驼）是一个可读性高，用来表达数据序列化的格式。YAML 参考了其他多种语言，包括： C 语言、 Python、Perl，并从 XML、电子邮件的数据格式（RFC 2822）中获得灵感。Clark Evans 在 2001 年首次发表了这种语言 ，另外 Ingy döt Net 与 Oren Ben-Kiki 也是这语言的共同设计者 。当前已经有数种编程语言或脚本语言支持（或者说解析）这种语言。

### [K8S](https://kubernetes.io/zh/docs/home/)

Kubernetes 是一个可移植的、可扩展的开源平台，用于管理容器化的工作负载和服务，可促进声明式配置和自动化。
Kubernetes 拥有一个庞大且快速增长的生态系统。
Kubernetes 的服务、支持和工具广泛可用。
Kubernetes 的存在取代了Docker-Compose，因为Docker-Compose只是简单的单机容器管理，无法达到满足大家对需求；

#### Kubernetes能做什么?

- 服务发现和负载均衡
- 存储编排
- 自动部署和回滚
- 资源分配
- 自我修复
- 密钥与配置管理

## 项目组成部分  

### [JLP.Web](http://www.web.com)

一个Core的web项目，使用该项目连接Mysql数据库  

### JLP.DB

一个Core的类库，提供数据库访问

## 项目实施步骤

- 通过在本地把项目跑起来
  1. 新建项目  
  2. 连接数据
- 把项目通过Docker跑起来
  1. 写[Dockerfile](/docker/Dockerfile)  
  2. 通过Dockerfile Build镜像：
      - `docker build -f ./docker/Dockerfile -t jlp_web:1.0 .`
      - $\color{#FF0000}{注意：上边命令在解决方案目录执行}$
  3. 启动容器：
      - `docker run -it --rm -p 7777:7777 jlp_web:1.0 /bin/bash`
- 把项目通过Docker-Compose跑起来
  1. 增加[docker-compose.yml](docker-compose.yml)文件
  2. 创建网络 `docker network create web`  
  3. 设置Mysql服务[参数](https://hub.docker.com/_/mysql)  
  4. 设置卷和初始化数据库
  5. 启动服务：- [1、 DockerWeb 项目](#1-dockerweb-项目)
  - [Docker，Docker-Compose和K8S](#dockerdocker-compose和k8s)
    - [Docker](#docker)
      - [常用操作](#常用操作)
      - [在容器中运行本地代码](#在容器中运行本地代码)
      - [构建一个新MVC项目](#构建一个新mvc项目)
    - [Docker-Compose](#docker-compose)
    - [YML](#yml)
    - [K8S](#k8s)
      - [Kubernetes能做什么?](#kubernetes能做什么)
  - [项目组成部分](#项目组成部分)
    - [JLP.Web](#jlpweb)
    - [JLP.DB](#jlpdb)
  - [项目实施步骤](#项目实施步骤)- [1、 DockerWeb 项目](#1-dockerweb-项目)
  - [Docker，Docker-Compose和K8S](#dockerdocker-compose和k8s)
    - [Docker](#docker)
      - [常用操作](#常用操作)
      - [在容器中运行本地代码](#在容器中运行本地代码)
      - [构建一个新MVC项目](#构建一个新mvc项目)
    - [Docker-Compose](#docker-compose)
    - [YML](#yml)
    - [K8S](#k8s)
      - [Kubernetes能做什么?](#kubernetes能做什么)
  - [项目组成部分](#项目组成部分)
    - [JLP.Web](#jlpweb)
    - [JLP.DB](#jlpdb)
  - [项目实施步骤](#项目实施步骤)
      - `docker-compose up -d`
- 编写K8S的yaml
  1. 部署Pod：[deployment](/k8s/web_demployment.yaml)  
  2. 提供服务：[service](/k8s/web_service.yaml)  
  3. 对外暴露（负载均衡）：[ingress](/k8s/web_ingress.yaml)  
  4. 存储：[pvc](/k8s/web_pvc.yaml)  
  5. 配置文件:[configmap](/k8s/mysql_configmap.yaml)  
  6. 存储密码：[secret](/k8s/mysql_secret.yaml)
  7. 所有的文件合并 [test.yaml](k8s/test.yaml)
