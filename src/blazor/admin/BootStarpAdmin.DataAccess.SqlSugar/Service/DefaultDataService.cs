// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapBlazor.Components;
using SqlSugar;

namespace BootStarpAdmin.DataAccess.SqlSugar.Service;

class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
{
    private ISqlSugarClient Client { get; }

    public DefaultDataService(ISqlSugarClient client) => Client = client;

    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override async Task<bool> DeleteAsync(IEnumerable<TModel> models)
    {
        await Client.Deleteable<TModel>(models).ExecuteCommandAsync();
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
            await Client.Insertable<TModel>(model).ExecuteCommandAsync();
        }
        else if (changedType == ItemChangedType.Update)
        {
            await Client.Updateable<TModel>(model).ExecuteCommandAsync();
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
        if (option.IsPage)
        {
            var count = 0;
            ret.Items = Client.Queryable<TModel>()
                              .ToPageList(option.PageIndex, option.PageItems, ref count);
            ret.TotalCount = count;
        }
        else
        {
            ret.Items = Client.Queryable<TModel>()
                              .ToList();
        }
        return Task.FromResult(ret);
    }
}
