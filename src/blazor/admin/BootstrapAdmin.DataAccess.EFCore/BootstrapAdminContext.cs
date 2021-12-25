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
        public DbSet<Navigation>? Navigations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Ignore(u => u.Checked);
            modelBuilder.Entity<User>().Ignore(u => u.Period);
            modelBuilder.Entity<User>().Ignore(u => u.NewPassword);
            modelBuilder.Entity<User>().Ignore(u => u.IsReset);
        }
    }
}
