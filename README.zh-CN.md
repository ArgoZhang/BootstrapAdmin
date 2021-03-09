# BootstrapAdmin

<a href="README.md">English</a> | <span>中文</span>

---

##### Version & Coverage
[![Release](https://img.shields.io/endpoint.svg?logo=Groupon&logoColor=red&color=green&label=release&url=https://admin.blazor.zone/api/Gitee/Releases?userName=dotnetchina)](https://gitee.com/dotnetchina/BootstrapAdmin/releases)
[![Coveralls](https://img.shields.io/coveralls/github/ArgoZhang/BootstrapAdmin/master.svg?logo=ReverbNation&logoColor=green&label=coveralls)](https://coveralls.io/github/ArgoZhang/BootstrapAdmin?branch=master)
[![Codecov](https://img.shields.io/codecov/c/gh/argozhang/bootstrapadmin/master.svg?logo=codecov&label=codecov)](https://codecov.io/gh/argozhang/bootstrapadmin/branch/master)

##### Gitee
[![Appveyor build](https://img.shields.io/endpoint.svg?logo=appveyor&label=build&color=blueviolet&url=https://admin.blazor.zone/api/Gitee/Builds?projName=bootstrapadmin-9m1jm)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin-9m1jm)
[![Build Status](https://img.shields.io/appveyor/ci/ArgoZhang/bootstrapadmin-9m1jm/master.svg?logo=appveyor&label=master)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin-9m1jm/branch/master)
[![Test](https://img.shields.io/appveyor/tests/ArgoZhang/bootstrapadmin-9m1jm/master.svg?logo=appveyor&)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin-9m1jm/branch/master/tests)
[![Issue Status](https://img.shields.io/endpoint.svg?logo=Groupon&logoColor=critical&label=issues&url=https://admin.blazor.zone/api/Gitee/Issues?userName=dotnetchina)](https://gitee.com/dotnetchina/BootstrapAdmin/issues)
[![Pull Status](https://img.shields.io/endpoint.svg?logo=Groupon&logoColor=green&color=success&label=pulls&url=https://admin.blazor.zone/api/Gitee/Pulls?userName=dotnetchina)](https://gitee.com/dotnetchina/BootstrapAdmin/pulls)

##### GitHub
[![Appveyor build](https://img.shields.io/endpoint.svg?logo=appveyor&label=build&color=blueviolet&url=https://admin.blazor.zone/api/Gitee/Builds?projName=bootstrapadmin)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin)
[![master status](https://img.shields.io/appveyor/ci/ArgoZhang/bootstrapadmin/master.svg?logo=appveyor&label=master)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin/branch/master)
[![Test](https://img.shields.io/appveyor/tests/argozhang/bootstrapadmin/master.svg?logo=appveyor&)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin/branch/master/tests)
[![Github build](https://img.shields.io/github/workflow/status/ArgoZhang/BootstrapAdmin/Auto%20Build%20CI/master?label=master&logoColor=green&logo=github)](https://github.com/ArgoZhang/BootstrapAdmin/actions?query=workflow%3A%22Auto+Build+CI%22+branch%3Amaster)
[![Repo Size](https://img.shields.io/github/repo-size/ArgoZhang/BootstrapAdmin.svg?logo=github&logoColor=green&label=repo)](https://github.com/ArgoZhang/BootstrapAdmin)
[![Commit Date](https://img.shields.io/github/last-commit/ArgoZhang/BootstrapAdmin/master.svg?logo=github&logoColor=green&label=commit)](https://github.com/ArgoZhang/BootstrapAdmin)

## 项目介绍
一直需要一款后台管理系统，但是网上很多开源项目都是 **Java** 开发的，本人是 **NET** 平台的对 **Java** 一窍不通，C#版本的本来就少而且还没有合适的。于是决定自己开发一套后台管理系统。由于前台采用 **Bootstrap** 布局样式，所以就叫做 **BootstrapAdmin** 。本系统可以用于所有的 Web 应用程序，目前版本已经升级到 **NET CORE** 具备跨平台能力。数据库方面同时支持多种数据库，详细列表见后面**数据库**的详细列表，切换数据源仅需更改配置文件无需重启应用程序，配置简单灵活。UI 前端使用流行的 Bootstrap 框架布局对移动设备的兼容性非常好，自适应目前市场几乎所有终端设备。本系统还具备单一后台支持多前台的特色，提供 **单点登录（SSO）** 的能力。  

使用 NET Core + Bootstrap + PetaPoco + HTML 5 + jQuery 构建的后台管理平台  

### 特别说明
**BootstrapAdmin** 无需二次开发，要做的仅仅是与前台系统集成，前台系统模板工程为 **Bootstrap.Client**   
项目原始出发点是把权限系统从业务系统中剥离出来，项目开发专注于功能，详细配置说明请点击 [查看文档](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E7%B3%BB%E7%BB%9F%E9%9B%86%E6%88%90)

### 主要功能  
1. 通过配置与前台网站集成
2. 构建前台系统分层级菜单
3. 提供单一后台支持多前台应用配置
4. 提供单点登录
5. 集成系统认证授权模块
6. 提供角色，部门，用户，菜单，前台应用程序授权  
角色对用户授权  
角色对菜单授权  
角色对部门授权  
角色对应用程序授权（多个前台应用公用一个后台权限管理系统）  
部门对用户授权  
7. 提供字典表用于前台网站个性化配置  
8. 完全响应式布局（支持电脑、平板、手机等所有主流设备）
9. 内置多数据源支持，配置简单立即生效无需重启
10. 内置数据内存缓存机制，页面快速响应
11. 内置数据 **操作日志** 与用户 **登录日志**   
跟踪记录用户 **登录主机地点**  **浏览器**  **操作系统** 信息  

更新日志：[传送门](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/更新日志)

### 优势
1. 前台系统不用编写登录、授权、认证模块；只负责编写业务模块即可
2. 后台系统无需任何二次开发，直接发布即可使用
3. 前台与后台系统分离，分别为不同的系统（域名可独立）
4. 可扩展为多租户应用

详细资料请点击 [查看文档](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E9%A1%B9%E7%9B%AE%E4%BB%8B%E7%BB%8D)  

### 数据库
数据库支持列表如下：  
**MSSQL/Oracle/SQLite/MySql/MariaDB/Postgresql/Firebird/MongoDB**  

### 浏览器支持

![chrome](https://img.shields.io/badge/chrome->%3D4.5-success.svg?logo=google%20chrome&logoColor=red)
![firefox](https://img.shields.io/badge/firefox->38-success.svg?logo=mozilla%20firefox&logoColor=red)
![edge](https://img.shields.io/badge/edge->%3D12-success.svg?logo=microsoft%20edge&logoColor=blue)
![ie](https://img.shields.io/badge/ie->%3D11-success.svg?logo=internet%20explorer&logoColor=blue)
![Safari](https://img.shields.io/badge/safari->%3D9-success.svg?logo=safari&logoColor=blue)
![Andriod](https://img.shields.io/badge/andriod->%3D4.4-success.svg?logo=android)
![oper](https://img.shields.io/badge/opera->%3D3.0-success.svg?logo=opera&logoColor=red)

```json
"browserslist": [
  "Chrome >= 45",
  "Firefox >= 38",
  "Edge >= 12",
  "Explorer >= 11",
  "iOS >= 9",
  "Safari >= 9",
  "Android >= 4.4",
  "Opera >= 30"
]
```  

### 移动端支持  
![ios](https://img.shields.io/badge/ios-supported-success.svg?logo=apple&logoColor=white)
![Andriod](https://img.shields.io/badge/andriod-suported-success.svg?logo=android)
![windows](https://img.shields.io/badge/windows-suported-success.svg?logo=windows&logoColor=blue)

|                        |  **Chrome**  |  **Firefox**  |  **Safari**  |  **Android Browser & WebView**  |  **Microsoft Edge**  |
| -------                | ---------    | ---------     | ------       | -------------------------       | --------------       |
|  **iOS**               | Supported    | Supported     | Supported    | N/A                             | Supported            |
|  **Android**           | Supported    | Supported     | N/A          | Android v5.0+ supported         | Supported            |
|  **Windows 10 Mobile** | N/A          | N/A           | N/A          | N/A                             | Supported            |

### 桌面浏览器支持  
![macOS](https://img.shields.io/badge/macOS-supported-success.svg?logo=apple&logoColor=white)
![linux](https://img.shields.io/badge/linux-suported-success.svg?logo=linux&logoColor=white)
![windows](https://img.shields.io/badge/windows-suported-success.svg?logo=windows)

|         | Chrome    | Firefox   | Internet Explorer | Microsoft Edge | Opera     | Safari        |
| ------- | --------- | --------- | ----------------- | -------------- | --------- | ------------- |
| Mac     | Supported | Supported | N/A               | N/A            | Supported | Supported     |
| Linux   | Supported | Supported | N/A               | N/A            | N/A       | N/A           |
| Windows | Supported | Supported | Supported, IE10+  | Supported      | Supported | Not supported |

## QQ交流群
[![QQ](https://img.shields.io/badge/QQ-795206915-green.svg?logo=tencent%20qq&logoColor=red)](https://shang.qq.com/wpa/qunwpa?idkey=d381355e50ff91db410c3da3eadb081ba859f64c2877e86343f4709b171f28b8)

## 开发环境搭建
1. 安装 .net core sdk [官方网址](http://www.microsoft.com/net/download)
2. 安装 Visual Studio 2019 最新版 [官方网址](https://visualstudio.microsoft.com/vs/getting-started/)
3. 获取本项目代码 [BootstrapAdmin](https://gitee.com/dotnetchina/BootstrapAdmin)  

环境搭建教程 [详细说明](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E5%AE%89%E8%A3%85%E6%95%99%E7%A8%8B?sort_id=1333477)   

### 安装数据库

本项目默认使用 SQLite 数据库，内置数据库脚本 
1. SQLite
2. SqlServer
3. MySql
4. Oracle
5. MongoDB   

数据库配置 [详细说明](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/数据库连接配置?sort_id=1333482)   

## 分支说明  
分支说明 [详细说明](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/分支说明)

## 演示地址  
[![website1](https://img.shields.io/badge/linux-http://ba.zylweb.cn-success.svg?logo=buzzfeed&logoColor=green)](http://ba.zylweb.cn)
[![website2](https://img.shields.io/badge/linux-http://admin.blazor.zone-success.svg?logo=buzzfeed&logoColor=green)](http://admin.blazor.zone)  

### 登录用户名与密码  
管理账号：Admin/123789  
普通账号：User/123789

## Docker 镜像
[![Docker](https://img.shields.io/docker/cloud/automated/argozhang/ba.svg?logo=docker&logoColor=success)](https://hub.docker.com/r/argozhang/ba)
[![Docker](https://img.shields.io/docker/cloud/build/argozhang/ba.svg?logo=docker&logoColor=success)](https://hub.docker.com/r/argozhang/ba/builds)
[![Docker](https://img.shields.io/github/workflow/status/ArgoZhang/BootstrapAdmin/Docker%20Image%20CI/master?label=Docker%20Image%20CI&logo=github&logoColor=green)](https://github.com/ArgoZhang/BootstrapAdmin/actions?query=workflow%3A%22Docker+Image+CI%22%3Amaster)

### Docker Hub 
镜像拉取 [传送门](https://hub.docker.com/r/argozhang/ba)
```bash
docker pull argozhang/ba
```
### 七牛云:  
镜像拉取 [传送门](https://hub.qiniu.com/store/argozhang/ba) 
```bash
docker pull reg.qiniu.com/argozhang/ba
```

## 配置说明
详细配置说明请点击 [查看文档](https://gitee.com/dotnetchina/BootstrapAdmin/wikis) 查看配置说明小节  

## 常见问题Q&A
请点击 [查看文档](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E5%B8%B8%E8%A7%81%E9%97%AE%E9%A2%98Q&A) 查看常见问题小节  

## 开源协议
[![Gitee license](https://img.shields.io/github/license/argozhang/bootstrapadmin.svg?logo=git&logoColor=red)](https://gitee.com/dotnetchina/BootstrapAdmin/blob/master/LICENSE)

## GVP 奖杯
![项目奖杯](https://images.gitee.com/uploads/images/2021/0112/112021_9d570be1_554725.png "GVP.png")

## 项目截图

后台首页

![后台首页](https://gitee.com/LongbowEnterprise/Pictures/raw/master/BootstrapAdmin/BA02-01.png "BAHome-01.png")

更多截图请点击 [查看文档](https://gitee.com/dotnetchina/BootstrapAdmin/wikis) 查看项目截图小节  

## 特别鸣谢

1. <a href="https://gitee.com/571183806" target="_blank">**云龙**</a> 提供云服务器搭建在线演示系统
2. <a href="https://gitee.com/Ysmc" target="_blank">**一事冇诚**</a> 对 MongoDB 数据库提供了详细测试
3. <a href="https://gitee.com/Axxbis" target="_blank">**爱吃油麦菜**</a> 提供云服务器与二级域名搭建备份演示系统、测试环境以及图床
4. <a href="https://gitee.com/kasenhoo" target="_blank">**kasenhoo**</a> 对 CentOS + MySql 环境提供详细测试
5. <a href="https://gitee.com/finally44177" target="_blank">**finally44177**</a> 提供 AdminLTE UI 样式 PR 对 MongoDB 数据库提供了详细测试

## 参与贡献

1. Fork 本项目
2. 新建 Feat_xxx 分支
3. 提交代码
4. 新建 Pull Request

## 相关视频讲解

[视频教材](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E8%AF%BE%E7%A8%8B%E5%88%97%E8%A1%A8?sort_id=1916635#%E8%AF%BE%E7%A8%8B%E5%88%97%E8%A1%A8)

## 捐助

如果这个项目对您有所帮助，请扫下方二维码打赏一杯咖啡。    

<img src="https://gitee.com/LongbowEnterprise/Pictures/raw/master/WeChat/BarCode@2x.png" width="382px;" />
