using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

public class AppInfo
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "系统名称")]
    [Required(ErrorMessage = "{0}不可为空")]
    [NotNull]
    public string? Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "网站页脚")]
    [Required(ErrorMessage = "{0}不可为空")]
    [NotNull]
    public string? Footer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "登录地址")]
    [NotNull]
    public string? Login { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "后台地址")]
    [NotNull]
    public string? AuthUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    public string? Theme { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "系统演示")]
    public bool IsDemo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "授权码")]
    [Required(ErrorMessage = "{0}不可为空")]
    [NotNull]
    public string? AuthCode { get; set; }
}
