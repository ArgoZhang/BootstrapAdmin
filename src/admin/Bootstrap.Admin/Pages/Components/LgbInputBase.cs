using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// LgbInputText 组件
    /// </summary>
    public class LgbInputBase<TItem> : ValidateInputBase<TItem>
    {
        /// <summary>
        /// 获得/设置 控件样式 默认为 col-sm-6
        /// </summary>
        [Parameter]
        public string ColumnClass { get; set; } = "col-sm-6";

        /// <summary>
        /// 获得/设置 控件 type 属性 默认为 text
        /// </summary>
        [Parameter]
        public string InputType { get; set; } = "text";

        /// <summary>
        /// 获取 最大长度属性
        /// </summary>
        protected int? MaxLength
        {
            get
            {
                if (Rules.Count == 0 &&
                    AdditionalAttributes != null &&
                    AdditionalAttributes.TryGetValue("maxlength", out var maxlength) &&
                    int.TryParse(Convert.ToString(maxlength), out int ml))
                {
                    return ml;
                }
                return (Rules.FirstOrDefault(r => r is StringLengthValidator) as StringLengthValidator)?.Length;
            }
        }
    }
}
