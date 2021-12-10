// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using PetaPoco;
using PetaPoco.Extensions;
using PetaPoco.Providers;

namespace BootstrapBlazor.DataAcces.PetaPoco
{
    /// <summary>
    /// PetaPoco ORM 的 IDataService 接口实现
    /// </summary>
    internal class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
    {
        private readonly IDatabase _db;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultDataService(IConfiguration configuration)
        {
            //TODO: 后期改造成自定适配 
            var connString = configuration.GetConnectionString("bb");
            _db = new Database<SQLiteDatabaseProvider>(connString);
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public override Task<bool> DeleteAsync(IEnumerable<TModel> models)
        {
            // 通过模型获取主键列数据
            // 支持批量删除
            _db.DeleteBatch(models);
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
            if (changedType == ItemChangedType.Add)
            {
                await _db.InsertAsync(model);
            }
            else
            {
                await _db.UpdateAsync(model);
            }
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

            var filters = option.Filters.Concat(option.Searchs).Concat(option.CustomerSearchs);
            if (option.IsPage)
            {
                var items = await _db.PageAsync<TModel>(option.PageIndex, option.PageItems, filters, option.SortName, option.SortOrder);

                ret.TotalCount = Convert.ToInt32(items.TotalItems);
                ret.Items = items.Items;
            }
            else
            {
                var items = await _db.FetchAsync<TModel>(filters, option.SortName, option.SortOrder);
                ret.TotalCount = items.Count;
                ret.Items = items;
            }
            return ret;
        }
    }
}
