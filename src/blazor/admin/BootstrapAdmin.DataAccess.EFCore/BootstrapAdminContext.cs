// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.EFCore.Models;
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
        [NotNull]
        public DbSet<Group>? Groups { get; set; }

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
        [NotNull]
        public DbSet<RoleApp>? RoleApp { get; set; }

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
