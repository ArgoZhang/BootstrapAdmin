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
        public override Task<bool> DeleteAsync(IEnumerable<TModel> models)
        {
            // 通过模型获取主键列数据
            // 支持批量删除
            using var context = DbFactory.CreateDbContext();
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
            using var context = DbFactory.CreateDbContext();
            return true;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public override async Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
        {
            var ret = new QueryData<TModel>()
            {
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

            //var filters = option.Filters.Concat(option.Searchs).Concat(option.CustomerSearchs);
            //if (option.IsPage)
            //{
            //    var items = await _db.PageAsync<TModel>(option.PageIndex, option.PageItems, filters, option.SortName, option.SortOrder);

            //    ret.TotalCount = Convert.ToInt32(items.TotalItems);
            //    ret.Items = items.Items;
            //}
            //else
            //{
            //    var items = await _db.FetchAsync<TModel>(filters, option.SortName, option.SortOrder);
            //    ret.TotalCount = items.Count;
            //    ret.Items = items;
            //}
            return ret;
        }
    }
}
