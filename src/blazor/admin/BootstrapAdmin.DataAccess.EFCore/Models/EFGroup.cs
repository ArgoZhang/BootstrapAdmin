using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

public class EFGroup : Group
{
    /// <summary>
    /// 
    /// </summary>
    public ICollection<User>? Users { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICollection<UserGroup>? UserGroup { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICollection<Role>? Roles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<RoleGroup>? RoleGroup { get; set; }
}
