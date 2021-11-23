using BootstrapBlazor.Components;

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
            services.AddSingleton(typeof(IDataService<>), typeof(TableDataService<>));
            return services;
        }

        internal class TableDataService<BootstrapDict> : DataServiceBase<Bootstrap.Security.BootstrapDict>
        {
            /// <summary>
            /// 查询操作方法
            /// </summary>
            /// <param name="options"></param>
            /// <returns></returns>
            public override Task<QueryData<Bootstrap.Security.BootstrapDict>> QueryAsync(QueryPageOptions options)
            {

                var items = DataAccess.DictHelper.RetrieveDicts();

                var total = items.Count();

                // 内存分页
                items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

                return Task.FromResult(new QueryData<Bootstrap.Security.BootstrapDict>()
                {
                    Items = items,
                    TotalCount = total,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                });
            }
        }
    }
}
