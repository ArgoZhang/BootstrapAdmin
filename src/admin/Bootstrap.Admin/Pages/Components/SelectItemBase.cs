using Microsoft.AspNetCore.Components;
using System;

namespace Bootstrap.Admin.Pages.Components
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
        public SelectedItem Item { get; set; } = new SelectedItem();

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Action<SelectedItem> ItemClickCallback { get; set; } = new Action<SelectedItem>(SelectedItem => { });
    }
}
