using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dicts
    {
        private ITableSearchModel DictsSearchModel { get; set; } = new DictsSearchModel();

        private List<string> SortList { get; } = new List<string> { "Define", "Category", "Name" };
    }
}
