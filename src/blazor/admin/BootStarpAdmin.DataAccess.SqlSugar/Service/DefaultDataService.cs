// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class DefaultDataService<TModel>(ISqlSugarClient db) : DataServiceBase<TModel> where TModel : class, new()
{
    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override async Task<bool> DeleteAsync(IEnumerable<TModel> models)
    {
        await db.Deleteable<TModel>(models).ExecuteCommandAsync();
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
            await db.Insertable(model).ExecuteCommandAsync();
        }
        else if (changedType == ItemChangedType.Update)
        {
            await db.Updateable(model).ExecuteCommandAsync();
        }
        return true;
    }

    public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
    {
        int count = 0;
        var filter = option.ToFilter();
        var items = db.Queryable<TModel>()
                      .WhereIF(filter.HasFilters(), filter.GetFilterLambda<TModel>())
                      .OrderByIF(option.SortOrder != SortOrder.Unset, $"{option.SortName} {option.SortOrder}")
                      .ToPageList(option.PageIndex, option.PageItems, ref count);
        var data = new QueryData<TModel>
        {
            IsSorted = option.SortOrder != SortOrder.Unset,
            IsFiltered = option.Filters.Any(),
            IsAdvanceSearch = option.AdvanceSearches.Any(),
            IsSearch = option.Searches.Any() || option.CustomerSearches.Any(),
            Items = items,
            TotalCount = Convert.ToInt32(count)
        };
        return Task.FromResult(data);
    }
}
