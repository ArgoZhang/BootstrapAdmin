using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// Table Toolbar 按钮呈现组件
    /// </summary>
    public class TableToolbarContent : ComponentBase
    {
        /// <summary>
        /// 获得/设置 Table Toolbar 实例
        /// </summary>
        [CascadingParameter]
        protected TableToolbarBase? Toolbar { get; set; }

        /// <summary>
        /// 渲染组件方法
        /// </summary>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // 渲染正常按钮
            if (Toolbar != null && Toolbar.Buttons.Any())
            {
                // 渲染 Toolbar 按钮
                //<div class="toolbar btn-group">
                //  <button type="button" class="btn btn-success"><i class="fa fa-plus" aria-hidden="true"></i><span>新增</span></button>
                //  <button type="button" class="btn btn-danger"><i class="fa fa-remove" aria-hidden="true"></i><span>删除</span></button>
                //  <button type="button" class="btn btn-primary"><i class="fa fa-pencil" aria-hidden="true"></i><span>编辑</span></button>
                //</div>
                var index = 0;
                builder.OpenElement(index++, "div");
                builder.AddAttribute(index++, "class", "toolbar btn-group");
                foreach (var button in Toolbar.Buttons)
                {
                    builder.OpenElement(index++, "button");
                    builder.AddAttribute(index++, "type", "button");
                    builder.AddMultipleAttributes(index++, button.AdditionalAttributes);
                    builder.AddAttribute(index++, "onclick", button.OnClick);

                    // icon
                    builder.OpenElement(index++, "i");

                    // class="fa fa-plus" aria-hidden="true"
                    builder.AddAttribute(index++, "class", button.Icon);
                    builder.AddAttribute(index++, "aria-hidden", "true");
                    builder.CloseElement();

                    // span
                    builder.OpenElement(index++, "span");
                    builder.AddContent(index++, button.Title);
                    builder.CloseElement();
                    builder.CloseElement();
                }
                builder.CloseElement();

                // 渲染移动版按钮
                //<div class="gear btn-group">
                //  <button class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" type="button"><i class="fa fa-gear"></i></button>
                //  <div class="dropdown-menu">
                //      <div class="dropdown-item" title="新增" @onclick="Add" asp-auth="add"><i class="fa fa-plus"></i></div>
                //      <div class="dropdown-item" title="删除" @onclick="Delete" asp-auth="del"><i class="fa fa-remove"></i></div>
                //      <div class="dropdown-item" title="编辑" @onclick="Edit" asp-auth="edit"><i class="fa fa-pencil"></i></div>
                //  </div>
                //</div>
                builder.OpenElement(index++, "div");
                builder.AddAttribute(index++, "class", "gear btn-group");

                builder.OpenElement(index++, "button");
                builder.AddAttribute(index++, "class", "btn btn-secondary dropdown-toggle");
                builder.AddAttribute(index++, "data-toggle", "dropdown");
                builder.AddAttribute(index++, "type", "button");

                // i
                builder.OpenElement(index++, "i");
                builder.AddAttribute(index++, "class", "fa fa-gear");
                builder.CloseElement();
                builder.CloseElement(); // end button

                // div dropdown-menu
                builder.OpenElement(index++, "div");
                builder.AddAttribute(index++, "class", "dropdown-menu");

                foreach (var button in Toolbar.Buttons)
                {
                    builder.OpenElement(index++, "div");
                    builder.AddAttribute(index++, "class", "dropdown-item");
                    builder.AddAttribute(index++, "title", button.Title);
                    builder.AddAttribute(index++, "onclick", EventCallback.Factory.Create(button, button.OnClick));

                    // icon
                    builder.OpenElement(index++, "i");

                    // class="fa fa-plus" aria-hidden="true"
                    builder.AddAttribute(index++, "class", button.Icon);
                    builder.AddAttribute(index++, "aria-hidden", "true");
                    builder.CloseElement(); // end i

                    builder.CloseElement(); // end div
                }
                builder.CloseElement(); // end dropdown-menu
                builder.CloseElement(); // end div
            }
        }
    }
}
