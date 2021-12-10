using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// Table Toolbar 组件
    /// </summary>
    public class TableToolbarBase : ComponentBase
    {
        /// <summary>
        /// Specifies the content to be rendered inside this
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 添加按钮到工具栏方法
        /// </summary>
        public void AddButtons(TableToolbarButton button) => Buttons.Add(button);

        /// <summary>
        /// 获得 Toolbar 按钮集合
        /// </summary>
        public ICollection<TableToolbarButton> Buttons { get; } = new HashSet<TableToolbarButton>();
    }
}
