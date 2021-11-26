using Bootstrap.Security;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Blazor.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dicts
    {
        private IEnumerable<SelectedItem>? Categories { get; set; }

        private IEnumerable<SelectedItem>? EditCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Categories = new List<SelectedItem>()
            {
                new SelectedItem("","全部"){ Active = true},
                new SelectedItem("0","系统使用"),
                new SelectedItem("1","自定义"),
            };

            EditCategories = new List<SelectedItem>()
            {
                new SelectedItem("0","系统使用"),
                new SelectedItem("1","自定义"),
            };
        }

        private Task<(IEnumerable<BootstrapDict>, int)> QueryAsyncCallback(QueryPageOptions options)
        {
            var items = DataAccess.DictHelper.RetrieveDicts();
            var total = items.Count();

            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<BootstrapDict>());
            }
            return Task.FromResult((items, total));
        }

        private string CreateComponent(int code) => code switch
        {
            0 => "<div>系统使用</div>",
            1 => "<div>自定义</div>",
            _ => ""
        };
    }
}
