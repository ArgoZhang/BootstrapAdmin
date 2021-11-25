using BootstrapBlazor.Components;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Admin.Blazor.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataserviceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTableDataService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDataService<>), typeof(TableDataService<>));
            return services;
        }

        internal class TableDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
        {
            [NotNull]
            public Func<QueryPageOptions, Task<(IEnumerable<TModel> Items, int Total)>>? QueryAsyncCallback { get; set; }

            /// <summary>
            /// 查询操作方法
            /// </summary>
            /// <param name="options"></param>
            /// <returns></returns>
            public override async Task<QueryData<TModel>> QueryAsync(QueryPageOptions options)
            {
                var items = await QueryAsyncCallback(options);

                return new QueryData<TModel>()
                {
                    Items = items.Items,
                    TotalCount = items.Total,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
        }
    }
}
