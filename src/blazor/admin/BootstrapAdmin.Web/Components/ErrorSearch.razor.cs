using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Utils;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ErrorSearch
    {
        private List<SelectedItem>? Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public ErrorSearchModel? Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<ErrorSearchModel> ValueChanged { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Items = new List<SelectedItem>
            {
                new SelectedItem("", "全部")
            };
            Items.AddRange(LookupHelper.GetExceptionCategory());
        }
    }
}
