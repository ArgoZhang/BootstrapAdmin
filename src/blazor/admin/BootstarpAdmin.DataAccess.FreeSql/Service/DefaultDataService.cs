// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.FreeSql.Extensions;
using BootstrapBlazor.Components;
using BootstrapBlazor.DataAccess.FreeSql;

namespace BootstrapAdmin.DataAccess.FreeSql.Service;

class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
{
    private IFreeSql FreeSql { get; }

    public DefaultDataService(IFreeSql freeSql) => FreeSql = freeSql;

    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override async Task<bool> DeleteAsync(IEnumerable<TModel> models)
    {
        await FreeSql.Delete<TModel>(models).ExecuteAffrowsAsync();
        return true;
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
    {
        if (changedType == ItemChangedType.Add)
        {
            await FreeSql.Insert<TModel>(model).ExecuteAffrowsAsync();
        }
        else if (changedType == ItemChangedType.Update)
        {
            await FreeSql.Update<TModel>(model).ExecuteAffrowsAsync();
        }
        return true;
    }

    public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option) => Task.FromResult(new QueryData<TModel>
    {
        IsSorted = option.SortOrder != SortOrder.Unset,
        IsFiltered = option.Filters.Any(),
        IsAdvanceSearch = option.AdvanceSearches.Any(),
        IsSearch = option.Searches.Any() || option.CustomerSearches.Any(),
        Items = FreeSql.Select<TModel>()
                       .WhereDynamicFilter(option.ToDynamicFilter())
                       .OrderByPropertyNameIf(option.SortOrder != SortOrder.Unset, option.SortName, option.SortOrder == SortOrder.Asc)
                       .Count(out var count)
                       .PageIf(option.PageIndex, option.PageItems, option.IsPage)
                       .ToList(),
        TotalCount = Convert.ToInt32(count)
    });
}
