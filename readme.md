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
> > 搜索镜像：`docker search dotnet`  
> > 拉去镜像： `docker pull docker.tidebuy.net/dotnet/core/sdk:3.1`  
> > 构建镜像：  `docker build -t jlp_web:test .`  
> > 修改镜像tag: `docker tag jlp_web:test private-docker.tidebuy.net`   
> > 推送镜像: `docker push private-docker.tidebuy.net`  
> > 运行容器：`docker run -it -d -p 5000:80 jlp_web:test /bin/bash`
> > 在容器中运行本地代码：`docker run --rm -it --entrypoint=/usr/bin/dotnet --workdir=/app -p 5555:7777 -v /Users/ios1/jialipeng/github/DotNetForDocker/app:/app docker.tidebuy.net/dotnet/core/sdk:3.1 /app/JLP.Web.dll`   
> > 注意: 在容器中运行本地代码时，需要修改appsettings.json=>urls  
> > `dotnet pull microsoft/dotnet`  
> > `docker run --rm -it -p 5555:5000 -v /Users/ios1/jialipeng/test:/app --workdir /app microsoft/dotnet:latest /bin/bash`
> > `dotnet new mvc`  
> > 需要修改appsettings.json=>`"urls":"http://*:5000"`  
> > `dotnet run` 就可以成功运行
> ## 2.Docker-Compose
> > Compose 是用于定义和运行多容器 Docker 应用程序的工具。通过 Compose，您可以使用 YML 文件来配置应用程序需要的所有服务。然后，使用一个命令，就可以从 YML 文件配置中创建并启动所有服务。
# 2、项目组成部分  
> ##  1）、JLP.Web : 
> > 一个Core的web项目，使用该项目连接Mysql数据库  
> ##  2）、JLP.DB : 
> > 一个Core的类库，提供数据库访问
# 3、项目实施步骤
> 1. 通过在本地把项目跑起来
> >1）、新建项目  
> >2）、连接数据
> 2. 把项目通过Docker跑起来
> >1）、写Dockerfile  
> >2）、通过Dockerfile Build镜像：
> > - `docker build -f ./docker/Dockerfile -t jlp_web:1.0 .`
> > - 注意：上边命令在解决方案目录执行
> 3. 把项目通过Docker-Compose跑起来
> >1）、增加docker-compose.yml文件  
> >2）、
