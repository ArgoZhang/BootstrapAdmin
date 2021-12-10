using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 模态框组件类
    /// </summary>
    public class AssignModalBase<TItem> : ComponentBase
    {
#nullable disable
        /// <summary>
        /// 获得/设置 数据绑定项
        /// </summary>
        [Parameter]
        public TItem Item { get; set; }
#nullable restore

        /// <summary>
        /// 获得/设置 Modal 实例
        /// </summary>
        [Parameter]
        public ModalBase? Modal { get; set; }

        /// <summary>
        /// 获得/设置 Id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 获得/设置 弹窗标题
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "未设置";

        /// <summary>
        /// 获得/设置 表格 Toolbar 按钮模板
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ItemTemplate { get; set; }

        /// <summary>
        /// 获得/设置 Items
        /// </summary>
        public IEnumerable<TItem> Items { get; set; } = new TItem[0];

        /// <summary>
        /// 获得/设置 保存回调事件
        /// </summary>
        [Parameter]
        public Action<IEnumerable<TItem>>? OnSave { get; set; }

        /// <summary>
        /// OnAfterRender 方法
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (show)
            {
                show = false;
                Modal?.Toggle();
            }
        }

        /// <summary>
        /// SetParametersAsync 方法
        /// </summary>
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (string.IsNullOrEmpty(Id)) throw new InvalidOperationException("Modal Component Id property must be set");
            return base.SetParametersAsync(ParameterView.Empty);
        }

        /// <summary>
        /// Save 方法
        /// </summary>
        protected void Save()
        {
            OnSave?.Invoke(Items);
            Modal?.Toggle();
        }

        private bool show;
        /// <summary>
        /// Update 方法
        /// </summary>
        public void Update(IEnumerable<TItem> items)
        {
            Items = items;
            show = true;
            StateHasChanged();
        }
    }
}
