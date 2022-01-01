using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

public class EFUser : User
{
    /// <summary>
    /// 
    /// </summary>
    public ICollection<Role>? Roles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UserRole>? UserRoles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICollection<Group>? Groups { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICollection<UserGroup>? UserGroup { get; set; }
}
