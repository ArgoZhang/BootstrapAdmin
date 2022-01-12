using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
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
    [Display(Name = "登录首页")]
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
    [Display(Name = "网站主题")]
    [NotNull]
    public string? Theme { get; set; }

    /// <summary>
    /// 是否开启默认应用功能
    /// </summary>
    [Display(Name = "默认应用")]
    public bool EnableDefaultApp { get; set; }

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
