// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapBlazor.Components;
using FreeSql;
using FreeSql.Internal.Model;

namespace BootStarpAdmin.DataAccess.FreeSql.Extensions;

/// <summary>
/// 
/// </summary>
static class FilterExtensions
{
    public static ISelect<TModel> PageIf<TModel>(this ISelect<TModel> source, int pageIndex, int pageItems, bool isPage) => isPage
        ? source.Page(pageIndex, pageItems)
        : source;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static DynamicFilterInfo ToDynamicFilter(this IEnumerable<IFilterAction> filters)
    {
        var ret = new DynamicFilterInfo() { Filters = new List<DynamicFilterInfo>() };

        // 处理 过滤 高级搜索 自定义搜索
        foreach (var filter in filters)
        {
            var item = new DynamicFilterInfo() { Filters = new List<DynamicFilterInfo>() };
            var actions = filter.GetFilterConditions();
            foreach (var f in actions)
            {
                item.Logic = f.FilterLogic.ToDynamicFilterLogic();
                item.Filters.Add(new DynamicFilterInfo()
                {
                    Field = f.FieldKey,
                    Value = f.FieldValue,
                    Operator = f.FilterAction.ToDynamicFilterOperator()
                });
            }
            ret.Filters.Add(item);
        }
        return ret;
    }

    private static DynamicFilterLogic ToDynamicFilterLogic(this FilterLogic logic) => logic switch
    {
        FilterLogic.And => DynamicFilterLogic.And,
        _ => DynamicFilterLogic.Or
    };

    private static DynamicFilterOperator ToDynamicFilterOperator(this FilterAction action) => action switch
    {
        FilterAction.Equal => DynamicFilterOperator.Equal,
        FilterAction.NotEqual => DynamicFilterOperator.NotEqual,
        FilterAction.Contains => DynamicFilterOperator.Contains,
        FilterAction.NotContains => DynamicFilterOperator.NotContains,
        FilterAction.GreaterThan => DynamicFilterOperator.GreaterThan,
        FilterAction.GreaterThanOrEqual => DynamicFilterOperator.GreaterThanOrEqual,
        FilterAction.LessThan => DynamicFilterOperator.LessThan,
        FilterAction.LessThanOrEqual => DynamicFilterOperator.LessThanOrEqual,
        _ => throw new System.NotSupportedException()
    };
}
