# 1、 DockerWeb 项目  
> 项目目标：可以将Dotnet项目部署到K8s上  

# 2、Docker，Docker-Compose和K8S
> ##  1.Docker 容器
> >Docker本身并不是容器，它只是一个进程，它是创建容器的工具，是应用容器引擎。  
> >Docker组成：(镜像)Image，（容器）Container和（镜像仓）Registry，三者的关系是两两有关系，镜像和镜像仓，镜像和容器；  
> > >镜像仓：镜像集中存储，需要构建镜像的基础镜像可以从镜像仓中拉去,仓库分类：公共仓和私有仓。镜像仓就像Nuget，NPM，PIP3。  
> > >常见公共仓:[DockerHub](https://hub.docker.com)，[网易云](https://c.163yun.com/hub#/home)和[阿里云](https://developer.aliyun.com/mirror/)。  
> > >私有仓：开发者或者企业自建的镜像存储库，通常用来保存企业内部的 Docker 镜像，用于内部开发流程和产品的发布、版本控制。
> > >常用操作：`docker pull`、`docker push`  
> > >镜像：  
> > >容器：  

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
