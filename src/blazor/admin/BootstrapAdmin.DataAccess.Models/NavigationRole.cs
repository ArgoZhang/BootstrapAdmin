namespace BootstrapAdmin.DataAccess.Models;

public class NavigationRole
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

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
