﻿using BootStarpAdmin.DataAccess.FreeSql.Extensions;
using BootstrapBlazor.Components;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

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

    public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
    {
        var ret = new QueryData<TModel>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true,
            IsAdvanceSearch = option.AdvanceSearchs.Any() || option.CustomerSearchs.Any()
        };
        ret.Items = FreeSql.Select<TModel>()
                           .WhereDynamicFilter(option.Searchs.ToDynamicFilter())
                           .WhereDynamicFilter(option.Filters
                                .Concat(option.AdvanceSearchs)
                                .Concat(option.CustomerSearchs)
                                .ToDynamicFilter())
                           .OrderByPropertyNameIf(option.SortOrder != SortOrder.Unset, option.SortName, option.SortOrder == SortOrder.Asc)
                           .Count(out var count)
                           .PageIf(option.PageIndex, option.PageItems, option.IsPage)
                           .ToList();
        ret.TotalCount = Convert.ToInt32(count);
        return Task.FromResult(ret);
    }
}
