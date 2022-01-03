namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingTypeParameter(nameof(TItem))]
    public partial class AdminTable<TItem> where TItem : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public IEnumerable<int>? PageItemsSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int ExtendButtonColumnWidth { get; set; } = 130;

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
        public bool IsPagination { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsFixedHeader { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsTree { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowAdvancedSearch { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowDefaultButtons { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowExtendButtons { get; set; } = true;

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

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, Task<IEnumerable<TItem>>>? OnTreeExpand { get; set; }
    }
}
