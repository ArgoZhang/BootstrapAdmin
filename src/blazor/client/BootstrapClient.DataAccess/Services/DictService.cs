// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapClient.DataAccess.Models;
using BootstrapClient.Web.Core;
using PetaPoco;

namespace BootstrapClient.DataAccess.PetaPoco.Services;

class DictService : IDict
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public DictService(IDBManager db) => DBManager = db;

    public List<Dict> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<Dict>();
    }

    public bool IsDemo()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "演示系统" && d.Define == EnumDictDefine.System)?.Code ?? "0";
        return code == "1";
    }

    public string GetWebTitle(string appId)
    {
        var dicts = GetAll();
        var title = "网站标题";
        var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站标题") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站标题");
            title = dict?.Code ?? "网站标题";
        }
        return title;
    }

    public string GetWebFooter(string appId)
    {
        var dicts = GetAll();
        var title = "网站页脚";
        var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站页脚") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站页脚");
            title = dict?.Code ?? "网站标题";
        }
        return title;
    }

    public string? GetProfileUrl(string appId) => GetUrlByName(appId, "个人中心地址");

    public string? GetSettingsUrl(string appId) => GetUrlByName(appId, "系统设置地址");

    public string? GetNotificationUrl(string appId) => GetUrlByName(appId, "系统通知地址");

    private string? GetUrlByName(string appId, string dictName)
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Category == appId && d.Name == dictName && d.Define == EnumDictDefine.Customer)?.Code;
    }

    public string GetAdminUrl()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Category == "应用首页" && d.Name == "BA" && d.Define == EnumDictDefine.System)?.Code ?? "{0}://{1}:5210";
    }
}
