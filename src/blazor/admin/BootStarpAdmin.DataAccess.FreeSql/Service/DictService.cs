﻿using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class DictService : IDict
{
    private IFreeSql FreeSql { get; }

    private string? AppId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    public DictService(IFreeSql freeSql, IConfiguration configuration)
    {
        FreeSql = freeSql;
        AppId = configuration.GetValue("AppId", "BA");
    }

    public bool AuthenticateDemo(string code)
    {
        var ret = false;
        if (!string.IsNullOrEmpty(code))
        {
            var dicts = GetAll();
            var salt = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "授权盐值" && d.Define == EnumDictDefine.System)?.Code;
            var authCode = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "哈希结果" && d.Define == EnumDictDefine.System)?.Code;
            if (!string.IsNullOrEmpty(salt))
            {
                ret = LgbCryptography.ComputeHash(code, salt) == authCode;
            }
        }
        return ret;
    }

    public List<Dict> GetAll()
    {
        return FreeSql.Select<Dict>().ToList();
    }

    public Dictionary<string, string> GetApps()
    {
        var dicts = GetAll();

        return dicts.Where(d => d.Category == "应用程序").Select(s => new KeyValuePair<string, string>(s.Code, s.Name)).ToDictionary(i => i.Key, i => i.Value);
    }

    public int GetCookieExpiresPeriod()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "Cookie保留时长" && d.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(code, out var ret);
        return ret;
    }

    public string GetCurrentLogin()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "登录界面" && d.Define == EnumDictDefine.System)?.Code ?? "Login";
    }

    public Dictionary<string, string> GetLogins()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == "系统首页").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
    }

    public string? GetNotificationUrl(string appId) => GetUrlByName(appId, "系统通知地址");

    public string? GetProfileUrl(string appId) => GetUrlByName(appId, "个人中心地址");

    public string? GetSettingsUrl(string appId) => GetUrlByName(appId, "系统设置地址");

    public Dictionary<string, string> GetThemes()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == "网站样式").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
    }

    public string GetWebFooter()
    {
        var dicts = GetAll();
        var title = "网站页脚";
        var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站页脚") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站页脚");
            title = dict?.Code ?? "网站标题";
        }
        return title;
    }

    public string GetWebTitle()
    {
        var dicts = GetAll();
        var title = "网站标题";
        var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站标题") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站标题");
            title = dict?.Code ?? "网站标题";
        }
        return title;
    }

    public bool IsDemo()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "演示系统" && d.Define == EnumDictDefine.System)?.Code ?? "0";
        return code == "1";
    }

    public string RetrieveIconFolderPath()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == EnumDictDefine.System)?.Code ?? "images/uploder/";
    }

    private bool SaveDict(Dict dict) => FreeSql.Update<Dict>().Where(s => s.Category == dict.Category && s.Name == dict.Code).Set(s => s.Code, dict.Code).ExecuteAffrows() > 0;

    public bool SaveCookieExpiresPeriod(int expiresPeriod) => SaveDict(new Dict { Category = "网站设置", Name = "Cookie保留时长", Code = expiresPeriod.ToString() });

    public bool SaveDemo(bool isDemo)
    {
        return FreeSql.Update<Dict>()
                .Where(s => s.Category == "网站设置" && s.Name == "演示系统" && s.Define == EnumDictDefine.System)
                .Set(s => s.Code, isDemo ? "1" : "0").ExecuteAffrows() > 0;
    }

    public bool SaveHealthCheck(bool enable = true)
    {
        return FreeSql.Update<Dict>()
                .Where(s => s.Category == "网站设置" && s.Name == "健康检查" && s.Define == EnumDictDefine.System)
                .Set(s => s.Code, enable ? "1" : "0").ExecuteAffrows() > 0;
    }

    public bool SaveLogin(string login) => SaveDict(new Dict { Category = "网站设置", Name = "登录界面", Code = login });

    public bool SaveTheme(string theme) => SaveDict(new Dict { Category = "网站设置", Name = "使用样式", Code = theme });

    public bool SaveWebFooter(string footer) => SaveDict(new Dict { Category = "网站设置", Name = "网站页脚", Code = footer });

    public bool SaveWebTitle(string title) => SaveDict(new Dict { Category = "网站设置", Name = "网站标题", Code = title });

    private string? GetUrlByName(string appId, string dictName)
    {
        string? url = null;
        var dicts = GetAll();
        var appName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId && d.Define == EnumDictDefine.System)?.Name;
        if (!string.IsNullOrEmpty(appName))
        {
            url = dicts.FirstOrDefault(d => d.Category == appName && d.Name == dictName && d.Define == EnumDictDefine.Customer)?.Code;
        }
        return url;
    }
}