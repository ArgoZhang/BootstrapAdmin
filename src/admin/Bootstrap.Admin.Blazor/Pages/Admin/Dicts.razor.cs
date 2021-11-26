using Bootstrap.Security;
using BootstrapBlazor.Components;

namespace Bootstrap.Admin.Blazor.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dicts
    {
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
    }
}
