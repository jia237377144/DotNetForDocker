# 1、 DockerWeb 项目  
> 项目目标：可以将Dotnet项目部署到K8s上  
> 为什么需要K8S?  
> 没有K8S之前的做法：
> - 搭建网站运行环境
> - 发布部署
> - 故障无法快速转移  
> - 纵向扩容
> 
> 有了K8S之后做法：
> - 网站运行环境直接使用对应版本镜像
> - 使用CI持续集成，DI快速部署
> - 故障可以快速转移到集群其他节点
> - 自动横向扩容

# 2、Docker，Docker-Compose和K8S
> ##  1.Docker 
> >Docker本身并不是容器，它只是一个进程，它是创建容器的工具，是应用容器引擎。  
> >Docker组成：(镜像)Image，（容器）Container和（镜像仓）Registry，三者的关系是两两有关系，镜像和镜像仓，镜像和容器；  
> >- 镜像仓：镜像集中存储，每个镜像都有不同的标签(tag),仓库分类：公共仓和私有仓。
> >- 镜像：镜像就是一个只读模板，就像exe安装包一样；  
> >- 容器：就是进项运行的实例，点击exe安装包就可以运行软件；
> >- 常用操作：   
> > [搜索镜像](https://hub.docker.com/)：`docker search dotnet`  
> > 拉取镜像： `docker pull docker.tidebuy.net/dotnet/core/sdk:3.1`  
> > 构建镜像：  `docker build -t jlp_web:test .`  
> > 修改镜像tag: `docker tag jlp_web:test private-docker.tidebuy.net`   
> > 推送镜像: `docker push private-docker.tidebuy.net`  
> > 运行容器：`docker run -it -d -p 5000:80 jlp_web:test /bin/bash`
> > 在容器中运行本地代码：`docker run --rm -it --entrypoint=/usr/bin/dotnet --workdir=/app -p 5555:7777 -v /Users/ios1/jialipeng/github/DotNetForDocker/app:/app docker.tidebuy.net/dotnet/core/sdk:3.1 /app/JLP.Web.dll`   
> $\color{#FF0000}{注意: 在容器中运行本地代码时，需要修改appsettings.json=>urls}$  
> > `dotnet pull microsoft/dotnet`  
> > `docker run --rm -it -p 5555:5000 -v /Users/ios1/jialipeng/test:/app --workdir /app microsoft/dotnet:latest /bin/bash`
> > `dotnet new mvc`  
> > 需要修改appsettings.json=>`"urls":"http://*:5000"`  
> > `dotnet run` 就可以成功运行
> > `docker --helper` 帮助命令
> ## 2.[Docker-Compose](https://docs.docker.com/compose/reference/exec/)
> > Compose 是用于定义和运行多容器 Docker 应用程序的工具。通过 Compose，您可以使用 YML 文件来配置应用程序需要的所有服务。然后，使用一个命令，就可以从 YML 文件配置中创建并启动所有服务。
> > Compose的命令和Docker的差不多：
> > `docker-compose build`、`docker-compose up -d`、`docker-compose stop`、`docker-compose down`、`docker-compose logs jlp_web`  
> > [YML](https://www.jianshu.com/p/a65e692edd5a):YAML（/ˈjæməl/，尾音类似 camel 骆驼）是一个可读性高，用来表达数据序列化的格式。YAML 参考了其他多种语言，包括： C 语言、 Python、Perl，并从 XML、电子邮件的数据格式（RFC 2822）中获得灵感。Clark Evans 在 2001 年首次发表了这种语言 ，另外 Ingy döt Net 与 Oren Ben-Kiki 也是这语言的共同设计者 。当前已经有数种编程语言或脚本语言支持（或者说解析）这种语言。
> ## 3.[K8S](https://kubernetes.io/zh/docs/home/)
> > Kubernetes 是一个可移植的、可扩展的开源平台，用于管理容器化的工作负载和服务，可促进声明式配置和自动化。Kubernetes 拥有一个庞大且快速增长的生态系统。Kubernetes 的服务、支持和工具广泛可用。
> > Kubernetes 的存在取代了Docker-Compose，因为Docker-Compose只是简单的单机容器管理，无法达到满足大家对需求；
> > Kubernetes能做什么？
> > - 服务发现和负载均衡
> > - 存储编排
> > - 自动部署和回滚
> > - 自动二进制打包
> > - 自我修复
> > - 密钥与配置管理
# 2、项目组成部分  
> ##  1）、[JLP.Web](http://www.web.com) : 
> > 一个Core的web项目，使用该项目连接Mysql数据库  
> ##  2）、JLP.DB : 
> > 一个Core的类库，提供数据库访问
# 3、项目实施步骤
> 1. 通过在本地把项目跑起来
> >1）、新建项目  
> >2）、连接数据
> 2. 把项目通过Docker跑起来
> >1）、写[Dockerfile](/docker/Dockerfile)  
> >2）、通过Dockerfile Build镜像：
> > - `docker build -f ./docker/Dockerfile -t jlp_web:1.0 .`
> > - $\color{#FF0000}{注意：上边命令在解决方案目录执行}$
> 3. 把项目通过Docker-Compose跑起来
> >1）、增加[docker-compose.yml](docker-compose.yml)文件    
> >2）、创建网络 `docker network create web`  
> >3) 、设置Mysql服务[参数](https://hub.docker.com/_/mysql)  
> >4) 、设置卷和初始化数据库
> 4. 编写K8S的yaml
> >1）、部署Pod：[deployment](/k8s/web_demployment.yaml)  
> >2) 、提供服务：[service](/k8s/web_service.yaml)  
> >3) 、对外暴露（负载均衡）：[ingress](/k8s/web_ingress.yaml)  
> >4) 、存储：[pvc](/k8s/web_pvc.yaml)  
> >5) 、配置文件:[configmap](/k8s/mysql_configmap.yaml)  
> >6) 、存储密码：[secret](/k8s/mysql_secret.yaml)
> >7) 、所有的文件合并 [test.yaml](k8s/test.yaml)
