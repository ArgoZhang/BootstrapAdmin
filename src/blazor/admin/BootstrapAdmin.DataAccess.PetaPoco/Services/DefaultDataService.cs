// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapAdmin.Web.Extensions;
using BootstrapBlazor.Components;
using PetaPoco;
using PetaPoco.Extensions;

namespace BootstrapBlazor.DataAcces.PetaPoco.Services
{
    /// <summary>
    /// PetaPoco ORM 的 IDataService 接口实现
    /// </summary>
    class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
    {
        private IDatabase Database { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultDataService(IDatabase db) => Database = db;

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public override Task<bool> DeleteAsync(IEnumerable<TModel> models)
        {
            // 通过模型获取主键列数据
            // 支持批量删除
            Database.DeleteBatch(models);
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
                await Database.InsertAsync(model);
            }
            else
            {
                await Database.UpdateAsync(model);
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

            if (option.IsPage)
            {
                var items = await Database.PageAsync<TModel>(option);

                ret.TotalCount = Convert.ToInt32(items.TotalItems);
                ret.Items = items.Items;
            }
            else
            {
                var items = await Database.FetchAsync<TModel>(option);
                ret.TotalCount = items.Count;
                ret.Items = items;
            }
            return ret;
        }
    }
}
