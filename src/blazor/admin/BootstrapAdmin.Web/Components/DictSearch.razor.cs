using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DictSearch
    {
        private IEnumerable<SelectedItem>? Items { get; set; } = typeof(EnumDictDefine).ToSelectList(new SelectedItem("", "全部"));

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public DictsSearchModel? Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<DictsSearchModel> ValueChanged { get; set; }
    }
}
