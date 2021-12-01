using BootstrapBlazor.Components;
using Bootstrap.DataAccess;
using Task = System.Threading.Tasks.Task;

namespace Bootstrap.Admin.Blazor.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Users
    {
        private Task<(IEnumerable<User>, int)> OnQueryAsync(QueryPageOptions options)
        {
            var items = UserHelper.Retrieves();
            var total = items.Count();

            // 处理高级搜索
            if (options.Searchs.Any())
            {
                items = items.Where(options.Searchs.GetFilterFunc<User>());
            }

            if (!string.IsNullOrEmpty(options.SortName))
            {
                items = items.Sort(options.SortName, options.SortOrder);
            }

            if (options.Filters.Any())
            {
                var aa = options.Filters.GetFilterFunc<User>();
                items = items.Where(options.Filters.GetFilterFunc<User>());
            }
            return Task.FromResult((items, total));
        }
    }
}
