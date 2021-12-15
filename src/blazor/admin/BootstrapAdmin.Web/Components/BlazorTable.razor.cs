namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingTypeParameter(nameof(TItem))]
    public partial class BlazorTable<TItem> where TItem : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public IEnumerable<int>? PageItemsSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<TItem>? ColumnsTemplete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? RowButtonTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
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
        public bool IsPagination { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ITableSearchModel? TableSearchModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public IDataService<TItem>? DataService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<QueryPageOptions, Task<QueryData<TItem>>>? OnQueryAsync { get; set; }
    }
}
