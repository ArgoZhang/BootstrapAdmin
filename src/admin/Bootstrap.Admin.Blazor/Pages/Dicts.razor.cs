using Bootstrap.Security;
using BootstrapBlazor.Components;
using System.Linq;

namespace Bootstrap.Admin.Blazor.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dicts
    {
        private Task<(IEnumerable<BootstrapDict>, int)> QueryAsyncCallback(QueryPageOptions options)
        {
            var items = DataAccess.DictHelper.RetrieveDicts();
            if (options.Filters.Any())
            {
                items.Where(options.Filters.GetFilterFunc<BootstrapDict>());
            }
            var total = DataAccess.DictHelper.RetrieveDicts().Count();


            var pageItems = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems);

            return Task.FromResult((pageItems, total));
        }

        private Task OnCustomterColumnCreating(List<ITableColumn> columns)
        {
            foreach (var item in columns)
            {
                item.Searchable = true;
                item.Sortable = true;
                item.Filterable = true;
            }

            return Task.CompletedTask;
        }
    }
}
