using System.ComponentModel;

namespace BootstrapClient.DataAccess.Models;

/// <summary>
/// 资源类型枚举 0 表示菜单 1 表示资源 2 表示按钮
/// </summary>
public enum EnumResource
{
    /// <summary>
    /// 
    /// </summary>
    [Description("菜单")]
    Navigation,
    /// <summary>
    /// 
    /// </summary>
    [Description("资源")]
    Resource,
    /// <summary>
    /// 
    /// </summary>
    [Description("代码块")]
    Block
}
