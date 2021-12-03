using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Admin.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingTypeParameter(nameof(TItem))]
    public partial class BlazorTable<TItem> where TItem : class, new()
    {

        private int[] PageSource { get; set; } = { 5, 20, 40 };

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public Func<Task<IEnumerable<TItem>>>? OnQueryAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public Func<IEnumerable<TItem>, Task<bool>>? OnDeleteAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public Func<TItem, Task<bool>>? OnAddOrUpdateAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public Func<List<ITableColumn>, Task>? OnCustomterColumnCreating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<TItem>? ColumnsTemplete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<ITableSearchModel>? CustomerSearchTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment? TableToolbarTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsPagination { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ITableSearchModel? TableSearchModel { get; set; }

        [NotNull]
        [Inject]
        private IDataService<TItem>? DataService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (DataService is BlazorTableDataService<TItem> tableService)
            {
                tableService.OnQueryAsync = OnQueryBaseAsync;
                tableService.OnDeleteAsync = OnDeleteBaseAsync;
                tableService.OnAddOrUpdateAsync = OnAddOrUpdateBaseAsync;
            }
        }

        private async Task<QueryData<TItem>> OnQueryBaseAsync(QueryPageOptions options)
        {
            var items = await OnQueryAsync();
            var total = items.Count();

            // 处理高级搜索
            if (options.Searchs.Any())
            {
                items = items.Where(options.Searchs.GetFilterFunc<TItem>());
            }
            else if (options.CustomerSearchs.Any())
            {
                items = items.Where(options.CustomerSearchs.GetFilterFunc<TItem>());
            }

            if (!string.IsNullOrEmpty(options.SortName))
            {
                items = items.Sort(options.SortName, options.SortOrder);
            }

            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<TItem>());
            }
            return new QueryData<TItem>()
            {
                Items = items,
                TotalCount = total,
                IsFiltered = options.Filters.Any(),
                IsSearch = options.CustomerSearchs.Any(),
                IsSorted = !string.IsNullOrEmpty(options.SortName),
            };
        }

        private async Task<bool> OnDeleteBaseAsync(IEnumerable<TItem> items)
        {
            return await OnDeleteAsync(items);
        }

        private async Task<bool> OnAddOrUpdateBaseAsync(TItem item, ItemChangedType changedType)
        {
            return await OnAddOrUpdateAsync(item);
        }
    }
}
