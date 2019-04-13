USE [BootstrapAdmin]
GO

DELETE From Users where ID = 1
SET IDENTITY_INSERT [dbo].[Users] ON 
-- ADMIN/123789
insert into Users (ID, UserName, Password, PassSalt, DisplayName, RegisterTime, ApprovedTime,ApprovedBy, [Description]) values (1, 'Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator', GetDate(), GetDate(), 'system', N'系统默认创建')
SET IDENTITY_INSERT [dbo].[Users] OFF

DELETE From Dicts Where Define = 0
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'菜单', N'系统菜单', N'0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'菜单', N'外部菜单', N'1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'应用程序', N'未设置', N'0', 0)
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
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'当前样式', N'使用样式', N'blue.css', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'网站设置', N'前台首页', N'~/Home/Index', 0)

-- 时长单位 月
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'程序异常保留时长', '1', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'操作日志保留时长', '12', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'登录日志保留时长', '12', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'访问日志保留时长', '1', 0)

-- 时长单位 天
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'Cookie保留时长', '7', 0)

INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'IP地理位置接口', 'None', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'BaiDuIPSvr', 'http://api.map.baidu.com/location/ip?ak=6lvVPMDlm2gjLpU0aiqPsHXi2OiwGQRj&ip=', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'JuheIPSvr', 'http://apis.juhe.cn/ip/ipNew?key=f57102d1b9fadd3f4a1c29072d0c0206&ip=', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'演示系统', '0', 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'系统设置', N'验证码图床', 'https://longbow-1258823021.cos.ap-shanghai.myqcloud.com/pic/280/150/', 0)

DELETE FROM Navigations Where Category = N'0'
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'后台管理', 10, N'fa fa-gear', N'~/Admin/Index', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'个人中心', 20, N'fa fa-suitcase', N'~/Admin/Profiles', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'保存显示名称', 10, 'fa fa-fa', 'saveDisplayName', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'保存密码', 20, 'fa fa-fa', 'savePassword', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'保存应用', 30, 'fa fa-fa', 'saveApp', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'保存样式', 40, 'fa fa-fa', 'saveTheme', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'保存头像', 50, 'fa fa-fa', 'saveIcon', '0', 2);
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'返回前台', 30, N'fa fa-hand-o-left', N'~/Home/Index', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'网站设置', 40, N'fa fa-fa', N'~/Admin/Settings', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'保存系统名称', 10, 'fa fa-fa', 'saveTitle', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'保存页脚设置', 20, 'fa fa-fa', 'saveFooter', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'保存样式', 30, 'fa fa-fa', 'saveTheme', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'清理缓存', 40, 'fa fa-fa', 'clearCache', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'清理全部缓存', 50, 'fa fa-fa', 'clearAllCache', '0', 2);
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'菜单管理', 50, N'fa fa-dashboard', N'~/Admin/Menus', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配角色', 40, 'fa fa-fa', 'assignRole', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (0, N'图标页面', 50, 'fa fa-fa', '~/Admin/IconView', '0', 1);
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'用户管理', 60, N'fa fa-user', N'~/Admin/Users', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配部门', 40, 'fa fa-fa', 'assignGroup', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'分配角色', 50, 'fa fa-fa', 'assignRole', '0', 2);
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'角色管理', 70, N'fa fa-sitemap', N'~/Admin/Roles', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配用户', 40, 'fa fa-fa', 'assignUser', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'分配部门', 50, 'fa fa-fa', 'assignGroup', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 5, N'分配菜单', 60, 'fa fa-fa', 'assignMenu', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 6, N'分配应用', 70, 'fa fa-fa', 'assignApp', '0', 2);
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'部门管理', 80, N'fa fa-bank', N'~/Admin/Groups', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 3, N'分配用户', 40, 'fa fa-fa', 'assignUser', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 4, N'分配角色', 50, 'fa fa-fa', 'assignRole', '0', 2);
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'字典表维护', 90, N'fa fa-book', N'~/Admin/Dicts', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'新增', 10, 'fa fa-fa', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 1, N'编辑', 20, 'fa fa-fa', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity - 2, N'删除', 30, 'fa fa-fa', 'del', '0', 2);
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'站内消息', 100, N'fa fa-envelope', N'~/Admin/Messages', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'任务管理', 110, N'fa fa fa-tasks', N'~/Admin/Tasks', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'通知管理', 120, N'fa fa-bell', N'~/Admin/Notifications', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'日志管理', 130, N'fa fa-gears', N'#', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity, N'操作日志', 10, N'fa fa-edit', N'~/Admin/Logs', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 1, N'登录日志', 20, N'fa fa-user-circle-o', N'~/Admin/Logins', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 2, N'访问日志', 30, N'fa fa-bars', N'~/Admin/Traces', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'在线用户', 140, N'fa fa-users', N'~/Admin/Online', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'程序异常', 150, N'fa fa-cubes', N'~/Admin/Exceptions', N'0')
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (@@Identity, N'服务器日志', 10, N'fa fa-fa', N'log', N'0', 2)
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, N'工具集合', 160, N'fa fa-gavel', N'#', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity, N'客户端测试', 10, N'fa fa-wrench', N'~/Admin/Mobile', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 1, N'API文档', 10, N'fa fa-wrench', N'~/swagger', N'0')
INSERT [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (@@Identity - 2, N'图标集', 10, N'fa fa-dashboard', N'~/Admin/FAIcon', N'0')

DELETE FROM GROUPS WHERE ID = 1
SET IDENTITY_INSERT [dbo].[Groups] ON 
INSERT [dbo].[Groups] ([ID], [GroupName], [Description]) VALUES (1, 'Admin', N'系统默认组')
SET IDENTITY_INSERT [dbo].[Groups] OFF

DELETE FROM Roles where ID in (1, 2)
SET IDENTITY_INSERT [dbo].[Roles] ON 
INSERT [dbo].[Roles] ([ID], [RoleName], [Description]) VALUES (1, N'Administrators', N'系统管理员')
INSERT [dbo].[Roles] ([ID], [RoleName], [Description]) VALUES (2, N'Default', N'默认用户，可访问前台页面')
SET IDENTITY_INSERT [dbo].[Roles] OFF

DELETE FROM RoleGroup Where RoleID = 1
INSERT [dbo].[RoleGroup] ([RoleID], [GroupID]) VALUES (1, 1)

DELETE FROM UserGroup Where UserID = 1
INSERT [dbo].[UserGroup] ([UserID], [GroupID]) VALUES (1, 1)

DELETE FROM UserRole Where UserID = 1
INSERT [dbo].[UserRole] ([UserID], [RoleID]) VALUES (1, 1)
INSERT [dbo].[UserRole] ([UserID], [RoleID]) VALUES (1, 2)

DELETE FROM NavigationRole

-- Client Data
Declare @AppId nvarchar(1)
set @AppId = N'2'
declare @AppName nvarchar(8)
set @AppName = N'测试平台'

Delete From [dbo].[Dicts] Where Category = N'应用程序' and Code = @AppId
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'应用程序', @AppName, @AppId, 0)
INSERT [dbo].[Dicts] ([Category], [Name], [Code], [Define]) VALUES (N'应用首页', @AppId, 'http://localhost:49185/', 0)

Delete From [dbo].[Dicts] Where Category = @AppName
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'网站标题', N'托盘组垛程序', 1);
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'网站页脚', N'通用后台管理测试平台', 1);
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'个人中心地址', N'http://localhost:50852/Admin/Profiles', 1);
Insert Dicts (Category, Name, Code, Define) values (@AppName, N'系统设置地址', N'http://localhost:50852/Admin/Index', 1);

-- 菜单
DELETE FROM Navigations Where [Application] = @AppId
INSERT [dbo].[Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, N'首页', 10, N'fa fa-fa', N'~/Home/Index', N'1', @AppId)

INSERT [dbo].[Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, N'测试页面', 10, N'fa fa-fa', N'~/Home/Index', N'1', @AppId)
INSERT [dbo].[Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (@@Identity, N'关于', 10, N'fa fa-fa', N'~/Home/Index', N'1', @AppId)

-- 菜单授权
DELETE FROM NavigationRole Where NavigationID in (Select ID From Navigations Where [Application] = @AppId)
INSERT INTO NavigationRole SELECT ID, 2 FROM Navigations Where [Application] = @AppId
