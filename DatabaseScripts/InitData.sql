USE [BootstrapAdmin]
GO

DELETE From Users where ID = 1
SET IDENTITY_INSERT [dbo].[Users] ON 
insert into Users (ID, UserName, Password, PassSalt, DisplayName) values (1, 'Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator')
SET IDENTITY_INSERT [dbo].[Users] OFF

DELETE From Dicts where ID in (1, 2)
SET IDENTITY_INSERT [dbo].[Dicts] ON 
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code]) VALUES (1, N'�˵�', N'ϵͳ�˵�', '0')
INSERT [dbo].[Dicts] ([ID], [Category], [Name], [Code]) VALUES (2, N'�˵�', N'�ⲿ�˵�', '1')
SET IDENTITY_INSERT [dbo].[Dicts] OFF

DELETE FROM Navigations where ID in (1, 2, 3, 4, 5, 6, 7)
SET IDENTITY_INSERT [dbo].[Navigations] ON 
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (1, 0, N'�˵�����', 10, N'fa fa-dashboard', N'~/Admin/Menus', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (2, 0, N'�û�����', 20, N'fa fa-user', N'~/Admin/Users', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (3, 0, N'��ɫ����', 30, N'fa fa-sitemap', N'~/Admin/Roles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (4, 0, N'���Ź���', 40, N'fa fa-home', N'~/Admin/Groups', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (5, 0, N'�ֵ��ά��', 50, N'fa fa-book', N'~/Admin/Dicts', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (6, 0, N'���Ի�ά��', 60, N'fa fa-pencil', N'~/Admin/Profiles', N'0')
INSERT [dbo].[Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (7, 0, N'ϵͳ��־', 70, N'fa fa-gears', N'~/Admin/Logs', N'0')
SET IDENTITY_INSERT [dbo].[Navigations] OFF