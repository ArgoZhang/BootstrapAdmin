using Bootstrap.Admin.Blazor.Models;
using Bootstrap.Security;
using BootstrapBlazor.Components;

namespace Bootstrap.Admin.Blazor.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dicts
    {
        private IEnumerable<SelectedItem>? EditDefines { get; set; }

        private IEnumerable<SelectedItem>? LookUp { get; set; }

        private ITableSearchModel? DictsSearchModel { get; set; } = new DictsSearchModel();

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            EditDefines = new List<SelectedItem>()
            {
                new SelectedItem("0","系统使用"),
                new SelectedItem("1","自定义"),
            };

            LookUp = EditDefines;
        }

        private Task<(IEnumerable<BootstrapDict>, int)> OnQueryAsync(QueryPageOptions options)
        {
            var items = DataAccess.DictHelper.RetrieveDicts();
            var total = items.Count();

            // 处理高级搜索
            if (options.Searchs.Any())
            {
                items = items.Where(options.Searchs.GetFilterFunc<BootstrapDict>());
            }
            else
            {
                // 处理 SearchText 模糊搜索
                if (!string.IsNullOrEmpty(options.SearchText))
                {
                    items = items.Where(item => (item.Name?.Contains(options.SearchText) ?? false)
                        || (item.Category?.Contains(options.SearchText) ?? false));
                }
            }

            if (!string.IsNullOrEmpty(options.SortName))
            {
                items = items.Sort(options.SortName, options.SortOrder);
            }

            if (options.Filters.Any())
            {
                var aa = options.Filters.GetFilterFunc<BootstrapDict>();
                items = items.Where(options.Filters.GetFilterFunc<BootstrapDict>());
            }
            return Task.FromResult((items, total));
        }

        private Task<bool> OnDeleteAsync(IEnumerable<BootstrapDict> dicts)
        {
            var ids = dicts.Select(s => s.Id!);
            return Task.FromResult(DataAccess.DictHelper.Delete(ids));
        }

        private Task<bool> OnAddOrUpdateAsync(BootstrapDict dicts, ItemChangedType changedType)
        {
            if (ItemChangedType.Add == changedType)
            {
                return Task.FromResult(DataAccess.DictHelper.Save(dicts));
            }
            else
            {
                return Task.FromResult(DataAccess.DictHelper.Save(dicts));
            }
        }
    }
}
