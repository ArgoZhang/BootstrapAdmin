using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.Models;

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
