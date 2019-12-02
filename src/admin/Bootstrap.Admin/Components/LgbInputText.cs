using Bootstrap.Admin.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// LgbInputText 组件
    /// </summary>
    public class LgbInputText : InputText, IValidateComponent
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter]
        public LgbEditFormBase? EditForm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected string ErrorMessage { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        protected string ValidCss { get; set; } = "";

        private string _tooltipMethod = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="context"></param>
        /// <param name="results"></param>
        public void ValidateProperty(object? propertyValue, ValidationContext context, List<ValidationResult> results)
        {
            Rules.ToList().ForEach(validator => validator.Validate(propertyValue, context, results));
        }

        /// <summary>
        /// 显示/隐藏验证结果方法
        /// </summary>
        /// <param name="results"></param>
        /// <param name="validProperty">是否对本属性进行数据验证</param>
        public void ToggleMessage(IEnumerable<ValidationResult> results, bool validProperty)
        {
            if (results.Where(r => r.MemberNames.Any(m => m == FieldIdentifier.FieldName)).Any())
            {
                ErrorMessage = results.First().ErrorMessage;
                ValidCss = "is-invalid";

                // 控件自身数据验证时显示 tooltip
                // EditForm 数据验证时调用 tooltip('enable') 保证 tooltip 组件生成
                // 调用 tooltip('hide') 后导致鼠标悬停时 tooltip 无法正常显示
                _tooltipMethod = validProperty ? "show" : "enable";
            }
            else
            {
                ErrorMessage = "";
                ValidCss = "is-valid";
                _tooltipMethod = "dispose";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            EditForm?.AddValidator((FieldIdentifier.Model.GetType(), FieldIdentifier.FieldName), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (!string.IsNullOrEmpty(_tooltipMethod) && !string.IsNullOrEmpty(Id))
            {
                JSRuntime.Tooltip(Id, _tooltipMethod);
                _tooltipMethod = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ValidatorComponentBase> Rules { get; } = new HashSet<ValidatorComponentBase>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "class", $"{CssClass} {ValidCss}");
            builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValue));
            builder.AddAttribute(4, "oninput", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
            if (!string.IsNullOrEmpty(ErrorMessage)) builder.AddAttribute(5, "data-original-title", ErrorMessage);
            if (!string.IsNullOrEmpty(Id)) builder.AddAttribute(6, "id", Id);
            builder.OpenComponent<CascadingValue<LgbInputText>>(7);
            builder.AddAttribute(8, "IsFixed", true);
            builder.AddAttribute(9, "Value", this);
            builder.AddAttribute(10, "ChildContent", ChildContent);
            builder.CloseComponent();
            builder.CloseElement();
        }
    }
}
