using System.ComponentModel;

namespace BootstrapClient.DataAccess.Models;

/// <summary>
/// 菜单分类 0 表示系统菜单 1 表示用户自定义菜单
/// </summary>
public enum EnumNavigationCategory
{
    /// <summary>
    /// 系统使用
    /// </summary>
    [Description("系统使用")]
    System,

    /// <summary>
    /// 用户自定义
    /// </summary>
    [Description("自定义")]
    Customer
}
