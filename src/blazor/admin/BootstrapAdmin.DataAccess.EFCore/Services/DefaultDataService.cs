// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

/// <summary>
/// EFCore ORM 的 IDataService 接口实现
/// </summary>
class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public DefaultDataService(IDbContextFactory<BootstrapAdminContext> factory) => DbFactory = factory;

    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override async Task<bool> DeleteAsync(IEnumerable<TModel> models)
    {
        // 通过模型获取主键列数据
        // 支持批量删除
        var context = DbFactory.CreateDbContext();
        context.RemoveRange(models);
        return await context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
    {
        var context = DbFactory.CreateDbContext();
        if (changedType == ItemChangedType.Add)
        {
            context.Entry(model).State = EntityState.Added;
        }
        else
        {
            context.Entry(model).State = EntityState.Modified;
        }
        return await context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// 查询方法
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
    {
        var context = DbFactory.CreateDbContext();
        var ret = new QueryData<TModel>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        var filter = option.ToFilter();
        if (option.IsPage)
        {
            var items = context.Set<TModel>()
                               .Where(filter.GetFilterLambda<TModel>(), filter.HasFilters())
                               .Sort(option.SortName!, option.SortOrder, !string.IsNullOrEmpty(option.SortName))
                               .Count(out var count)
                               .Page((option.PageIndex - 1) * option.PageItems, option.PageItems);

            ret.TotalCount = count;
            ret.Items = items;
        }
        else
        {
            var items = context.Set<TModel>()
                               .Where(filter.GetFilterLambda<TModel>(), filter.HasFilters())
                               .Sort(option.SortName!, option.SortOrder, !string.IsNullOrEmpty(option.SortName))
                               .Count(out var count);
            ret.TotalCount = count;
            ret.Items = items;
        }
        return Task.FromResult(ret);
    }
}
