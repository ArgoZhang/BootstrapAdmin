using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ValidatorComponentBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string ErrorMessage { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter]
        public LgbInputTextBase? Input { get; set; }

        /// <summary>
        /// 初始化方法
        /// </summary>
        protected override void OnInitialized()
        {
            if (Input == null)
            {
                throw new InvalidOperationException($"{nameof(ValidatorComponentBase)} requires a cascading " +
                    $"parameter of type {nameof(LgbInputTextBase)}. For example, you can use {nameof(ValidatorComponentBase)} " +
                    $"inside an LgbInputText.");
            }

            Input.Rules.Add(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="context"></param>
        /// <param name="results"></param>
        public abstract void Validate(object? propertyValue, ValidationContext context, List<ValidationResult> results);
    }
}
