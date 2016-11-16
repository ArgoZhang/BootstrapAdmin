USE [BootstrapAdmin]
GO

DELETE From Users where ID = 1
SET IDENTITY_INSERT [dbo].[Users] ON 
insert into Users (ID, UserName, Password, PassSalt, DisplayName, RegisterTime, ApprovedTime,ApprovedBy, [Description]) values (1, 'Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator', GetDate(), GetDate(), 'system', N'系统默认创建')
SET IDENTITY_INSERT [dbo].[Users] OFF

DELETE From Dicts
SET IDENTITY_INSERT [dbo].[Dicts] ON 
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (1, N'菜单', N'系统菜单', N'0', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (2, N'菜单', N'外部菜单', N'1', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (3, N'网站设置', N'网站标题', N'后台管理系统', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (4, N'网站设置', N'网站页脚', N'2016 © 通用后台管理系统', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (5, N'系统通知', N'用户注册', N'0', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (6, N'系统通知', N'程序异常', N'1', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (7, N'系统通知', N'数据库连接', N'2', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (8, N'通知状态', N'未处理', N'0', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (9, N'通知状态', N'已处理', N'1', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (10, N'处理结果', N'同意', N'0', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (11, N'处理结果', N'拒绝', N'1', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (12, N'消息状态', N'未读', N'0', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (13, N'消息状态', N'已读', N'1', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (14, N'消息标签', N'一般', N'0', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (15, N'消息标签', N'紧要', N'1', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (16, N'头像地址', N'头像路径', N'~/Content/images/uploader/', 0)
SET IDENTITY_INSERT [dbo].[Dicts] OFF

DELETE FROM Navigations
SET IDENTITY_INSERT [dbo].[Navigations] ON 
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (1, 0, N'后台管理', 10, N'fa fa-gear', N'~/Admin/Index', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (2, 0, N'个人中心', 20, N'fa fa-suitcase', N'~/Admin/Infos', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (3, 0, N'返回前台', 30, N'fa fa-hand-o-left', N'~/Home/Index', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (4, 0, N'网站设置', 40, N'fa fa-fa', N'~/Admin/Profiles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (5, 0, N'菜单管理', 50, N'fa fa-dashboard', N'~/Admin/Menus', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (6, 0, N'用户管理', 60, N'fa fa-user', N'~/Admin/Users', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (7, 0, N'角色管理', 70, N'fa fa-sitemap', N'~/Admin/Roles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (8, 0, N'部门管理', 80, N'fa fa-bank', N'~/Admin/Groups', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (9, 0, N'字典表维护', 90, N'fa fa-book', N'~/Admin/Dicts', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (10, 0, N'站内消息', 100, N'fa fa-envelope-o', N'~/Admin/Messages', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (11, 0, N'任务消息', 110, N'fa fa fa-tasks', N'~/Admin/Tasks', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (12, 0, N'通知管理', 120, N'fa fa-bell-o', N'~/Admin/Notifications', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (13, 0, N'系统日志', 130, N'fa fa-gears', N'~/Admin/Logs', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (14, 0, N'程序异常', 140, N'fa fa-cubes', N'~/Admin/Exceptions', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (15, 0, N'锁定屏幕', 150, N'fa fa-lock', N'~/Home/Lock', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (16, 0, N'锁定屏幕', 10, N'fa fa-lock', N'~/Home/Lock', N'1')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (17, 16, N'锁定屏幕', 10, N'fa fa-lock', N'~/Home/Lock', N'1')
SET IDENTITY_INSERT [dbo].[Navigations] OFF

DELETE FROM GROUPS WHERE ID = 1
SET IDENTITY_INSERT [dbo].[Groups] ON 
INSERT [dbo].[Groups] ([ID], [GroupName], [Description]) VALUES (1, 'Admin', N'系统默认组')
SET IDENTITY_INSERT [dbo].[Groups] OFF

DELETE FROM Roles where ID in (1, 2)
SET IDENTITY_INSERT [dbo].[Roles] ON 
INSERT [dbo].[Roles] ([ID], [RoleName], [Description]) VALUES (1, N'Administrators', N'系统管理员')
INSERT [dbo].[Roles] ([ID], [RoleName], [Description]) VALUES (2, N'Default', N'默认用户，可访问前台页面')
SET IDENTITY_INSERT [dbo].[Roles] OFF

DELETE FROM RoleGroup where ID in (1)
SET IDENTITY_INSERT [dbo].[RoleGroup] ON 
INSERT [dbo].[RoleGroup] ([ID], [RoleID], [GroupID]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[RoleGroup] OFF

DELETE FROM UserGroup where ID in (1)
SET IDENTITY_INSERT [dbo].[UserGroup] ON 
INSERT [dbo].[UserGroup] ([ID], [UserID], [GroupID]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[UserGroup] OFF

DELETE FROM UserRole where ID in (1)
SET IDENTITY_INSERT [dbo].[UserRole] ON 
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[UserRole] OFF

DELETE FROM NavigationRole
SET IDENTITY_INSERT [dbo].[NavigationRole] ON 
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (1, 1, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (2, 2, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (3, 3, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (4, 4, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (5, 5, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (6, 6, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (7, 7, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (8, 8, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (9, 9, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (10, 10, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (11, 11, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (12, 12, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (13, 13, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (14, 14, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (15, 15, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (16, 16, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (17, 17, 1)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (18, 1, 2)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (19, 2, 2)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (20, 3, 2)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (21, 16, 2)
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (22, 17, 2)
SET IDENTITY_INSERT [dbo].[NavigationRole] OFF