using BootstrapAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<User>? Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<Role>? Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<UserRole>? UserRole { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<Navigation>? Navigations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<NavigationRole>? NavigationRole { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EntityConfiguration.Configure(modelBuilder);
        }
    }
}
