USE [BootstrapAdmin]
GO

DELETE From Users where ID = 1
SET IDENTITY_INSERT [dbo].[Users] ON 
insert into Users (ID, UserName, Password, PassSalt, DisplayName) values (1, 'Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator')
SET IDENTITY_INSERT [dbo].[Users] OFF

DELETE From Dicts where ID in (1, 2)
SET IDENTITY_INSERT [dbo].[Dicts] ON 
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (1, N'菜单', N'系统菜单', N'0', 0)
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (2, N'菜单', N'外部菜单', N'1', 0)
SET IDENTITY_INSERT [dbo].[Dicts] OFF

DELETE FROM Navigations where ID in (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
SET IDENTITY_INSERT [dbo].[Navigations] ON 
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (1, 0, N'后台管理', 1, N'fa fa-gear', N'~/Admin/Index', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (2, 0, N'菜单管理', 10, N'fa fa-dashboard', N'~/Admin/Menus', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (3, 0, N'用户管理', 20, N'fa fa-user', N'~/Admin/Users', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (4, 0, N'角色管理', 30, N'fa fa-sitemap', N'~/Admin/Roles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (5, 0, N'部门管理', 40, N'fa fa-home', N'~/Admin/Groups', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (6, 0, N'字典表维护', 50, N'fa fa-book', N'~/Admin/Dicts', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (7, 0, N'个性化维护', 60, N'fa fa-pencil', N'~/Admin/Profiles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (8, 0, N'系统日志', 70, N'fa fa-gears', N'~/Admin/Logs', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (9, 0, N'通知管理', 80, N'fa fa-bell-o', N'~/Admin/News', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (10, 0, N'个人中心', 90, N'fa fa-suitcase', N'~/Admin/Infos', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (11, 0, N'返回前台', 100, N'fa fa-hand-o-left', N'~/Home/Index', N'0')
SET IDENTITY_INSERT [dbo].[Navigations] OFF

DELETE FROM NavigationRole where ID in (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)
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
INSERT [dbo].[NavigationRole] ([ID], [NavigationID], [RoleID]) VALUES (12, 11, 2)
SET IDENTITY_INSERT [dbo].[NavigationRole] OFF

DELETE FROM RoleGroup where ID in (1)
SET IDENTITY_INSERT [dbo].[RoleGroup] ON 
INSERT [dbo].[RoleGroup] ([ID], [RoleID], [GroupID]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[RoleGroup] OFF

DELETE FROM Roles where ID in (1, 2)
SET IDENTITY_INSERT [dbo].[Roles] ON 
INSERT [dbo].[Roles] ([ID], [RoleName], [Description]) VALUES (1, N'Administrators', N'系统管理员')
INSERT [dbo].[Roles] ([ID], [RoleName], [Description]) VALUES (2, N'Default', N'默认用户，可访问前台页面')
SET IDENTITY_INSERT [dbo].[Roles] OFF

DELETE FROM UserGroup where ID in (1)
SET IDENTITY_INSERT [dbo].[UserGroup] ON 
INSERT [dbo].[UserGroup] ([ID], [UserID], [GroupID]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[UserGroup] OFF

DELETE FROM UserRole where ID in (1)
SET IDENTITY_INSERT [dbo].[UserRole] ON 
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
