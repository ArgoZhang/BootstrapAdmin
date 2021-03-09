USE [BootstrapAdmin]
GO

-- ADMIN/123789
-- User/123789
DELETE From Users where UserName in ('Admin', 'User')
INSERT INTO Users (UserName, Password, PassSalt, DisplayName, RegisterTime, ApprovedTime, ApprovedBy, [Description]) values ('Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator', GetDate(), GetDate(), 'system', N'系统默认创建')
INSERT INTO Users (UserName, Password, PassSalt, DisplayName, RegisterTime, ApprovedTime, ApprovedBy, [Description], App) values ('User', 'tXG/yNffpnm6cThrCH7wf6jN1ic3VHvLoY4OrzKtrZ4=', 'c5cIrRMn8XjB84M/D/X7Lg9uUqQFmYNEdxb/4HWH8OLa4pNZ', N'测试账号', GetDate(), GetDate(), 'system', N'系统默认创建', 'Demo')

DELETE From Dicts Where Define = 0
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'菜单', N'系统菜单', N'0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'菜单', N'外部菜单', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'应用程序', N'后台管理', N'BA', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'网站标题', N'后台管理系统', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'网站页脚', N'2016 © 通用后台管理系统', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统通知', N'用户注册', N'0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统通知', N'程序异常', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统通知', N'数据库连接', N'2', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'通知状态', N'未处理', N'0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'通知状态', N'已处理', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'处理结果', N'同意', N'0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'处理结果', N'拒绝', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'消息状态', N'未读', N'0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'消息状态', N'已读', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'消息标签', N'一般', N'0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'消息标签', N'紧要', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'头像地址', N'头像路径', N'~/images/uploader/', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'头像地址', N'头像文件', N'default.jpg', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站样式', N'蓝色样式', N'blue.css', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站样式', N'黑色样式', N'black.css', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站样式', N'AdminLTE', N'lte.css', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'使用样式', N'blue.css', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'前台首页', N'~/Home/Index', 0)

-- 网站UI设置
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'侧边栏状态', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'卡片标题状态', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'固定表头', N'1', 0)

-- 登录配置
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'短信验证码登录', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'OAuth 认证登录', N'1', 0)

-- 自动锁屏（秒）默认 30 秒
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'自动锁屏时长', N'30', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'自动锁屏', N'0', 0)

-- 是否启用 Blazor 默认为 0 未启用
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'Blazor', N'0', 0)

-- 是否启用 健康检查 默认为 0 未启用 1 启用
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'健康检查', N'1', 0);

-- 时长单位 月
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'程序异常保留时长', '1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'操作日志保留时长', '12', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'登录日志保留时长', '12', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'访问日志保留时长', '1', 0)

-- 时长单位 天
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'Cookie保留时长', '7', 0)

-- 地理位置接口
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'IP地理位置接口', 'None', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'地理位置服务', N'百度地图开放平台', 'BaiDuIPSvr', 0);
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'地理位置服务', N'聚合地理位置', 'JuheIPSvr', 0);
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'地理位置服务', N'百度138地理位置', 'BaiDuIP138Svr', 0);

INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'地理位置', N'BaiDuIPSvr', 'http://api.map.baidu.com/location/ip?ak=6lvVPMDlm2gjLpU0aiqPsHXi2OiwGQRj&ip=', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'地理位置', N'JuheIPSvr', 'http://apis.juhe.cn/ip/ipNew?key=f57102d1b9fadd3f4a1c29072d0c0206&ip=', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'地理位置', N'BaiDuIP138Svr', 'https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?resource_id=6006&query=', 0)

-- 时长单位 分钟
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'IP请求缓存时长', '10', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'演示系统', '0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'授权盐值', 'yjglE2eddCGcS7tTFTDd2DfvqXHgCnMhNhpmx9HJaC9l8GAZ', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'哈希结果', '6jTT50HGuk8V+AIsiE4IfqjcER71PBN1DY7gqOLZE7E=', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'验证码图床', 'http://imgs.sdgxgz.com/images/', 0)

INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'默认应用程序', '0', 0)

INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'后台地址', 'http://localhost:50852', 0)

-- 系统登录首页设置
INSERT INTO Dicts (Category, Name, Code, Define) VALUES (N'系统首页', N'高仿码云', N'Login-Gitee', 1);
INSERT INTO Dicts (Category, Name, Code, Define) VALUES (N'系统首页', N'蓝色清新', N'Login-Blue', 1);
INSERT INTO Dicts (Category, Name, Code, Define) VALUES (N'系统首页', N'系统默认', N'Login', 1);
INSERT INTO Dicts (Category, Name, Code, Define) VALUES (N'系统首页', N'科技动感', N'Login-Tec', 1);
INSERT INTO Dicts (Category, Name, Code, Define) VALUES (N'系统首页', N'Admin-LTE', N'Login-LTE', 1);

INSERT INTO Dicts (Category, Name, Code, Define) VALUES (N'网站设置', N'登录界面', N'Login', 1);

DELETE FROM Navigations Where Category = N'0'
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'后台管理', 10, N'fa fa-gear', N'~/Admin/Index', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'个人中心', 20, N'fa fa-suitcase', N'~/Admin/Profiles', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'保存显示名称', 10, 'fa fa-fa', 'saveDisplayName', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'保存密码', 20, 'fa fa-fa', 'savePassword', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'保存应用', 30, 'fa fa-fa', 'saveApp', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'保存样式', 40, 'fa fa-fa', 'saveTheme', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'保存头像', 50, 'fa fa-fa', 'saveIcon', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 5, N'保存网站设置', 60, 'fa fa-fa', 'saveUISettings', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'系统锁屏', 25, N'fa fa-television', N'~/Account/Lock', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'返回前台', 30, N'fa fa-hand-o-left', N'~/Home/Index', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'网站设置', 40, N'fa fa-fa', N'~/Admin/Settings', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'保存系统名称', 10, 'fa fa-fa', 'saveTitle', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'保存页脚设置', 20, 'fa fa-fa', 'saveFooter', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'保存样式', 30, 'fa fa-fa', 'saveTheme', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'清理缓存', 40, 'fa fa-fa', 'clearCache', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'清理全部缓存', 50, 'fa fa-fa', 'clearAllCache', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 5, N'登录设置', 60, 'fa fa-fa', 'loginSettings', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 6, N'自动锁屏', 70, 'fa fa-fa', 'lockScreen', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 7, N'默认应用', 80, 'fa fa-fa', 'defaultApp', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'菜单管理', 50, N'fa fa-dashboard', N'~/Admin/Menus', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配角色', 40, 'fa fa-fa', 'assignRole', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (0, N'图标页面', 50, 'fa fa-fa', '~/Admin/IconView', '0', 1);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (0, N'侧边栏', 55, 'fa fa-fa', '~/Admin/Sidebar', '0', 1);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'用户管理', 60, N'fa fa-user', N'~/Admin/Users', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配部门', 40, 'fa fa-fa', 'assignGroup', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'分配角色', 50, 'fa fa-fa', 'assignRole', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'角色管理', 70, N'fa fa-sitemap', N'~/Admin/Roles', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配用户', 40, 'fa fa-fa', 'assignUser', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'分配部门', 50, 'fa fa-fa', 'assignGroup', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 5, N'分配菜单', 60, 'fa fa-fa', 'assignMenu', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 6, N'分配应用', 70, 'fa fa-fa', 'assignApp', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'部门管理', 80, N'fa fa-bank', N'~/Admin/Groups', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配用户', 40, 'fa fa-fa', 'assignUser', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'分配角色', 50, 'fa fa-fa', 'assignRole', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'字典表维护', 90, N'fa fa-book', N'~/Admin/Dicts', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'站内消息', 100, N'fa fa-envelope', N'~/Admin/Messages', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'任务管理', 110, N'fa fa fa-tasks', N'~/Admin/Tasks', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'暂停', 10, 'fa fa-fa', 'pause', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'日志', 20, 'fa fa-fa', 'info', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'通知管理', 120, N'fa fa-bell', N'~/Admin/Notifications', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'日志管理', 130, N'fa fa-gears', N'#', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity, N'操作日志', 10, N'fa fa-edit', N'~/Admin/Logs', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 1, N'登录日志', 20, N'fa fa-user-circle-o', N'~/Admin/Logins', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 2, N'访问日志', 30, N'fa fa-bars', N'~/Admin/Traces', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 3, N'SQL日志', 40, N'fa fa-database', N'~/Admin/SQL', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'在线用户', 140, N'fa fa-users', N'~/Admin/Online', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'网站分析', 145, N'fa fa-line-chart', N'~/Admin/Analyse', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'程序异常', 150, N'fa fa-cubes', N'~/Admin/Exceptions', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'服务器日志', 10, N'fa fa-fa', N'log', N'0', 2)
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'健康检查', 155, N'fa fa-heartbeat', N'~/Admin/Healths', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'工具集合', 160, N'fa fa-gavel', N'#', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity, N'客户端测试', 10, N'fa fa-wrench', N'~/Admin/Mobile', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 1, N'API文档', 10, N'fa fa-wrench', N'~/swagger', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 2, N'图标集', 10, N'fa fa-dashboard', N'~/Admin/FAIcon', N'0')

DELETE FROM GROUPS WHERE GroupName = 'Admin'
INSERT [dbo].[Groups] ([GroupCode], [GroupName], [Description]) VALUES ('001', 'Admin', N'系统默认组')

DELETE FROM Roles where RoleName in ('Administrators', 'Default')
INSERT [dbo].[Roles] ([RoleName], [Description]) VALUES (N'Administrators', N'系统管理员')
INSERT [dbo].[Roles] ([RoleName], [Description]) VALUES (N'Default', N'默认用户，可访问前台页面')

-- 角色部门关联
TRUNCATE Table RoleGroup
INSERT INTO RoleGroup (GroupId, RoleId) SELECT g.Id, r.Id From Groups g left join Roles r on 1=1 where GroupName = 'Admin' and RoleName = N'Administrators'

-- 用户部门关联
TRUNCATE Table UserGroup

-- 用户角色关联
TRUNCATE Table UserRole
INSERT INTO UserRole (UserId, RoleId) SELECT u.Id, r.Id From Users u left join Roles r on 1=1 where UserName = 'Admin' and RoleName = N'Administrators'
INSERT INTO UserRole (UserId, RoleId) SELECT u.Id, r.Id From Users u left join Roles r on 1=1 where UserName = 'User' and RoleName = N'Default'

-- 角色菜单关联
TRUNCATE Table NavigationRole
INSERT INTO NavigationRole (NavigationID, RoleID) SELECT n.Id, r.Id FROM Navigations n left join Roles r on 1=1 Where RoleName = 'Administrators'
INSERT INTO NavigationRole (NavigationID, RoleID) SELECT n.Id, r.Id FROM Navigations n left join Roles r on 1=1 where RoleName = 'Default' and Name in (N'后台管理', N'个人中心', N'返回前台', N'通知管理')
INSERT INTO NavigationRole (NavigationID, RoleID) SELECT n.Id, r.Id FROM Navigations n left join Roles r on 1=1 where RoleName = 'Default' and ParentId in (select Id from Navigations where Name in (N'个人中心'))

-- Client Data
Declare @AppId nvarchar(50)
set @AppId = N'Demo'
declare @AppName nvarchar(50)
set @AppName = N'测试平台'

Delete From [dbo].[Dicts] Where Category = N'应用程序' and Code = @AppId
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'应用程序', @AppName, @AppId, 0)
Delete From [Dicts] Where Category = '应用首页' and Name = @AppId
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'应用首页', @AppId, 'http://localhost:49185', 0)

Delete From [dbo].[Dicts] Where Category = @AppName
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'网站标题', N'前台演示程序', 1);
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'网站页脚', N'前台演示程序后台权限管理框架', 1);
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'个人中心地址', N'/Admin/Profiles', 1);
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'系统设置地址', N'/Admin/Index', 1);
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'系统通知地址', N'/Admin/Notifications', 1);
INSERT Dicts (Category, Name, Code, Define) VALUES (@AppName, N'favicon', N'/favicon.ico', 1);
INSERT Dicts (Category, Name, Code, Define) VALUES (@AppName, N'网站图标', '/favicon.png', 1);

-- 菜单
DELETE FROM Navigations Where [Application] = @AppId
INSERT [dbo].[Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, N'首页', 10, N'fa fa-fa', N'~/Home/Index', N'1', @AppId)
INSERT [dbo].[Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, N'测试页面', 10, N'fa fa-fa', N'~/Home/Index', N'1', @AppId)
INSERT [dbo].[Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (@@Identity, N'关于', 10, N'fa fa-fa', N'~/Home/Index', N'1', @AppId)

INSERT into [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, N'返回码云', 20, 'fa fa-fa', 'https://gitee.com/dotnetchina/BootstrapAdmin', '1', @AppId)

-- 菜单授权
INSERT INTO NavigationRole SELECT n.ID, r.ID FROM Navigations n left join Roles r on 1=1 Where r.RoleName = 'Default' and [Application] = @AppId

-- 角色对应用授权
DELETE From RoleApp where AppId = @AppId;
INSERT INTO RoleApp (AppId, RoleId) SELECT @AppId, ID From Roles Where RoleName = 'Default'
INSERT INTO RoleApp (AppId, RoleId) SELECT 'BA', ID From Roles Where RoleName = 'Default'
