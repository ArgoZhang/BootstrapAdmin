# BootstrapAdmin

<span>English</span> | <a href="README.zh-CN.md">中文</a>

---

##### Version & Coverage
[![Release](https://img.shields.io/endpoint.svg?logo=Groupon&logoColor=red&color=green&label=release&url=https://admin.blazor.zone/api/Gitee/Releases?userName=dotnetchina)](https://gitee.com/dotnetchina/BootstrapAdmin/releases)
[![Coveralls](https://img.shields.io/coveralls/github/ArgoZhang/BootstrapAdmin/master.svg?logo=ReverbNation&logoColor=green&label=coveralls)](https://coveralls.io/github/ArgoZhang/BootstrapAdmin)
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

## Introduce
Because the dependent on Bootstrap v4, so it is called **Bootstrap Admin**. This system can be integrated with asp.net and asp.net core applications. The database supports multiple databases at the same time. The detailed list is shown in the following **database** detailed list. Switching the data source only needs to change the configuration file without restarting the application. The configuration is simple and flexible. The UI front-end uses the popular Bootstrap framework layout, which is very compatible with mobile devices and adapts to almost all terminal devices in the current market. The system also has the feature of single background supporting multi-front desk, and provides the ability of **single sign-on (SSO)**.  

### Notes
Bootstrap Admin does not require secondary development, but only integration with the front-end system. The front-end system model project is **Bootstrap. Client**  
The original starting point of the project is to separate the privilege system from the business system. The project development focuses on functions. For detailed configuration instructions, please click on [View Documents](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E7%B3%BB%E7%BB%9F%E9%89B%86%E6%88%90).  

### Features  
1. Integration with Front-end Website through Configuration
2. Constructing Hierarchical Menu of Front-end System
3. Provide a single background to support multiple front-end application configurations
4. Provide single sign-on(SSO)
5. Integrated System Authentication and Authorization Module
6. Provide role, department, user, menu, foreground application authorization
Role Authorization to Users  
Role-to-Menu Authorization  
Role Authorization to Departments  
Role-to-application authorization (multiple front-end applications share a back-end privilege management system)  
Departments Authorize Users
7. Provide dictionary tables for personalized configuration of front-end websites
8. Fully responsive layout (supporting all mainstream devices such as computers, tablets, mobile phones, etc.)
9. Built-in multi-data source support, simple configuration and immediate effect without restart
10. Built-in data memory caching mechanism, page fast response
11. Built-in Data **Operation Log** and User **Log on**  

[Update log](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/更新日志)

### Advantage
1. The front-end system does not need to write login, authorization and authentication modules; it is only responsible for writing business modules.
2. Background system can be used directly without any secondary development.
3. Front-end and back-end systems are separated, which are different systems (domain name can be independent)
4. Extensible to multi-tenant applications

For more information, please click [wiki](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E9%A1%B9%E7%9B%AE%E4%BB%8B%E7%BB%8D)  

### Database 
**MSSQL/Oracle/SQLite/MySql/MariaDB/Firebird/MongoDB**  

For more information, please click [wiki](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/数据库连接配置?sort_id=1333482)  

### Browser
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

### Mobile 
![ios](https://img.shields.io/badge/ios-supported-success.svg?logo=apple&logoColor=white)
![Andriod](https://img.shields.io/badge/andriod-suported-success.svg?logo=android)
![windows](https://img.shields.io/badge/windows-suported-success.svg?logo=windows&logoColor=blue)

|                        |  **Chrome**  |  **Firefox**  |  **Safari**  |  **Android Browser & WebView**  |  **Microsoft Edge**  |
| -------                | ---------    | ---------     | ------       | -------------------------       | --------------       |
|  **iOS**               | Supported    | Supported     | Supported    | N/A                             | Supported            |
|  **Android**           | Supported    | Supported     | N/A          | Android v5.0+ supported         | Supported            |
|  **Windows 10 Mobile** | N/A          | N/A           | N/A          | N/A                             | Supported            |

### Desktop
![macOS](https://img.shields.io/badge/macOS-supported-success.svg?logo=apple&logoColor=white)
![linux](https://img.shields.io/badge/linux-suported-success.svg?logo=linux&logoColor=white)
![windows](https://img.shields.io/badge/windows-suported-success.svg?logo=windows)

|         | Chrome    | Firefox   | Internet Explorer | Microsoft Edge | Opera     | Safari        |
| ------- | --------- | --------- | ----------------- | -------------- | --------- | ------------- |
| Mac     | Supported | Supported | N/A               | N/A            | Supported | Supported     |
| Linux   | Supported | Supported | N/A               | N/A            | N/A       | N/A           |
| Windows | Supported | Supported | Supported, IE10+  | Supported      | Supported | Not supported |

## QQ Group
[![QQ](https://img.shields.io/badge/QQ-795206915-green.svg?logo=tencent%20qq&logoColor=red)](https://shang.qq.com/wpa/qunwpa?idkey=d381355e50ff91db410c3da3eadb081ba859f64c2877e86343f4709b171f28b8)

## Installation
1. Install .net core sdk [Offical website](http://www.microsoft.com/net/download)
2. Install Visual Studio 2019 lastest [Offical website](https://visualstudio.microsoft.com/vs/getting-started/)
3. Git clone `git clone https://gitee.com/dotnetchina/BootstrapAdmin.git`
4. Login as Admin/123789  

## Branchs  
[Details](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/分支说明)

## Online Demonstration   
[![website1](https://img.shields.io/badge/linux-http://ba.zylweb.cn-success.svg?logo=buzzfeed&logoColor=green)](http://ba.zylweb.cn)
[![website2](https://img.shields.io/badge/linux-http://admin.blazor.zone-success.svg?logo=buzzfeed&logoColor=green)](http://admin.blazor.zone)  

### Login  
Administrator: Admin/123789  
User: User/123789

## Docker Images
[![Docker](https://img.shields.io/docker/cloud/automated/argozhang/ba.svg?logo=docker&logoColor=success)](https://hub.docker.com/r/argozhang/ba)
[![Docker](https://img.shields.io/docker/cloud/build/argozhang/ba.svg?logo=docker&logoColor=success)](https://hub.docker.com/r/argozhang/ba/builds)
[![Docker](https://img.shields.io/github/workflow/status/ArgoZhang/BootstrapAdmin/Docker%20Image%20CI/master?label=Docker%20Image%20CI&logo=github&logoColor=green)](https://github.com/ArgoZhang/BootstrapAdmin/actions?query=workflow%3A%22Docker+Image+CI%22%3Amaster)

### Docker Hub 
Mirror [Portal](https://hub.docker.com/r/argozhang/ba)
```bash
docker pull argozhang/ba
```
### Qiniu Cloud:  
Mirror [Portal](https://hub.qiniu.com/store/argozhang/ba)
```bash
docker pull reg.qiniu.com/argozhang/ba
```

## Configurations
Detailed configuration instructions please click [wikis](https://gitee.com/dotnetchina/BootstrapAdmin/wikis)

## Q&A
Please click [wikis](https://gitee.com/dotnetchina/BootstrapAdmin/wikis/%E5%B8%B8%E8%A7%81%E9%97%AE%E9%A2%98Q&A)  

## License
[![Gitee license](https://img.shields.io/github/license/argozhang/bootstrapadmin.svg?logo=git&logoColor=red)](https://gitee.com/dotnetchina/BootstrapAdmin/blob/master/LICENSE)

## GVP award
[View](https://images.gitee.com/uploads/images/2021/0112/112021_9d570be1_554725.png "GiteeGVP.png")

## Screenshots

Home

![Home](https://gitee.com/LongbowEnterprise/Pictures/raw/master/BootstrapAdmin/BA02-01.png "BAHome-01.png")

For more screenshots, Click [wiki](https://gitee.com/dotnetchina/BootstrapAdmin/wikis)

## Contribution

1. Fork
2. Create Feat_xxx branch
3. Commit
4. Create Pull Request

## Donate

If this project is helpful to you, please scan the QR code below for a cup of coffee.

![WeChat](https://gitee.com/LongbowEnterprise/Pictures/raw/master/WeChat/WeChat.png "WeChat")
