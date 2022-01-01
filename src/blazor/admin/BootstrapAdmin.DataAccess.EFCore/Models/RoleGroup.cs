using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.EFCore.Models;

public class RoleGroup
{
    public string? Id { get; set; }

    public string? RoleId { get; set; }

    public Role? Role { get; set; }

    public string? GroupId { get; set; }

    public Group? Group { get; set; }
}
