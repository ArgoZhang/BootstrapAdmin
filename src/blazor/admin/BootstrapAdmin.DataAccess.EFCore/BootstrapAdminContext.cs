﻿using BootstrapAdmin.DataAccess.EFCore.Models;
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
        public DbSet<EFUser>? Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<EFRole>? Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<UserRole>? UserRole { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<EFNavigation>? Navigations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<NavigationRole>? NavigationRole { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<EFGroup>? Groups { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<UserGroup>? UserGroup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DbSet<RoleGroup>? RoleGroup { get; set; }

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
