using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

public class EFRole : Role
{
    /// <summary>
    /// 
    /// </summary>
    public ICollection<User>? Users { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UserRole>? UserRoles { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public ICollection<Navigation>? Navigations { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<NavigationRole>? NavigationRoles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICollection<Group>? Groups { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<RoleGroup>? RoleGroup { get; set; }
}
