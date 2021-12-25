using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

public class AppInfo
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "系统名称")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string? Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "网站页脚")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string? Footer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "登录地址")]
    public string? Login { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "后台地址")]
    public string? AuthUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Theme { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDemo { get; set; }
}
