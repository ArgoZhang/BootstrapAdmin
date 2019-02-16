# BootstrapAdmin

## 项目介绍
使用HTML5+jQuery + NET Core 2.2 + Bootstrap4.1 + PetaPoco构建的后台管理平台，主要功能如下：  
1. 通过配置与前台网站集成
2. 构建前台系统分层级菜单
3. 提供单点登录
4. 集成系统认证授权模块
5. 提供角色，部门，用户，菜单授权  
角色对用户授权  
角色对菜单授权  
角色对部门授权  
部门对用户授权  
6. 提供字典表用于前台网站个性化配置  
优势：前台系统，不用编写登录、授权、认证模块；只负责编写业务模块即可

## 数据库
数据库支持列表如下：  
1. MSSQL  
2. Oracle  
3. SQLite  
4. MySql  
5. MariaDB  
6. Postgresql  
7. Firebird  
8. MsAccess  
9. MongoDB  

## 功能列表
1. 系统登录   
支持新用户注册
2. 后台管理  
3. 个人中心  
4. 返回前台  
通过字典表配置，可以直接返回前台页面
5. 网站设置  
设置网站标题与页脚；设置网站样式；查看系统缓存
6. 菜单管理  
7. 用户管理  
8. 角色管理  
9. 部门管理  
10. 字典表维护  
11. 站内消息  
12. 任务管理  
13. 通知管理  
14. 系统日志  
15. 程序异常  
16. 工具集合  

## 安装教程
1. 安装 .net core 2.2 sdk [官方网址](http://www.microsoft.com/net/download)
2. 安装 Visual Studio 2017 [官方网址](https://visualstudio.microsoft.com/vs/getting-started/)
3. 获取本项目代码 [BootstrapAdmin](https://gitee.com/LgbAdmin/BootstrapAdmin)
4. 安装数据库  
以微软MSSQL为例，执行解决方案中SQLServer目录（物理硬盘中DatabaseScripts目录下）Install.sql脚本创建数据库
5. 初始化数据  
执行对应目录下InitData.sql脚本
6. 拷贝Longbow.lic文件  
拷贝Scripts目录下Longbow.lic文件到bin目录下
7. 系统登录用户名与口令  
用户名：Admin  
密码：123789  

## 解决方案结构

### Client
前台页面Demo工程

#### Bootstrap.Client
前台Web工程

#### Bootstrap.Client.DataAccess
前台Web工程数据库操作类

### MongoDB
MongoDB数据库Collection导入文件

### MySQL
MySQL数据库脚本与初始化脚本文件

### Postgresql
Postgresql 数据库脚本与初始化脚本文件

### Scripts
Longbow.lic 文件，运行时需要拷贝到工程bin目录下

### SQLite
SQLite 数据库脚本与初始化脚本文件

### SQLServer
SQLServer 数据库脚本与初始化脚本文件

### Bootstrap.Admin
后台管理系统Web工程

### Bootstrap.DataAccess
后台管理系统数据库操作工程

### Bootstrap.DataAccess.MongoDB
后台管理系统MongoDB数据库操作工程

### UnitTest
后台管理系统单元测试

## 配置说明
1. 错误日志配置  
```json
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    },
    "LgbFile": {
      "IncludeScopes": true,
      "LogLevel": {
        "Default": "Error"
      },
      "FileName": "Error\\Log.log"
    }
```
### FileName:   
配置系统错误日志所在文件夹与文件名，以上面代码为例，系统错误日志自动记录在应用程序目录下Error目录内，文件名为Log{Date}.log  
MacOS系统设置为
```json
      "FileName": "Error/Log.log"
```

2. 数据库连接配置  
```json
  "ConnectionStrings": {
    "ba": "Data Source=.;Initial Catalog=BootstrapAdmin;User ID=sa;Password=sa"
  },
  "MongoDB": "BootstrapAdmin",
  "DB": [
    {
      "Enabled": false
    },
    {
      "Enabled": false,
      "ProviderName": "Sqlite",
      "ConnectionStrings": {
        "ba": "Data Source=BootstrapAdmin.db;"
      }
    },
    {
      "Enabled": false,
      "ProviderName": "MySql",
      "ConnectionStrings": {
        "ba": "Server=localhost;Database=BA;Uid=argozhang;Pwd=argo@163.com;SslMode=none;"
      }
    },
    {
      "Enabled": false,
      "ProviderName": "Npgsql",
      "ConnectionStrings": {
        "ba": "Server=localhost;Database=BootstrapAdmin;User ID=argozhang;Password=argo@163.com;"
      }
    },
    {
      "Enabled": true,
      "Widget": "Bootstrap.DataAccess.MongoDB",
      "ConnectionStrings": {
        "ba": "mongodb://10.211.55.2:27017"
      }
    }
  ]
```

### ConnectionStrings:  
默认数据库连接字符串

### DB:  
数据库配置项  
Enabled: &#160; &#160;是否生效  
ProviderName: &#160; &#160;数据库类型，未配置默认使用Microsoft SQLServer  
Widget: &#160; &#160;数据库实体类程序集，此程序集负责具体数据库CRUD操作，未指定时默认使用Bootstrap.DataAccess程序集  
ConnectionStrings: &#160; &#160;数据库连接字符串，未配置使用上面的默认数据库连接字符串  
  
以上面代码为例，由于前面的配置项中的Enabled均为false，最后一个MongoDB配置项为true，则系统采用此配置，使用MongoDB作为系统数据库，根据其配置项内的ConnectionStrings下的数据库连接字符串

### MongoDB:  
MongoDB DatabaseName

3. 跨域访问配置  
```json
  "AllowOrigins": "http://localhost,http://10.15.63.218"
```

### AllowOrigins  
设置第三方网站跨域访问后台管理系统主机头，多个站点使用逗号(,)分割  

4. WebApi JWT Token授权访问配置  
```json
  "TokenValidateOption": {
    "Issuer": "BA",
    "Audience": "api",
    "Expires": 5,
    "SecurityKey": "BootstrapAdmin-V1.1"
  }
```
系统默认提供WebApi JWT Token登录方式，Token获取地址为 [Post]/api/Login，客户端调用添加Header Bear [Token]即可登录  
  
5. 单点登录 SSO 配置  
```json
  "KeyPath": "..\\keys",
  "ApplicationName": "__bd__",
  "ApplicationDiscriminator": "BootstrapAdmin",
  "DisableAutomaticKeyGeneration": false
```  
KeyPath:&#160; &#160;单点登录Key保存路径MacOS设置为
```json
  "KeyPath": "../keys",
```  
ApplicationName:&#160; &#160;应用程序名称，与第三方系统配置文件内的ApplicationName值保持一致  
ApplicationDiscriminator:&#160; &#160;应用程序名称，与第三方系统配置文件内的ApplicationDiscriminator值保持一致  
DisableAutomaticKeyGeneration:&#160; &#160;第三方系统配置文件设置为true，后台管理系统设置为false，这样第三方系统登录自动导航到后台管理系统内实现单点登录入口统一  

#### 前台系统配置
```json
  "AuthHost": "http://localhost:50852",
```
AuthHost:&#160; &#160;配置为本系统地址，如果发布成应用程序，如BA，此处配置应配置成包括PathBase的形式，http://localhost/BA


6. 系统日志保存时间间隔配置  
```json
  "KeepExceptionsPeriod": 12,
  "KeepLogsPeriod": 12,
  "CookieExpiresDays": 7,
```
KeepExceptionsPeriod:&#160; &#160;系统异常日志数据库保留时间间隔，默认为12个月  
KeepLogsPeriod:&#160; &#160;系统操作日志数据库保留时间间隔，默认为7个月  
CookieExpiresDays:&#160; &#160;保存登录信息时间间隔，默认为7天  

7. 系统数据缓存配置
```json
  "LongbowCache": {
    "Enabled": true,
    "CorsItems": [
      {
        "Enabled": true,
        "Key": "ba",
        "Url": "http://localhost/BA/CacheList.axd",
        "Desc": "后台管理数据缓存接口",
        "Self": true
      },
      {
        "Enabled": true,
        "Key": "Pallet",
        "Url": "http://localhost/WebConsole/CacheList.axd",
        "Desc": "托盘组垛系统",
        "Self": false
      }
    ],
    "CacheItems": [
      {
        "Enabled": true,
        "Key": "RoleHelper-RetrieveRolesByUserName",
        "Interval": 600,
        "SlidingExpiration": true,
        "Desc": "指定用户角色数据缓存"
      }
    ]
  }
```  

### LongbowCache:  
此组件负责系统底层数据缓存  
Enabled:&#160; &#160;是否启用数据缓存，true为启用，第一次获取数据时从数据库内获取，然后缓存在内存中，再指定配置的时间内过期，时间单位为秒，以上面代码为例缓存项RoleHelper-RetrieveRolesByUserName将会在内存中缓存600秒  

### CorsItems:
跨域缓存清理配置，由于本项目是后台管理，必定有前台系统页面与之配合(前台页面会调用后台系统的api或者数据)，后台配置更改后前台页面需要使用新的数据，如果开启了缓存，必须要及时更新缓存，否则前台系统数据一定是过期的，本配置小节就是负责跨域数据同步的  

Enabled:&#160; &#160;是否启用跨域缓存清理功能  
Key:&#160; &#160;配置项键值  
Url:&#160; &#160;需要缓存清理的站点地址  
Desc:&#160; &#160;描述信息  
Self:&#160; &#160;是否为系统本身  

#### 本配置非常重要

如果配置未成功，可以暂时将根配置的Enabled设置为false，暂时关闭使用数据缓存功能

### CacheItems:  
Key:&#160; &#160;缓存项键值  
Interval:&#160; &#160;缓存时间，单位为秒  
SlidingExpiration:&#160; &#160;缓存延时设置，true时每次访问后指定秒数后过期，false时第一次访问进行缓存后指定秒数后过期

## 常见问题Q&A
1. 下载源代码后编译不通过报错非常多，都是NETCore系统dll缺失或者不兼容等异常信息  
问题原因:&#160; &#160;NET Core 框架加载不正确，可使用
```console
dotnet --version
```
输入结果为下面为正确
```console
2.2.200-preview-009648
```
命令行查看输出是否为2.2版本，注意后面可能有小版本号如 preview-009648 可以去控制面板程序安装与卸载列表中查看，我给出的示例是我本地环境的真实数据   

解决办法:&#160; &#160;强制项目使用NETCore 2.2 SDK版本  
在项目的根目录打开cmd，执行命令即可：
```console
dotnet new global.json --sdk-version <SDK版本号>
```
global.json 文件内容：  
```json
{
  "sdk": {
    "version": "2.2.200-preview-009648"
  }
}
```
[NETCore SDK版本查询](https://dotnet.microsoft.com/download/dotnet-core/2.2)  

## 项目截图

### 登录截图

1. 手机登录

![输入图片说明](https://images.gitee.com/uploads/images/2018/0925/171428_a581e157_554725.png "Loginm.png")

2. 电脑登录

![输入图片说明](https://images.gitee.com/uploads/images/2018/0925/171640_2224523f_554725.png "Login.png")

3. 前台首页

![输入图片说明](https://images.gitee.com/uploads/images/2018/0925/171658_f480f25f_554725.png "PalletHome.png")

4. 后台首页

![输入图片说明](https://images.gitee.com/uploads/images/2018/0925/171712_d00fae48_554725.png "BAHome.png")

## 参与贡献

1. Fork 本项目
2. 新建 Feat_xxx 分支
3. 提交代码
4. 新建 Pull Request
