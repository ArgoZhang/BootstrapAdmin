using Bootstrap.Admin.Blazor.Models;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Task = System.Threading.Tasks.Task;

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

        private static Task<QueryData<BootstrapDict>> OnQueryAsync()
        {
            var items = DictHelper.RetrieveDicts();
            return Task.FromResult(new QueryData<BootstrapDict>()
            {
                Items = items,
                TotalCount = items.Count()
            });
        }

        private Task<bool> OnDeleteAsync(IEnumerable<BootstrapDict> dicts)
        {
            var ids = dicts.Select(s => s.Id!);
            return Task.FromResult(DictHelper.Delete(ids));
        }

        private Task<bool> OnAddOrUpdateAsync(BootstrapDict dict)
        {
            return Task.FromResult(DictHelper.Save(dict));
        }
    }
}
