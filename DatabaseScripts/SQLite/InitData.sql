DELETE From Users where ID = 1;
-- ADMIN/123789
INSERT INTO Users (ID, UserName, [Password], PassSalt, DisplayName, RegisterTime, ApprovedTime,ApprovedBy, [Description]) values (1, 'Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator', datetime(CURRENT_TIMESTAMP, 'localtime'), datetime(CURRENT_TIMESTAMP, 'localtime'), 'system', '系统默认创建');

DELETE From Dicts;
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '菜单', '系统菜单', '0', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '菜单', '外部菜单', '1', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '应用程序', '未设置', '0', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '网站设置', '网站标题', '后台管理系统', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '网站设置', '网站页脚', '2016 © 通用后台管理系统', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '系统通知', '用户注册', '0', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '系统通知', '程序异常', '1', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '系统通知', '数据库连接', '2', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '通知状态', '未处理', '0', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '通知状态', '已处理', '1', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '处理结果', '同意', '0', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '处理结果', '拒绝', '1', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '消息状态', '未读', '0', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '消息状态', '已读', '1', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '消息标签', '一般', '0', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '消息标签', '紧要', '1', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '头像地址', '头像路径', '~/images/uploader/', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '头像地址', '头像文件', 'default.jpg', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '网站样式', '蓝色样式', 'blue.css', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '网站样式', '黑色样式', 'black.css', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '当前样式', '使用样式', 'blue.css', 0);
INSERT INTO [Dicts] ([ID], [Category], [Name], [Code], [Define]) VALUES (NULL, '网站设置', '前台首页', '~/Home/Index', 0);

DELETE FROM Navigations;
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (1, 0, '后台管理', 10, 'fa fa-gear', '~/Admin/Index', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (2, 0, '个人中心', 20, 'fa fa-suitcase', '~/Admin/Profiles', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (3, 0, '返回前台', 30, 'fa fa-hand-o-left', '~/Home/Index', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (4, 0, '网站设置', 40, 'fa fa-fa', '~/Admin/Settings', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (5, 0, '菜单管理', 50, 'fa fa-dashboard', '~/Admin/Menus', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (6, 0, '用户管理', 60, 'fa fa-user', '~/Admin/Users', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (7, 0, '角色管理', 70, 'fa fa-sitemap', '~/Admin/Roles', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (8, 0, '部门管理', 80, 'fa fa-bank', '~/Admin/Groups', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (9, 0, '字典表维护', 90, 'fa fa-book', '~/Admin/Dicts', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (10, 0, '站内消息', 100, 'fa fa-envelope', '~/Admin/Messages', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (11, 0, '任务管理', 110, 'fa fa fa-tasks', '~/Admin/Tasks', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (12, 0, '通知管理', 120, 'fa fa-bell', '~/Admin/Notifications', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (13, 0, '系统日志', 130, 'fa fa-gears', '~/Admin/Logs', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (14, 0, '程序异常', 140, 'fa fa-cubes', '~/Admin/Exceptions', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (16, 0, '工具集合', 160, 'fa fa-gavel', '#', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (17, 16, '客户端测试', 10, 'fa fa-wrench', '~/Admin/Mobile', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (18, 16, 'API文档', 10, 'fa fa-wrench', '~/swagger', '0');
INSERT INTO [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (19, 16, '图标集', 10, 'fa fa-dashboard', '~/Admin/FAIcon', '0');

DELETE FROM GROUPS WHERE ID = 1;
INSERT INTO [Groups] ([ID], [GroupName], [Description]) VALUES (1, 'Admin', '系统默认组');

DELETE FROM Roles where ID in (1, 2);
INSERT INTO [Roles] ([ID], [RoleName], [Description]) VALUES (1, 'Administrators', '系统管理员');
INSERT INTO [Roles] ([ID], [RoleName], [Description]) VALUES (2, 'Default', '默认用户，可访问前台页面');

DELETE FROM RoleGroup;
INSERT INTO [RoleGroup] ([RoleID], [GroupID]) VALUES (1, 1);

DELETE FROM UserGroup;
INSERT INTO [UserGroup] ([UserID], [GroupID]) VALUES (1, 1);

DELETE FROM UserRole;
INSERT INTO [UserRole] ([UserID], [RoleID]) VALUES (1, 1);
INSERT INTO [UserRole] ([UserID], [RoleID]) VALUES (1, 2);

DELETE FROM NavigationRole;
INSERT INTO NavigationRole SELECT NULL, ID, 1 FROM navigations;
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (1, 2);
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (2, 2);
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (3, 2);
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (10, 2);
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (16, 2);
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (17, 2);
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (18, 2);
INSERT INTO [NavigationRole] ([NavigationID], [RoleID]) VALUES (19, 2);

-- Client Data
Delete From [Dicts] Where Category = '应用程序' and Code = 2;
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('应用程序', '测试平台', 2, 0);

Delete From [Dicts] Where Category = '测试平台';
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '网站标题', '前台演示系统', 1);
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '网站页脚', '通用后台管理测试平台', 1);
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '个人中心地址', 'http://localhost:50852/Admin/Profiles', 1);
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '系统设置地址', 'http://localhost:50852/Admin/Settings', 1);

Delete from [Navigations] where Application = 2;
INSERT into [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (NULL, 0, '首页', 10, 'fa fa-fa', '~/Home/Index', '1', 2);

INSERT into [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (NULL, 0, '测试页面', 20, 'fa fa-fa', '#', '1', 2);
INSERT into [Navigations] ([ID], [ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (NULL, last_insert_rowid(), '关于', 10, 'fa fa-fa', '~/Home/About', '1', 2);

-- 菜单授权
DELETE FROM NavigationRole Where NavigationID in (Select ID From Navigations Where [Application] = 2);
INSERT INTO NavigationRole SELECT NULL, ID, 2 FROM Navigations Where [Application] = 2;
