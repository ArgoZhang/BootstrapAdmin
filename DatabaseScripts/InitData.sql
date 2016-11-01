USE [BootstrapAdmin]
GO

DELETE From Users where ID = 1
SET IDENTITY_INSERT [dbo].[Users] ON 
insert into Users (ID, UserName, Password, PassSalt, DisplayName) values (1, 'Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator')
SET IDENTITY_INSERT [dbo].[Users] OFF

DELETE From Dicts where ID in (1, 2)
SET IDENTITY_INSERT [dbo].[Dicts] ON 
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code]) VALUES (1, N'菜单', N'系统菜单', N'0')
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code]) VALUES (2, N'菜单', N'外部菜单', N'1')
SET IDENTITY_INSERT [dbo].[Dicts] OFF

DELETE FROM Navigations where ID in (1, 2, 3, 4, 5, 6, 7, 8)
SET IDENTITY_INSERT [dbo].[Navigations] ON 
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (1, 0, N'菜单管理', 10, N'fa fa-dashboard', N'~/Admin/Menus', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (2, 0, N'用户管理', 20, N'fa fa-user', N'~/Admin/Users', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (3, 0, N'角色管理', 30, N'fa fa-sitemap', N'~/Admin/Roles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (4, 0, N'部门管理', 40, N'fa fa-home', N'~/Admin/Groups', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (5, 0, N'字典表维护', 50, N'fa fa-book', N'~/Admin/Dicts', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (6, 0, N'个性化维护', 60, N'fa fa-pencil', N'~/Admin/Profiles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (7, 0, N'系统日志', 70, N'fa fa-gears', N'~/Admin/Logs', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (8, 0, N'返回前台', 80, N'fa fa-hand-o-left', N'~/Home', N'0')
SET IDENTITY_INSERT [dbo].[Navigations] OFF