// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Utils;

static class LookupHelper
{
    public static List<SelectedItem> GetTargets() => new()
    {
        new SelectedItem("_self", "本窗口"),
        new SelectedItem("_blank", "新窗口"),
        new SelectedItem("_parent", "父级窗口"),
        new SelectedItem("_top", "顶级窗口"),
    };

    public static List<SelectedItem> GetExceptionCategory() => new()
    {
        new SelectedItem("App", "应用程序"),
        new SelectedItem("DB", "数据库")
    };

    public static List<SelectedItem> GetCheckItems() => new()
    {
        new("db", "数据库"),
        new("environment", "环境变量"),
        new("dotnet-runtime", "运行时"),
        new("file", "文件系统"),
        new("gc", "回收器"),
        new("app-mem", "程序内存"),
        new("sys-mem", "系统内存"),
        new("Gitee", "Gitee")
    };
}
