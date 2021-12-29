using BootstrapAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.EFCore;

public static class EntityConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void Configure(this ModelBuilder builder)
    {
        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().Ignore(u => u.Period);
        builder.Entity<User>().Ignore(u => u.NewPassword);
        builder.Entity<User>().Ignore(u => u.ConfirmPassword);
        builder.Entity<User>().Ignore(u => u.IsReset);
        builder.Entity<User>().HasMany(s => s.Roles).WithMany(s => s.Users).UsingEntity<UserRole>(s =>
        {
            s.HasOne(s => s.User).WithMany(s => s.UserRoles).HasForeignKey(s => s.UserId);
            s.HasOne(s => s.Role).WithMany(s => s.UserRoles).HasForeignKey(s => s.RoleId);
        });
        builder.Entity<User>().HasMany(s => s.Groups).WithMany(s => s.Users).UsingEntity<UserGroup>(s =>
        {
            s.HasOne(s => s.User).WithMany(s => s.UserGroup).HasForeignKey(s => s.UserId);
            s.HasOne(s => s.Group).WithMany(s => s.UserGroup).HasForeignKey(s => s.GroupId);
        });

        builder.Entity<User>().Ignore(s => s.NewPassword);
        builder.Entity<User>().Ignore(s => s.Period);
        builder.Entity<User>().Ignore(s => s.IsReset);

        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<Role>().HasKey(s => s.Id);
        builder.Entity<Role>().Property(s => s.Id).IsRequired();
        builder.Entity<Role>().HasMany(s => s.Navigations).WithMany(s => s.Roles).UsingEntity<NavigationRole>(s =>
        {
            s.HasOne(s => s.Navigation).WithMany(s => s.NavigationRoles).HasForeignKey(s => s.NavigationId);
            s.HasOne(s => s.Role).WithMany(s => s.NavigationRoles).HasForeignKey(s => s.RoleId);
        });
        builder.Entity<Role>().HasMany(s => s.Groups).WithMany(s => s.Roles).UsingEntity<RoleGroup>(s =>
        {
            s.HasOne(s => s.Group).WithMany(s => s.RoleGroup).HasForeignKey(s => s.GroupId);
            s.HasOne(s => s.Role).WithMany(s => s.RoleGroup).HasForeignKey(s => s.RoleId);
        });

        builder.Entity<Navigation>().ToTable("Navigations");
        builder.Entity<Navigation>().HasKey(s => s.Id);
        builder.Entity<Navigation>().Ignore(s => s.HasChildren);

        builder.Entity<Dict>();
    }
}
