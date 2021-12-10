﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// 验证组件基类
    /// </summary>
    public abstract class ValidatorComponentBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 错误描述信息
        /// </summary>
        [Parameter]
        public string ErrorMessage { get; set; } = "";

        /// <summary>
        /// 获得/设置 IRules 实例
        /// </summary>
        [CascadingParameter]
        public IRules? Input { get; set; }

        /// <summary>
        /// 初始化方法
        /// </summary>
        protected override void OnInitialized()
        {
            if (Input == null)
            {
                throw new InvalidOperationException($"{nameof(ValidatorComponentBase)} requires a cascading " +
                    $"parameter of type {nameof(IRules)}. For example, you can use {nameof(ValidatorComponentBase)} " +
                    $"inside an LgbInputText.");
            }

            Input.Rules.Add(this);
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="context"></param>
        /// <param name="results"></param>
        public abstract void Validate(object? propertyValue, ValidationContext context, List<ValidationResult> results);
    }
}
