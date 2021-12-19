using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models
{
    /// <summary>
    /// Role 实体类
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 获得/设置 角色主键ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 角色名称
        /// </summary>
        [DisplayName("角色名称")]
        [NotNull]
        public string? RoleName { get; set; }

        /// <summary>
        /// 获得/设置 角色描述
        /// </summary>
        [DisplayName("角色描述")]
        [NotNull]
        public string? Description { get; set; } 
    }
}
