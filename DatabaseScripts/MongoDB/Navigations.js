﻿var Navigations = [
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b90"),
        "ParentId": "0",
        "Name": "后台管理",
        "Order": NumberInt(10),
        "Icon": "fa fa-gear",
        "Url": "~/Admin/Index",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b91"),
        "ParentId": "0",
        "Name": "个人中心",
        "Order": NumberInt(20),
        "Icon": "fa fa-suitcase",
        "Url": "~/Admin/Profiles",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4a01"),
        "ParentId": "5bd7b8445fa31256f77e4b91",
        "Name": "保存显示名称",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "saveDisplayName",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4a02"),
        "ParentId": "5bd7b8445fa31256f77e4b91",
        "Name": "保存密码",
        "Order": NumberInt(20),
        "Icon": "fa fa-fa",
        "Url": "savePassword",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4a03"),
        "ParentId": "5bd7b8445fa31256f77e4b91",
        "Name": "保存应用",
        "Order": NumberInt(30),
        "Icon": "fa fa-fa",
        "Url": "saveApp",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4a04"),
        "ParentId": "5bd7b8445fa31256f77e4b91",
        "Name": "保存样式",
        "Order": NumberInt(40),
        "Icon": "fa fa-fa",
        "Url": "saveTheme",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4a05"),
        "ParentId": "5bd7b8445fa31256f77e4b91",
        "Name": "保存头像",
        "Order": NumberInt(50),
        "Icon": "fa fa-fa",
        "Url": "saveIcon",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4a06"),
        "ParentId": "5bd7b8445fa31256f77e4b91",
        "Name": "保存网站设置",
        "Order": NumberInt(60),
        "Icon": "fa fa-fa",
        "Url": "saveUISettings",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b92"),
        "ParentId": "0",
        "Name": "返回前台",
        "Order": NumberInt(30),
        "Icon": "fa fa-hand-o-left",
        "Url": "~/Home/Index",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b93"),
        "ParentId": "0",
        "Name": "网站设置",
        "Order": NumberInt(40),
        "Icon": "fa fa-fa",
        "Url": "~/Admin/Settings",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b01"),
        "ParentId": "5bd7b8445fa31256f77e4b93",
        "Name": "保存系统名称",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "saveTitle",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b02"),
        "ParentId": "5bd7b8445fa31256f77e4b93",
        "Name": "保存页脚设置",
        "Order": NumberInt(20),
        "Icon": "fa fa-fa",
        "Url": "saveFooter",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b03"),
        "ParentId": "5bd7b8445fa31256f77e4b93",
        "Name": "保存样式",
        "Order": NumberInt(30),
        "Icon": "fa fa-fa",
        "Url": "saveTheme",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b04"),
        "ParentId": "5bd7b8445fa31256f77e4b93",
        "Name": "清理缓存",
        "Order": NumberInt(40),
        "Icon": "fa fa-fa",
        "Url": "clearCache",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b05"),
        "ParentId": "5bd7b8445fa31256f77e4b93",
        "Name": "清理全部缓存",
        "Order": NumberInt(50),
        "Icon": "fa fa-fa",
        "Url": "clearAllCache",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b94"),
        "ParentId": "0",
        "Name": "菜单管理",
        "Order": NumberInt(50),
        "Icon": "fa fa-dashboard",
        "Url": "~/Admin/Menus",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b10"),
        "ParentId": "5bd7b8445fa31256f77e4b94",
        "Name": "新增",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "add",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b11"),
        "ParentId": "5bd7b8445fa31256f77e4b94",
        "Name": "编辑",
        "Order": NumberInt(20),
        "Icon": "fa fa-fa",
        "Url": "edit",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b12"),
        "ParentId": "5bd7b8445fa31256f77e4b94",
        "Name": "删除",
        "Order": NumberInt(30),
        "Icon": "fa fa-fa",
        "Url": "del",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b13"),
        "ParentId": "5bd7b8445fa31256f77e4b94",
        "Name": "分配角色",
        "Order": NumberInt(40),
        "Icon": "fa fa-fa",
        "Url": "assignRole",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b14"),
        "ParentId": "0",
        "Name": "图标页面",
        "Order": NumberInt(55),
        "Icon": "fa fa-fa",
        "Url": "~/Admin/IconView",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(1),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b95"),
        "ParentId": "0",
        "Name": "用户管理",
        "Order": NumberInt(60),
        "Icon": "fa fa-user",
        "Url": "~/Admin/Users",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b20"),
        "ParentId": "5bd7b8445fa31256f77e4b95",
        "Name": "新增",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "add",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b21"),
        "ParentId": "5bd7b8445fa31256f77e4b95",
        "Name": "编辑",
        "Order": NumberInt(20),
        "Icon": "fa fa-fa",
        "Url": "edit",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b22"),
        "ParentId": "5bd7b8445fa31256f77e4b95",
        "Name": "删除",
        "Order": NumberInt(30),
        "Icon": "fa fa-fa",
        "Url": "del",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b23"),
        "ParentId": "5bd7b8445fa31256f77e4b95",
        "Name": "分配部门",
        "Order": NumberInt(40),
        "Icon": "fa fa-fa",
        "Url": "assignGroup",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b24"),
        "ParentId": "5bd7b8445fa31256f77e4b95",
        "Name": "分配角色",
        "Order": NumberInt(50),
        "Icon": "fa fa-fa",
        "Url": "assignRole",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b96"),
        "ParentId": "0",
        "Name": "角色管理",
        "Order": NumberInt(70),
        "Icon": "fa fa-sitemap",
        "Url": "~/Admin/Roles",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b30"),
        "ParentId": "5bd7b8445fa31256f77e4b96",
        "Name": "新增",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "add",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b31"),
        "ParentId": "5bd7b8445fa31256f77e4b96",
        "Name": "编辑",
        "Order": NumberInt(20),
        "Icon": "fa fa-fa",
        "Url": "edit",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b32"),
        "ParentId": "5bd7b8445fa31256f77e4b96",
        "Name": "删除",
        "Order": NumberInt(30),
        "Icon": "fa fa-fa",
        "Url": "del",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b33"),
        "ParentId": "5bd7b8445fa31256f77e4b96",
        "Name": "分配用户",
        "Order": NumberInt(40),
        "Icon": "fa fa-fa",
        "Url": "assignUser",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b34"),
        "ParentId": "5bd7b8445fa31256f77e4b96",
        "Name": "分配部门",
        "Order": NumberInt(50),
        "Icon": "fa fa-fa",
        "Url": "assignGroup",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b35"),
        "ParentId": "5bd7b8445fa31256f77e4b96",
        "Name": "分配菜单",
        "Order": NumberInt(60),
        "Icon": "fa fa-fa",
        "Url": "assignMenu",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b36"),
        "ParentId": "5bd7b8445fa31256f77e4b96",
        "Name": "分配应用",
        "Order": NumberInt(70),
        "Icon": "fa fa-fa",
        "Url": "assignApp",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b97"),
        "ParentId": "0",
        "Name": "部门管理",
        "Order": NumberInt(80),
        "Icon": "fa fa-bank",
        "Url": "~/Admin/Groups",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b40"),
        "ParentId": "5bd7b8445fa31256f77e4b97",
        "Name": "新增",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "add",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b41"),
        "ParentId": "5bd7b8445fa31256f77e4b97",
        "Name": "编辑",
        "Order": NumberInt(20),
        "Icon": "fa fa-fa",
        "Url": "edit",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b42"),
        "ParentId": "5bd7b8445fa31256f77e4b97",
        "Name": "删除",
        "Order": NumberInt(30),
        "Icon": "fa fa-fa",
        "Url": "del",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b43"),
        "ParentId": "5bd7b8445fa31256f77e4b97",
        "Name": "分配用户",
        "Order": NumberInt(40),
        "Icon": "fa fa-fa",
        "Url": "assignUser",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b44"),
        "ParentId": "5bd7b8445fa31256f77e4b97",
        "Name": "分配角色",
        "Order": NumberInt(50),
        "Icon": "fa fa-fa",
        "Url": "assignRole",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b98"),
        "ParentId": "0",
        "Name": "字典表维护",
        "Order": NumberInt(90),
        "Icon": "fa fa-book",
        "Url": "~/Admin/Dicts",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b50"),
        "ParentId": "5bd7b8445fa31256f77e4b98",
        "Name": "新增",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "add",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b51"),
        "ParentId": "5bd7b8445fa31256f77e4b98",
        "Name": "编辑",
        "Order": NumberInt(20),
        "Icon": "fa fa-fa",
        "Url": "edit",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b52"),
        "ParentId": "5bd7b8445fa31256f77e4b98",
        "Name": "删除",
        "Order": NumberInt(30),
        "Icon": "fa fa-fa",
        "Url": "del",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b99"),
        "ParentId": "0",
        "Name": "站内消息",
        "Order": NumberInt(100),
        "Icon": "fa fa-envelope",
        "Url": "~/Admin/Messages",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b9a"),
        "ParentId": "0",
        "Name": "任务管理",
        "Order": NumberInt(110),
        "Icon": "fa fa fa-tasks",
        "Url": "~/Admin/Tasks",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b9b"),
        "ParentId": "0",
        "Name": "通知管理",
        "Order": NumberInt(120),
        "Icon": "fa fa-bell",
        "Url": "~/Admin/Notifications",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b9c"),
        "ParentId": "0",
        "Name": "日志管理",
        "Order": NumberInt(130),
        "Icon": "fa fa-gears",
        "Url": "#",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd9b3d868aa001661776f57"),
        "ParentId": "5bd7b8445fa31256f77e4b9c",
        "Name": "操作日志",
        "Order": NumberInt(10),
        "Icon": "fa fa-edit",
        "Url": "~/Admin/Logs",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd9b3d868aa001661776f58"),
        "ParentId": "5bd7b8445fa31256f77e4b9c",
        "Name": "登录日志",
        "Order": NumberInt(20),
        "Icon": "fa fa-user-circle-o",
        "Url": "~/Admin/Logins",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd9b3d868aa001661776f59"),
        "ParentId": "5bd7b8445fa31256f77e4b9c",
        "Name": "访问日志",
        "Order": NumberInt(30),
        "Icon": "fa fa-bars",
        "Url": "~/Admin/Traces",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b89"),
        "ParentId": "0",
        "Name": "在线用户",
        "Order": NumberInt(140),
        "Icon": "fa fa-users",
        "Url": "~/Admin/Online",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b90"),
        "ParentId": "0",
        "Name": "网站分析",
        "Order": NumberInt(145),
        "Icon": "fa fa-line-chart",
        "Url": "~/Admin/Analyse",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b9d"),
        "ParentId": "0",
        "Name": "程序异常",
        "Order": NumberInt(150),
        "Icon": "fa fa-cubes",
        "Url": "~/Admin/Exceptions",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b59"),
        "ParentId": "0",
        "Name": "健康检查",
        "Order": NumberInt(155),
        "Icon": "fa fa-heartbeat",
        "Url": "~/Admin/Healths",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b60"),
        "ParentId": "5bd7b8445fa31256f77e4b9d",
        "Name": "服务器日志",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "log",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(2),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b9e"),
        "ParentId": "0",
        "Name": "工具集合",
        "Order": NumberInt(160),
        "Icon": "fa fa-gavel",
        "Url": "#",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4b9f"),
        "ParentId": "5bd7b8445fa31256f77e4b9e",
        "Name": "客户端测试",
        "Order": NumberInt(10),
        "Icon": "fa fa-wrench",
        "Url": "~/Admin/Mobile",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4ba0"),
        "ParentId": "5bd7b8445fa31256f77e4b9e",
        "Name": "API文档",
        "Order": NumberInt(10),
        "Icon": "fa fa-wrench",
        "Url": "~/swagger",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4ba1"),
        "ParentId": "5bd7b8445fa31256f77e4b9e",
        "Name": "图标集",
        "Order": NumberInt(10),
        "Icon": "fa fa-dashboard",
        "Url": "~/Admin/FAIcon",
        "Category": "0",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "0"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4ba2"),
        "ParentId": "0",
        "Name": "首页",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "~/Home/Index",
        "Category": "1",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "2"
    },
    {
        "_id": ObjectId("5bd7b8445fa31256f77e4ba4"),
        "ParentId": "5bd9b3d868aa001661776f56",
        "Name": "关于",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "~/Home/About",
        "Category": "1",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "2"
    },
    {
        "_id": ObjectId("5bd9b3d868aa001661776f56"),
        "ParentId": "0",
        "Name": "帮助",
        "Order": NumberInt(10),
        "Icon": "fa fa-fa",
        "Url": "#",
        "Category": "1",
        "Target": "_self",
        "IsResource": NumberInt(0),
        "Application": "2"
    }
];