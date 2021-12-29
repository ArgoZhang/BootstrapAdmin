using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.Models;

public class RoleGroup
{
    public string? Id { get; set; }

    public string? RoleId { get; set; }

    public Role? Role { get; set; }

    public string? GroupId { get; set; }

    public Group? Group { get; set; }
}
