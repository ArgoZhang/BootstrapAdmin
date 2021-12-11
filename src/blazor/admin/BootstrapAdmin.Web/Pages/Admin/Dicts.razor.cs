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

            EditDefines = new List<SelectedItem>()
            {
                new SelectedItem("0","系统使用"),
                new SelectedItem("1","自定义"),
            };

            LookUp = EditDefines;
        }
    }
}
