using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// LgbInputText 组件
    /// </summary>
    public class LgbInputTextBase : ValidateInputBase<string>
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string ColumnClass { get; set; } = "col-sm-6";

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string InputType { get; set; } = "text";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validationErrorMessage"></param>
        /// <returns></returns>
        protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = "";
            return true;
        }

        /// <summary>
        /// 
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
