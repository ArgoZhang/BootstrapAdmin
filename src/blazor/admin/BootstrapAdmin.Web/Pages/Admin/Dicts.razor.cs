using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Pages.Admin
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

            EditDefines = typeof(EnumDictDefine).ToSelectList();
            LookUp = EditDefines;
        }
    }
}
