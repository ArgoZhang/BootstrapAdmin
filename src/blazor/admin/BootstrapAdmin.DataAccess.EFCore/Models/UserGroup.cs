using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

public class UserGroup
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

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
    public string? GroupId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Group? Group { get; set; }
}
