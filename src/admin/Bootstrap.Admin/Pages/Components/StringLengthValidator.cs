using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class StringLengthValidator : ValidatorComponentBase
    {
        private int _length = 50;
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public int Length
        {
            get { return _length; }
            set
            {
                _length = value;
                ErrorMessage = $"不可为空，{_length}字以内";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="context"></param>
        /// <param name="results"></param>
        public override void Validate(object? propertyValue, ValidationContext context, List<ValidationResult> results)
        {
            var val = propertyValue?.ToString() ?? "";
            if (val.Length > Length) results.Add(new ValidationResult(ErrorMessage, string.IsNullOrEmpty(context.MemberName) ? Enumerable.Empty<string>() : new string[] { context.MemberName }));
        }
    }
}
