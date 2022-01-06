using BootstrapAdmin.Web.Core;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Validators;

public class UserNameValidator : IValidator
{
    public string? ErrorMessage { get; set; }

    private IUser UserService { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userService"></param>
    public UserNameValidator(IUser userService) => UserService = userService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyValue"></param>
    /// <param name="context"></param>
    /// <param name="results"></param>
    public void Validate(object? propertyValue, ValidationContext context, List<ValidationResult> results)
    {
        var displayName = UserService.GetUserByUserName(propertyValue?.ToString())?.DisplayName;
        if (!string.IsNullOrEmpty(displayName))
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
