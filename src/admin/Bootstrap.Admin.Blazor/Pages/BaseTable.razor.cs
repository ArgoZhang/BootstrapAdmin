using Bootstrap.Security;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Admin.Blazor.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BaseTable<TModel> where TModel : class, new()
    {

        private int[] PageSource { get; set; } = { 5, 20, 40 };

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        [Parameter]
        public Func<QueryPageOptions, Task<(IEnumerable<TModel> Items, int Total)>>? QueryAsyncCallback { get; set; }

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
        public RenderFragment<TModel>? ColumnsTemplete { get; set; }

        [NotNull]
        [Inject]
        private IDataService<TModel>? CustomerDataService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ((Extensions.DataserviceExtensions.TableDataService<TModel>)CustomerDataService).QueryAsyncCallback = QueryAsyncCallback;
        }
    }
}
