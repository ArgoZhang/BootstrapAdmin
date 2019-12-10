using Bootstrap.Admin.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq.Expressions;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class LgbTableHeader<TItem> : ComponentBase
    {
#nullable disable
        /// <summary>
        /// 
        /// </summary>
        [Parameter] public TItem Value { get; set; }
#nullable restore

        /// <summary>
        /// 
        /// </summary>
        [Parameter] public EventCallback<TItem> ValueChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter] public Expression<Func<TItem>>? ValueExpression { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var filed = FieldIdentifier.Create(ValueExpression);
            var text = filed.GetDisplayName();
            builder.OpenElement(0, "th");
            builder.AddContent(1, text);
            builder.CloseElement();
        }
    }
}
