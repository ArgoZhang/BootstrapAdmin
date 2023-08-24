// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 访问日志自定义高级搜索模型
/// </summary>
public class TraceSearchModel : ITableSearchModel
{
    /// <summary>
    /// 获得/设置 登录用户
    /// </summary>
    [Display(Name = "登录用户")]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 操作时间
    /// </summary>
    [Display(Name = "操作时间")]
    [NotNull]
    public DateTimeRangeValue? LogTime { get; set; }

    /// <summary>
    /// 获得/设置 登录主机
    /// </summary>
    [Display(Name = "登录主机")]
    public string? Ip { get; set; }

    /// <summary>
    /// 获得/设置 请求网址
    /// </summary>
    [Display(Name = "请求网址")]
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TraceSearchModel()
    {
        Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IFilterAction> GetSearches()
    {
        var ret = new List<IFilterAction>();

        if (!string.IsNullOrEmpty(UserName))
        {
            ret.Add(new SearchFilterAction(nameof(Trace.UserName), UserName));
        }

        if (!string.IsNullOrEmpty(RequestUrl))
        {
            ret.Add(new SearchFilterAction(nameof(Trace.RequestUrl), RequestUrl));
        }

        if (LogTime != null)
        {
            ret.Add(new SearchFilterAction(nameof(Trace.LogTime), LogTime.Start, FilterAction.GreaterThanOrEqual));
            ret.Add(new SearchFilterAction(nameof(Trace.LogTime), LogTime.End, FilterAction.LessThanOrEqual));
        }

        if (!string.IsNullOrEmpty(Ip))
        {
            ret.Add(new SearchFilterAction(nameof(Trace.Ip), Ip));
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Reset()
    {
        UserName = null;
        RequestUrl = null;
        Ip = null;
        LogTime = new DateTimeRangeValue
        {
            Start = DateTime.Now.AddDays(-7),
            End = DateTime.Now
        };
    }
}
