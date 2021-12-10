using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// Table Header 表头组件
    /// </summary>
    public class TableHeaderContent : ComponentBase
    {
        /// <summary>
        /// 获得/设置 Table Header 实例
        /// </summary>
        [CascadingParameter]
        protected TableHeaderBase? Header { get; set; }

        /// <summary>
        /// 获得/设置 升序图标
        /// </summary>
        [Parameter]
        public string SortIconAsc { get; set; } = "fa fa-sort-asc";

        /// <summary>
        /// 获得/设置 降序图标
        /// </summary>
        [Parameter]
        public string SortIconDesc { get; set; } = "fa fa-sort-desc";

        /// <summary>
        /// 获得/设置 默认图标
        /// </summary>
        [Parameter]
        public string SortDefault { get; set; } = "fa fa-sort";

        private string sortName = "";
        private SortOrder sortOrder;

        /// <summary>
        /// 渲染组件方法
        /// </summary>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // 渲染正常按钮
            if (Header != null)
            {
                var index = 0;
                foreach (var header in Header.Headers)
                {
                    var fieldName = header.GetFieldName();
                    var displayName = header.GetDisplayName();
                    builder.OpenElement(index++, "th");
                    builder.AddMultipleAttributes(index++, header.AdditionalAttributes);

                    // 如果允许排序
                    if (header.Sort)
                    {
                        builder.AddAttribute(index++, "onclick", new Action(() =>
                        {
                            if (sortName != fieldName) sortOrder = SortOrder.Asc;
                            else sortOrder = sortOrder == SortOrder.Asc ? SortOrder.Desc : SortOrder.Asc;
                            sortName = fieldName;

                            // 通知 Table 组件刷新数据
                            Header.OnSort?.Invoke(sortName, sortOrder);
                            StateHasChanged();
                        }));
                        builder.AddAttribute(index++, "class", "sortable");
                    }
                    builder.OpenElement(index++, "span");
                    builder.AddContent(index++, displayName);
                    builder.CloseElement();
                    if (header.Sort)
                    {
                        builder.OpenElement(index++, "i");
                        var order = sortName == fieldName ? sortOrder : SortOrder.Unset;
                        var icon = order switch
                        {
                            SortOrder.Asc => SortIconAsc,
                            SortOrder.Desc => SortIconDesc,
                            _ => SortDefault
                        };
                        builder.AddAttribute(index++, "class", icon);
                        builder.CloseElement();
                    }
                    builder.CloseElement();
                }
            }
        }
    }

    /// <summary>
    /// 排序顺序枚举类型
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// 未设置
        /// </summary>
        Unset,
        /// <summary>
        /// 升序 0-9 A-Z
        /// </summary>
        Asc,
        /// <summary>
        /// 降序 9-0 Z-A
        /// </summary>
        Desc,
    }
}
