-- ADMIN/123789
-- User/123789
DELETE From Users where UserName in ('Admin', 'User');
INSERT INTO Users (UserName, Password, PassSalt, DisplayName, RegisterTime, ApprovedTime, ApprovedBy, [Description]) values ('Admin', 'Es7WVgNsJuELwWK8daCqufUBknCsSC0IYDphQZAiGOo=', 'W5vpBEOYRGHkQXatN0t+ECM/U8cHDuEgrq56+zZBk4J481xH', 'Administrator', datetime(CURRENT_TIMESTAMP, 'localtime'), datetime(CURRENT_TIMESTAMP, 'localtime'), 'system', '系统默认创建');
INSERT INTO Users (UserName, Password, PassSalt, DisplayName, RegisterTime, ApprovedTime, ApprovedBy, [Description], [App]) values ('User', 'tXG/yNffpnm6cThrCH7wf6jN1ic3VHvLoY4OrzKtrZ4=', 'c5cIrRMn8XjB84M/D/X7Lg9uUqQFmYNEdxb/4HWH8OLa4pNZ', '测试账号', datetime(CURRENT_TIMESTAMP, 'localtime'), datetime(CURRENT_TIMESTAMP, 'localtime'), 'system', '系统默认创建', 'Demo');

DELETE From Dicts Where Define = 0;
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('应用程序', '后台管理', 'BA', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '网站标题', '后台管理系统', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '网站页脚', '2016 © 通用后台管理系统', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('系统通知', '用户注册', '0', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('系统通知', '程序异常', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('系统通知', '数据库连接', '2', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('通知状态', '未处理', '0', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('通知状态', '已处理', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('处理结果', '同意', '0', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('处理结果', '拒绝', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('消息状态', '未读', '0', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('消息状态', '已读', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('消息标签', '一般', '0', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('消息标签', '紧要', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('头像地址', '头像路径', '~/images/uploader/', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('头像地址', '头像文件', 'default.jpg', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站样式', '蓝色样式', 'blue.css', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站样式', '黑色样式', 'black.css', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站样式', 'AdminLTE', 'lte.css', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '使用样式', 'blue.css', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '前台首页', '~/Home/Index', 0);

-- 网站UI设置
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '侧边栏状态', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '卡片标题状态', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '固定表头', '1', 0);

-- 登录配置
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '短信验证码登录', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', 'OAuth 认证登录', '1', 0);

-- 自动锁屏（秒）默认 30 秒
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '自动锁屏时长', '30', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '自动锁屏', '0', 0);

-- 是否启用 Blazor 默认为 0 未启用
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', 'Blazor', '0', 0);

-- 是否启用 健康检查 默认为 0 未启用 1 启用
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '健康检查', '1', 0);

-- 时长单位 月
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '程序异常保留时长', '1', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '操作日志保留时长', '12', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '登录日志保留时长', '12', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '访问日志保留时长', '1', 0);

-- 时长单位 天
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', 'Cookie保留时长', '7', 0);

INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', 'IP地理位置接口', 'None', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('地理位置服务', '百度地图开放平台', 'BaiDuIPSvr', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('地理位置服务', '聚合地理位置', 'JuheIPSvr', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('地理位置服务', '百度138地理位置', 'BaiDuIP138Svr', 0);

INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('地理位置', 'BaiDuIPSvr', 'http://api.map.baidu.com/location/ip?ak=6lvVPMDlm2gjLpU0aiqPsHXi2OiwGQRj&ip=', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('地理位置', 'JuheIPSvr', 'http://apis.juhe.cn/ip/ipNew?key=f57102d1b9fadd3f4a1c29072d0c0206&ip=', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('地理位置', 'BaiDuIP138Svr', 'https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?resource_id=6006&query=', 0);

-- 时长单位 分钟
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', 'IP请求缓存时长', '10', 0);

-- 演示系统
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '演示系统', '0', 0);
-- 授权密码默认为 123789
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '授权盐值', 'yjglE2eddCGcS7tTFTDd2DfvqXHgCnMhNhpmx9HJaC9l8GAZ', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '哈希结果', '6jTT50HGuk8V+AIsiE4IfqjcER71PBN1DY7gqOLZE7E=', 0);

INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '验证码图床', 'http://imgs.sdgxgz.com/images/', 0);

INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '默认应用程序', '0', 0);

INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '后台地址', 'http://localhost:50852', 0);

-- 系统登录首页设置
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('系统首页', '高仿码云', '2-Login-Gitee', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('系统首页', '蓝色清新', '3-Login-Blue', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('系统首页', '系统默认', '1-Login', 1);
INSERT INTO Dicts (Category, Name, Code, Define) VALUES ('系统首页', '科技动感', '4-Login-Tec', 0);
INSERT INTO Dicts (Category, Name, Code, Define) VALUES ('系统首页', 'Admin-LTE', '5-Login-LTE', 0);

INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('网站设置', '登录界面', '1-Login', 0);
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('应用首页', 'BA', '{0}://{1}:5210', 0);

DELETE FROM Navigations Where Category = '0';
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '后台管理', 10, 'fa-solid fa-flag', '~/Admin/Index', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '个人中心', 20, 'fa-solid fa-suitcase', '~/Admin/Profiles', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '保存显示名称', 10, 'fa-solid fa-flag', 'saveDisplayName', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '保存密码', 20, 'fa-solid fa-flag', 'savePassword', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 2, '保存应用', 30, 'fa-solid fa-flag', 'saveApp', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 3, '保存样式', 40, 'fa-solid fa-flag', 'saveTheme', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 4, '保存头像', 50, 'fa-solid fa-flag', 'saveIcon', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 5, '保存网站设置', 60, 'fa-solid fa-flag', 'saveUISettings', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '系统锁屏', 25, 'fa-solid fa-television', '~/Account/Lock', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '返回前台', 30, 'fa-solid fa-house-user', '~/Home/Index', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '网站设置', 40, 'fa-solid fa-gear', '~/Admin/Settings', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '保存系统名称', 10, 'fa-solid fa-flag', 'saveTitle', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '保存页脚设置', 20, 'fa-solid fa-flag', 'saveFooter', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 2, '保存样式', 30, 'fa-solid fa-flag', 'saveTheme', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 3, '清理缓存', 40, 'fa-solid fa-flag', 'clearCache', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 4, '清理全部缓存', 50, 'fa-solid fa-flag', 'clearAllCache', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 5, '登录设置', 60, 'fa-solid fa-flag', 'loginSettings', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 6, '自动锁屏', 70, 'fa-solid fa-flag', 'lockScreen', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 7, '默认应用', 80, 'fa-solid fa-flag', 'defaultApp', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '菜单管理', 50, 'fa-solid fa-bars', '~/Admin/Menus', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '新增', 10, 'fa-solid fa-flag', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '编辑', 20, 'fa-solid fa-flag', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 2, '删除', 30, 'fa-solid fa-flag', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 3, '分配角色', 40, 'fa-solid fa-flag', 'assignRole', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (0, '图标页面', 50, 'fa-solid fa-flag', '~/Admin/IconView', '0', 1);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (0, '侧边栏', 55, 'fa-solid fa-flag', '~/Admin/Sidebar', '0', 1);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '用户管理', 60, 'fa-solid fa-user-gear', '~/Admin/Users', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '新增', 10, 'fa-solid fa-flag', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '编辑', 20, 'fa-solid fa-flag', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 2, '删除', 30, 'fa-solid fa-flag', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 3, '分配部门', 40, 'fa-solid fa-flag', 'assignGroup', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 4, '分配角色', 50, 'fa-solid fa-flag', 'assignRole', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '角色管理', 70, 'fa-solid fa-users-gear', '~/Admin/Roles', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '新增', 10, 'fa-solid fa-flag', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '编辑', 20, 'fa-solid fa-flag', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 2, '删除', 30, 'fa-solid fa-flag', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 3, '分配用户', 40, 'fa-solid fa-flag', 'assignUser', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 4, '分配部门', 50, 'fa-solid fa-flag', 'assignGroup', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 5, '分配菜单', 60, 'fa-solid fa-flag', 'assignMenu', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 6, '分配应用', 70, 'fa-solid fa-flag', 'assignApp', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '组织管理', 80, 'fa-solid fa-people-roof', '~/Admin/Groups', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '新增', 10, 'fa-solid fa-flag', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '编辑', 20, 'fa-solid fa-flag', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 2, '删除', 30, 'fa-solid fa-flag', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 3, '分配用户', 40, 'fa-solid fa-flag', 'assignUser', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 4, '分配角色', 50, 'fa-solid fa-flag', 'assignRole', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '字典表维护', 90, 'fa-solid fa-book', '~/Admin/Dicts', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '新增', 10, 'fa-solid fa-flag', 'add', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '编辑', 20, 'fa-solid fa-flag', 'edit', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 2, '删除', 30, 'fa-solid fa-flag', 'del', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '站内消息', 100, 'fa-solid fa-envelope', '~/Admin/Messages', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '任务管理', 110, 'fa-solid fa-tasks', '~/Admin/Tasks', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '暂停', 10, 'fa-solid fa-flag', 'pause', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid() - 1, '日志', 20, 'fa-solid fa-flag', 'info', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '通知管理', 120, 'fa-solid fa-bell', '~/Admin/Notifications', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '系统日志', 130, 'fa-solid fa-file-circle-check', '#', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid(), '操作日志', 10, 'fa-solid fa-edit', '~/Admin/Logs', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid() - 1, '登录日志', 20, 'fa-solid fa-person-circle-check', '~/Admin/Logins', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid() - 2, '访问日志', 30, 'fa-solid fa-bars', '~/Admin/Traces', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid() - 3, 'SQL日志', 40, 'fa-solid fa-database', '~/Admin/SQL', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '在线用户', 140, 'fa-solid fa-users', '~/Admin/Online', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '网站分析', 145, 'fa-solid fa-line-chart', '~/Admin/Analyse', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '程序异常', 150, 'fa-solid fa-cubes', '~/Admin/Exceptions', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], IsResource) VALUES (last_insert_rowid(), '服务器日志', 10, 'fa-solid fa-flag', 'log', '0', 2);
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '健康检查', 155, 'fa-solid fa-heartbeat', '~/Admin/Healths', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '工具集合', 160, 'fa-solid fa-gavel', '#', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid(), '客户端测试', 10, 'fa-solid fa-wrench', '~/Admin/Mobile', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid() - 1, 'API文档', 20, 'fa-solid fa-wrench', '~/swagger', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid() - 2, '图标集', 30, 'fa-solid fa-dashboard', '~/Admin/FAIcon', '0');

-- 控件集合菜单
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (0, '控件集合', 170, 'fa-solid fa-stethoscope', '#', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid(), '行为式验证码', 10, 'fa-solid fa-wrench', 'https://gitee.com/LongbowEnterprise/SliderCaptcha', '0');
INSERT INTO [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category]) VALUES (last_insert_rowid() - 1, '下拉框', 20, 'fa-solid fa-bars', 'http://longbowenterprise.gitee.io/longbow-select/', '0');

DELETE FROM GROUPS WHERE GroupName = 'Admin';
INSERT INTO [Groups] ([GroupCode], [GroupName], [Description]) VALUES ('001', 'Admin', '系统默认组');

DELETE FROM Roles where RoleName in ('Administrators', 'Default');
INSERT INTO [Roles] ([RoleName], [Description]) VALUES ('Administrators', '系统管理员');
INSERT INTO [Roles] ([RoleName], [Description]) VALUES ('Default', '默认用户，可访问前台页面');

DELETE FROM RoleGroup;
INSERT INTO RoleGroup (GroupId, RoleId) SELECT g.Id, r.Id From Groups g left join Roles r on 1=1 where GroupName = 'Admin' and RoleName = 'Administrators';

DELETE FROM UserGroup;

DELETE FROM UserRole;
INSERT INTO UserRole (UserId, RoleId) SELECT u.Id, r.Id From Users u left join Roles r on 1=1 where UserName = 'Admin' and RoleName = 'Administrators';
INSERT INTO UserRole (UserId, RoleId) SELECT u.Id, r.Id From Users u left join Roles r on 1=1 where UserName = 'User' and RoleName = 'Default';

DELETE FROM NavigationRole;
INSERT INTO NavigationRole (NavigationID, RoleID) SELECT n.Id, r.Id FROM Navigations n left join Roles r on 1=1 Where RoleName = 'Administrators';
INSERT INTO NavigationRole (NavigationID, RoleID) SELECT n.Id, r.Id FROM Navigations n left join Roles r on 1=1 Where RoleName = 'Default' and Name in ('后台管理', '个人中心', '返回前台', '通知管理');
INSERT INTO NavigationRole (NavigationID, RoleID) SELECT n.Id, r.Id FROM Navigations n left join Roles r on 1=1 Where RoleName = 'Default' and ParentId in (select id from Navigations where Name in ('个人中心'));

-- Client Data
Delete From [Dicts] Where Category = '应用程序' and Code = 'Demo';
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('应用程序', '测试平台', 'Demo', 0);
Delete From [Dicts] Where Category = '应用首页' and Name = 'Demo';
INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('应用首页', 'Demo', 'http://localhost:49185', 0);

Delete From [Dicts] Where Category = '测试平台';
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '网站标题', '前台演示系统', 1);
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '网站页脚', '前台演示程序后台权限管理框架', 1);
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '个人中心地址', '/Admin/Profiles', 1);
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '系统设置地址', '/Admin/Index', 1);
Insert into Dicts (Category, [Name], Code, Define) values ('测试平台', '系统通知地址', '/Admin/Notifications', 1);
INSERT INTO Dicts (Category, [Name], Code, Define) VALUES ('测试平台', 'favicon', '/favicon.ico', 1);
INSERT INTO Dicts (Category, [Name], Code, Define) VALUES ('测试平台', '网站图标', '/favicon.png', 1);

Delete from [Navigations] where Application = 'Demo';
INSERT into [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, '首页', 10, 'fa-solid fa-flag', '~/Home/Index', '1', 'Demo');

INSERT into [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, '测试页面', 20, 'fa-solid fa-flag', '#', '1', 'Demo');
INSERT into [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (last_insert_rowid(), '关于', 10, 'fa-solid fa-flag', '~/Home/About', '1', 'Demo');

INSERT into [Navigations] ([ParentId], [Name], [Order], [Icon], [Url], [Category], [Application]) VALUES (0, '返回码云', 20, 'fa-solid fa-flag', 'https://gitee.com/LongbowEnterprise/BootstrapAdmin', '1', 'Demo');

-- 菜单授权
INSERT INTO NavigationRole (NavigationId, RoleId) SELECT n.ID, r.ID FROM Navigations n left join Roles r on 1=1 Where r.RoleName = 'Default' and [Application] = 'Demo';

-- 角色对应用授权
DELETE From RoleApp where AppId in ('Demo', 'BA');
INSERT INTO RoleApp (AppId, RoleId) SELECT 'Demo', ID From Roles Where RoleName = 'Default';
INSERT INTO RoleApp (AppId, RoleId) SELECT 'BA', ID From Roles Where RoleName = 'Default';
