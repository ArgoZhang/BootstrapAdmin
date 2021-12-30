using System.ComponentModel;

namespace Bootstrap.DataAccess;

/// <summary>
/// 登录用户信息实体类
/// </summary>
public class LoginLog
{
    /// <summary>
    /// 获得/设置 Id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 获得/设置 用户名
    /// </summary>
    [DisplayName("登录名称")]
    public string UserName { get; set; } = "";

    /// <summary>
    /// 获得/设置 登录时间
    /// </summary>
    [DisplayName("登录时间")]
    public DateTime LoginTime { get; set; }

    /// <summary>
    /// 获得/设置 登录IP地址
    /// </summary>
    [DisplayName("主机")]
    public string Ip { get; set; } = "";

    /// <summary>
    /// 获得/设置 登录浏览器
    /// </summary>
    [DisplayName("浏览器")]
    public string Browser { get; set; } = "";

    /// <summary>
    /// 获得/设置 登录操作系统
    /// </summary>
    [DisplayName("操作系统")]
    public string OS { get; set; } = "";

    /// <summary>
    /// 获得/设置 登录地点
    /// </summary>
    [DisplayName("登录地点")]
    public string City { get; set; } = "";

    /// <summary>
    /// 获得/设置 登录是否成功
    /// </summary>
    [DisplayName("登录结果")]
    public string Result { get; set; } = "";

    /// <summary>
    /// 获得/设置 用户 UserAgent
    /// </summary>
    [DisplayName("登录名称")]
    public string UserAgent { get; set; } = "";
}
