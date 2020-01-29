﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// IValidComponent 接口
    /// </summary>
    public interface IValidateComponent
    {
        /// <summary>
        /// 数据验证方法
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="context"></param>
        /// <param name="results"></param>
        void ValidateProperty(object? propertyValue, ValidationContext context, List<ValidationResult> results);

        /// <summary>
        /// 显示或者隐藏提示信息方法
        /// </summary>
        /// <param name="results"></param>
        /// <param name="validProperty"></param>
        void ToggleMessage(IEnumerable<ValidationResult> results, bool validProperty);
    }
}
