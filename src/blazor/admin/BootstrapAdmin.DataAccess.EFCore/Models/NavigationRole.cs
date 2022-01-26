using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

/// <summary>
/// 
/// </summary>
public class NavigationRole
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? NavigationId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Navigation? Navigation { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RoleId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Role? Role { get; set; }
}
