using BootstrapBlazor.Components;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    internal class BlazorTableDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
    {
        [NotNull]
        public Func<QueryPageOptions, Task<(IEnumerable<TModel> Items, int Total)>>? OnQueryAsync { get; set; }

        [NotNull]
        public Func<IEnumerable<TModel>, Task<bool>>? OnDeleteAsync { get; set; }

        [NotNull]
        public Func<TModel, ItemChangedType, Task<bool>>? OnAddOrUpdateAsync { get; set; }

        /// <summary>
        /// 查询操作方法
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public override async Task<QueryData<TModel>> QueryAsync(QueryPageOptions options)
        {
            var (Items, Total) = await OnQueryAsync(options);

            return new QueryData<TModel>()
            {
                Items = Items,
                TotalCount = Total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };
        }

        public override Task<bool> DeleteAsync(IEnumerable<TModel> models) => OnDeleteAsync(models);

        public override Task<bool> SaveAsync(TModel model, ItemChangedType changedType) => OnAddOrUpdateAsync(model, changedType);
    }
}
