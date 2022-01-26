// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Validators;

/// <summary>
/// 
/// </summary>
public class AppIdValidator : IValidator
{
    private IDict DictService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dictService"></param>
    public AppIdValidator(IDict dictService) => DictService = dictService;

    /// <summary>
    /// 
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyValue"></param>
    /// <param name="context"></param>
    /// <param name="results"></param>
    public void Validate(object? propertyValue, ValidationContext context, List<ValidationResult> results)
    {
        var check = DictService.ExistsAppId(propertyValue?.ToString()!);
        if (check)
        {
            ErrorMessage = $"{context.DisplayName}已存在";
        }
        else
        {
            ErrorMessage = null;
        }
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            results.Add(new ValidationResult(ErrorMessage, new string[] { context.MemberName! }));
        }
    }
}
