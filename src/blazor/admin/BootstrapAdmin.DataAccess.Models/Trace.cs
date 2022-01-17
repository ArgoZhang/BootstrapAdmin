namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class Trace
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime LogTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Ip { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Browser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? OS { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Referer { get; set; }
}
