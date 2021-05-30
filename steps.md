# 启动Mysql
``` shell
docker run -d -it -p 3306:3306 \
-v $PWD/db/data:/var/libmysql \
-v $PWD/db/config:/etc/mysql/conf.d \
-v $PWD/db/sql:/docker-entrypoint-initdb.d \
-e MYSQL_ROOT_PASSWORD=123456 \
-e MYSQL_USER=admin \
-e MYSQL_PASSWORD=123456 \
-e MYSQL_DATABASE=dockerweb \
--name mysql \
mysql:5.7
```

# 搭建本地仓库

## 拉取镜像，启动容器
``` shell
docker pull registry

docker tag registry:latest registry:local


docker run -d -p 5000:5000 -v $(pwd):/var/lib/registry --restart always --name registry registry:local

```

## 配置Docker的镜像仓
对docker 作出如下配置，并重启使其生效
``` shell
mac 配置路径: 工具栏–> docker –> Preferences –> Daemon –> basic –> Insecure registries 加上一行: 10.21.71.237:5000
{
  "insecure-registries" : [
    "10.21.71.237:5000"
  ]
}
```
## 给需要Push的重新打Tag
``` shell
docker tag jlp_web:1.0 192.168.31.138:5000/jlp_web:1.0
```


## Push本地镜像
``` shell
docker push 192.168.31.138:5000/jlp_web:1.0
```

## 重启MiniKube
``` shell

  minikube delete && minikube start --cpus=2 --memory=3072 \
	--registry-mirror=https://registry.docker-cn.com \
  --insecure-registry=192.168.31.138:5000


```

## 端口代理：
``` shell

kubectl port-forward svc/db 3306:3306

```