// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 字典维护自定义高级搜索模型
    /// </summary>
    public class ErrorSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 获得/设置 字典标签
        /// </summary>
        [Display(Name = "异常类型")]
        public string? Category { get; set; }

        /// <summary>
        /// 获得/设置 字典名称
        /// </summary>
        [Display(Name = "用户名")]
        public string? UserId { get; set; }

        /// <summary>
        /// 获得/设置 字典代码
        /// </summary>
        [Display(Name = "请求网址")]
        public string? ErrorPage { get; set; }

        /// <summary>
        /// 获得/设置 字典类型
        /// </summary>
        [Display(Name = "记录时间")]
        [NotNull]
        public DateTimeRangeValue? LogTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ErrorSearchModel()
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

            if (!string.IsNullOrEmpty(Category))
            {
                ret.Add(new SearchFilterAction(nameof(Error.Category), Category));
            }

            if (!string.IsNullOrEmpty(ErrorPage))
            {
                ret.Add(new SearchFilterAction(nameof(Error.ErrorPage), ErrorPage));
            }

            if (LogTime != null)
            {
                ret.Add(new SearchFilterAction(nameof(Error.LogTime), LogTime.Start, FilterAction.GreaterThanOrEqual));
                ret.Add(new SearchFilterAction(nameof(Error.LogTime), LogTime.End, FilterAction.LessThanOrEqual));
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                ret.Add(new SearchFilterAction(nameof(Error.UserId), UserId));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            Category = null;
            UserId = null;
            ErrorPage = null;
            LogTime = new DateTimeRangeValue
            {
                Start = DateTime.Now.AddDays(-7),
                End = DateTime.Now
            };
        }
    }
}
