using Bootstrap.Admin.Shared;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectItemBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool Active { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Text { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Value { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        [CascadingParameter]
        public Select? Select { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (Active) Select?.ActiveChanged?.Invoke(this);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void ClickItem()
        {
            Select?.ClickItemCallback?.Invoke(this);
        }
    }
}
