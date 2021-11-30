using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Admin.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingTypeParameter(nameof(TItem))]
    public partial class BaseTable<TItem> where TItem : class, new()
    {

        private int[] PageSource { get; set; } = { 5, 20, 40 };

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public Func<QueryPageOptions, Task<(IEnumerable<TItem> Items, int Total)>>? OnQueryAsyncCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public Func<IEnumerable<TItem>, Task<bool>>? OnDeleteAsyncCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public Func<TItem, ItemChangedType, Task<bool>>? OnAddOrUpdateAsyncCallback { get; set; }

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
                tableService.OnQueryAsyncCallback = OnQueryAsyncCallback;
                tableService.OnDeleteAsyncCallback = OnDeleteAsyncCallback;
                tableService.OnAddOrUpdateAsyncCallback = OnAddOrUpdateAsyncCallback;
            }
        }
    }
}
