// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services
{
    /// <summary>
    /// PetaPoco ORM 的 IDataService 接口实现
    /// </summary>
    class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
    {
        private IDbContextFactory<BootstrapAdminContext> DbFactory { get; set; }

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
        public override async Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
        {
            var context = DbFactory.CreateDbContext();
            var ret = new QueryData<TModel>()
            {
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

            var filters = option.Filters.Concat(option.Searchs).Concat(option.CustomerSearchs);
            if (option.IsPage)
            {
                var items = context.Set<TModel>().Where(filters.GetFilterLambda<TModel>()).Take(option.PageItems).Skip(option.PageItems * (option.PageIndex - 1));

                ret.TotalCount = await context.Set<TModel>().CountAsync();
                ret.Items = items;
            }
            else
            {
                var items = context.Set<TModel>().Where(option.Filters.GetFilterLambda<TModel>()).Where(option.CustomerSearchs.GetFilterLambda<TModel>());
                ret.TotalCount = await context.Set<TModel>().CountAsync();
                ret.Items = items;
            }
            return ret;
        }
    }
}
