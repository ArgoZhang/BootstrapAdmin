using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

public class UserRole
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RoleId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Role? Role { get; set; }
}
