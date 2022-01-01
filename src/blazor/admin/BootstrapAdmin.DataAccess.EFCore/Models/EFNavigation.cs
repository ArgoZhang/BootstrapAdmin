using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

public class EFNavigation : Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public ICollection<Role>? Roles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<NavigationRole>? NavigationRoles { get; set; }
}
