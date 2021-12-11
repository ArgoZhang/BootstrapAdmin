using BootstrapAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.EFCore
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapAdminContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public BootstrapAdminContext(DbContextOptions<BootstrapAdminContext> options) : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<Dict>? Dicts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<Navigation>? Navigations { get; set; }
    }
}
