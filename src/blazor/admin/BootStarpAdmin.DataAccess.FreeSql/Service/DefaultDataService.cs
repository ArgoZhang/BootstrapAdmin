using BootstrapBlazor.Components;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
{
    private IFreeSql FreeSql;

    public DefaultDataService(IFreeSql freeSql) => FreeSql = freeSql;

    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override Task<bool> DeleteAsync(IEnumerable<TModel> models)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
    {
        return await FreeSql.InsertOrUpdate<TModel>().ExecuteAffrowsAsync() > 0;
    }

    public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
    {
        var ret = new QueryData<TModel>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        var filters = option.Filters.Concat(option.Searchs).Concat(option.CustomerSearchs);
        if (option.IsPage)
        {
            var items = FreeSql.Select<TModel>()
                               .WhereIf(filters.Any(), filters.GetFilterLambda<TModel>())
                               .Count(out var count)
                               .Page((option.PageIndex - 1) * option.PageItems, option.PageItems)
                               .ToList();

            ret.TotalCount = Convert.ToInt32(count);
            ret.Items = items;
        }
        else
        {
            var items = FreeSql.Select<TModel>()
                               .WhereIf(filters.Any(), filters.GetFilterLambda<TModel>())
                               .Count(out var count)
                               .ToList();
            ret.TotalCount = Convert.ToInt32(count);
            ret.Items = items;
        }
        return Task.FromResult(ret);
    }
}
