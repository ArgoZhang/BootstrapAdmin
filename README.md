# BootstrapAdmin

##### Version & Licenses
[![Release Tag](https://img.shields.io/endpoint.svg?logo=Groupon&logoColor=red&color=green&label=release&url=https%3A%2F%2Fba.sdgxgz.com%2FBA%2Fapi%2FGitee%2FReleases)](http://ba.sdgxgz.com/BA/Tools/Index?ReturnUrl=https%3A%2F%2Fgitee.com%2FLongbowEnterprise%2FBootstrapAdmin%2Freleases)
[![Gitee license](https://img.shields.io/github/license/argozhang/bootstrapadmin.svg?logo=git&logoColor=red)](https://gitee.com/LongbowEnterprise/BootstrapAdmin/blob/master/LICENSE)

##### Gitee
[![Appveyor build](https://img.shields.io/endpoint.svg?logo=appveyor&label=build&color=blueviolet&url=https%3A%2F%2Fba.sdgxgz.com%2FBA%2Fapi%2FGitee%2FBuilds?projName=bootstrapadmin-9m1jm)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin-9m1jm)
[![Build Status](https://img.shields.io/appveyor/ci/ArgoZhang/bootstrapadmin-9m1jm/dev.svg?logo=appveyor&label=maser)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin-9m1jm)
[![Test Status](https://img.shields.io/appveyor/tests/ArgoZhang/bootstrapadmin-9m1jm/master.svg?logo=appveyor&)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin-9m1jm/build/tests)
[![Issue Status](https://img.shields.io/endpoint.svg?logo=Groupon&logoColor=critical&label=issues&url=https%3A%2F%2Fba.sdgxgz.com%2FBA%2Fapi%2FGitee%2FIssues)](https://gitee.com/LongbowEnterprise/BootstrapAdmin/issues)
[![Pull Status](https://img.shields.io/endpoint.svg?logo=Groupon&logoColor=green&color=success&label=pulls&url=https%3A%2F%2Fba.sdgxgz.com%2FBA%2Fapi%2FGitee%2FPulls)](https://gitee.com/LongbowEnterprise/BootstrapAdmin/pulls)

##### GitHub
[![Appveyor build](https://img.shields.io/endpoint.svg?logo=appveyor&label=build&color=blueviolet&url=https%3A%2F%2Fba.sdgxgz.com%2FBA%2Fapi%2FGitee%2FBuilds?projName=bootstrapadmin)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin)
[![Build status](https://img.shields.io/appveyor/ci/ArgoZhang/bootstrapadmin/dev.svg?logo=appveyor&label=dev)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin/branch/dev)
[![Build status](https://img.shields.io/appveyor/ci/ArgoZhang/bootstrapadmin/dev.svg?logo=appveyor&label=master)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin/branch/master)
[![Test Status](https://img.shields.io/appveyor/tests/argozhang/bootstrapadmin/master.svg?logo=appveyor&)](https://ci.appveyor.com/project/ArgoZhang/bootstrapadmin/branch/master/tests)
[![Cover Status](https://img.shields.io/coveralls/github/ArgoZhang/BootstrapAdmin/master.svg?logo=github&logoColor=green)](https://coveralls.io/github/ArgoZhang/BootstrapAdmin)
[![Repo Size](https://img.shields.io/github/repo-size/ArgoZhang/BootstrapAdmin.svg?logo=github&logoColor=green)](https://github.com/ArgoZhang/BootstrapAdmin)
[![Commit Date](https://img.shields.io/github/last-commit/ArgoZhang/BootstrapAdmin/master.svg?logo=github&logoColor=green)](https://github.com/ArgoZhang/BootstrapAdmin)

## 项目介绍
一直需要一款后台管理系统，但是网上很多开源项目都是 **Java** 开发的，本人是 **NET** 平台的对 **Java** 一窍不通，C#版本的本来就少而且还没有合适的。于是决定自己开发一套后台管理系统。由于前台采用 **Bootstrap** 布局样式，所以就叫做 **BootstrapAdmin** 。本系统可以用于所有的 Web 应用程序，目前版本已经升级到 **NET CORE** 具备跨平台能力。数据库方面同时支持多种数据库，详细列表见后面**数据库**的详细列表，切换数据源仅需更改配置文件无需重启应用程序，配置简单灵活。UI 前端使用流行的 Bootstrap 框架布局对移动设备的兼容性非常好，自适应目前市场几乎所有终端设备。本系统还具备单一后台支持多前台的特色，提供 **单点登录（SSO）** 的能力。  

使用 HTML 5 + jQuery + NET Core 2.2 + Bootstrap 4.1 + PetaPoco 构建的后台管理平台  

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

#### 优势
1. 前台系统不用编写登录、授权、认证模块；只负责编写业务模块即可
2. 后台系统无需任何二次开发，直接发布即可使用
3. 前台与后台系统分离，无任何依赖关系

详细资料请点击 [查看文档](https://gitee.com/LongbowEnterprise/BootstrapAdmin/wikis/项目介绍)  

### 数据库
数据库支持列表如下：  
**MSSQL/Oracle/SQLite/MySql/MariaDB/Postgresql/Firebird/MsAccess/MongoDB**  

### 浏览器支持
```json
"browserslist": [
  "Chrome >= 45",
  "Firefox >= 38",
  "Edge >= 12",
  "Explorer >= 10",
  "iOS >= 9",
  "Safari >= 9",
  "Android >= 4.4",
  "Opera >= 30"
]
```  

### 移动端支持  
|                        |  **Chrome**  |  **Firefox**  |  **Safari**  |  **Android Browser & WebView**  |  **Microsoft Edge**  |
| -------                | ---------    | ---------     | ------       | -------------------------       | --------------       |
|  **Android**           | Supported    | Supported     | N/A          | Android v5.0+ supported         | Supported            |
|  **iOS**               | Supported    | Supported     | Supported    | N/A                             | Supported            |
|  **Windows 10 Mobile** | N/A          | N/A           | N/A          | N/A                             | Supported            |

### 桌面浏览器支持  
|         | Chrome    | Firefox   | Internet Explorer | Microsoft Edge | Opera     | Safari        |
| ------- | --------- | --------- | ----------------- | -------------- | --------- | ------------- |
| Mac     | Supported | Supported | N/A               | N/A            | Supported | Supported     |
| Windows | Supported | Supported | Supported, IE10+  | Supported      | Supported | Not supported |

## QQ交流群
群号
795206915  
[快速加群](https://shang.qq.com/wpa/qunwpa?idkey=d381355e50ff91db410c3da3eadb081ba859f64c2877e86343f4709b171f28b8) 

## 安装教程
1. 安装 .net core sdk [官方网址](http://www.microsoft.com/net/download)
2. 安装 Visual Studio IDE 2017以上 [官方网址](https://visualstudio.microsoft.com/vs/getting-started/)
3. 获取本项目代码 [BootstrapAdmin](https://gitee.com/LongbowEnterprise/BootstrapAdmin)
4. 安装数据库  
以微软MSSQL为例，执行解决方案中SQLServer目录（物理硬盘中DatabaseScripts目录下）Install.sql脚本创建数据库
5. 初始化数据  
执行对应目录下InitData.sql脚本
6. 拷贝Longbow.lic文件  
拷贝Scripts目录下Longbow.lic文件到bin目录下的程序集输出目录（bin\debug\netcoreapp2.2\）
7. 系统登录用户名与口令  
用户名：**Admin**  
密码：**123789**  

## 分支说明  
 **dev** 开发分支目前开发环境配置是 windows + SQLite  
 **master** 发布分支与在线演示版本同步

## 演示地址  
**在线演示** <a href="http://argo.zylweb.cn" target="_blank">[传送门]http://argo.zylweb.cn</a>  
**备用地址** <a href="http://ba.sdgxgz.com" target="_blank">[传送门]http://ba.sdgxgz.com</a>  

### 登录用户名与密码  
管理员：**Admin/123789**  
普通用户：**User/123789**  

### 网站服务器配置：  
CPU: 1核  
MEM: 2G

## 配置说明
详细配置说明请点击 [查看文档](https://gitee.com/LongbowEnterprise/BootstrapAdmin/wikis) 查看配置说明小节  

## 常见问题Q&A
请点击 [查看文档](https://gitee.com/LongbowEnterprise/BootstrapAdmin/wikis/常见问题Q&A) 查看常见问题小节  

## 开源协议
[Apache License Version 2.0](https://gitee.com/LongbowEnterprise/BootstrapAdmin/blob/master/LICENSE)

## GVP 奖杯
[查看照片](https://images.gitee.com/uploads/images/2019/0516/124055_96cc9f8d_554725.png "GiteeGVP.png")

## 项目截图

后台首页

![后台首页](https://gitee.com/LongbowEnterprise/Pictures/raw/master/BootstrapAdmin/BA02-01.png "BAHome-01.png")

更多截图请点击 [查看文档](https://gitee.com/LongbowEnterprise/BootstrapAdmin/wikis) 查看项目截图小节  

## 特别鸣谢
1. <a href="https://gitee.com/571183806" target="_blank">**云龙**</a> 提供云服务器搭建在线演示系统
2. <a href="https://gitee.com/Ysmc" target="_blank">**一事冇诚**</a> 对MongoDB数据库提供了详细测试
3. <a href="https://gitee.com/Axxbis" target="_blank">**爱吃油麦菜**</a> 提供云服务器与二级域名搭建备份演示系统、测试环境以及图床

## 参与贡献

1. Fork 本项目
2. 新建 Feat_xxx 分支
3. 提交代码
4. 新建 Pull Request
