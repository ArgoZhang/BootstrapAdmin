using Bootstrap.Security.Blazor;
using BootstrapAdmin.Web.Core.Services;
using BootstrapAdmin.Web.Services;

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
        [Parameter]
        public List<string>? SortList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public RenderFragment<TItem>? TableColumns { get; set; }

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
        [Parameter]
        public RenderFragment<TItem>? EditTemplate { get; set; }

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
        public bool IsMultipleSelect { get; set; } = true;

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
        public bool ShowToolbar { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowEmpty { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowLoading { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool ShowSearch { get; set; } = true;

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
        public ITableSearchModel? CustomerSearchModel { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, ItemChangedType, Task<bool>>? OnSaveAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<IEnumerable<TItem>, Task<bool>>? OnDeleteAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public List<TItem>? SelectedRows { get; set; } = new List<TItem>();

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, bool>? ShowEditButtonCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TItem, bool>? ShowDeleteButtonCallback { get; set; }

        [NotNull]
        private Table<TItem>? Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask ToggleLoading(bool v) => Instance.ToggleLoading(v);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task QueryAsync() => Instance.QueryAsync();

        [Inject]
        [NotNull]
        private IBootstrapAdminService? AdminService { get; set; }

        [Inject]
        [NotNull]
        private NavigationManager? NavigationManager { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? AppContext { get; set; }

        private bool AuthorizeButton(string operate)
        {
            var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            return AdminService.AuhorizingBlock(AppContext.UserName, url, operate);
        }
    }
}
