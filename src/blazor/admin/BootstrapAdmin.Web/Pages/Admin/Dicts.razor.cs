using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dicts
    {
        private ITableSearchModel DictsSearchModel { get; set; } = new DictsSearchModel();

        [Inject]
        [NotNull]
        private IDict? DictService { get; set; }

        private Task<QueryData<Dict>> OnQueryAsync(QueryPageOptions options)
        {
            var ret = new QueryData<Dict>()
            {
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

            var items = DictService.GetAll();

            // 处理模糊查询
            if (!string.IsNullOrEmpty(options.SearchText))
            {
                items = items.Where(i => i.Category.Contains(options.SearchText) || i.Name.Contains(options.SearchText)).ToList();
            }

            // 处理高级查询
            if (options.CustomerSearchs.Any())
            {
                items = items.Where(options.CustomerSearchs.GetFilterFunc<Dict>()).ToList();
            }

            // 处理过滤
            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<Dict>()).ToList();
            }

            // 处理排序
            if (!string.IsNullOrEmpty(options.SortName))
            {
                items = items.Sort<Dict>(options.SortName, options.SortOrder).ToList();
            }
            else
            {
                items = items.OrderBy(i => i.Define).ThenBy(i => i.Category).ThenBy(i => i.Name).ToList();
            }

            ret.TotalCount = items.Count;
            ret.Items = items;
            return Task.FromResult(ret);
        }
    }
}
