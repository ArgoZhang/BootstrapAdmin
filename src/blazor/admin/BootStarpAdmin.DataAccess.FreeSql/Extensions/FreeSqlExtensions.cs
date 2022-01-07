using BootStarpAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.DataAccess.Models;

namespace BootStarpAdmin.DataAccess.FreeSql.Extensions;

public static class FreeSqlExtensions
{
    public static void Mapper(this IFreeSql freeSql)
    {
        freeSql.CodeFirst.ConfigEntity<Navigation>(i =>
        {
            i.Name("Navigations");
            i.Property(n => n.HasChildren).IsIgnore(true);
        });
        freeSql.CodeFirst.ConfigEntity<User>(i =>
        {
            i.Name("Users");
            i.Property(n => n.NewPassword).IsIgnore(true);
            i.Property(n => n.ConfirmPassword).IsIgnore(true);
            i.Property(n => n.Period).IsIgnore(true);
            i.Property(n => n.IsReset).IsIgnore(true);
        });
        freeSql.CodeFirst.ConfigEntity<Group>(i =>
        {
            i.Name("Groups");
        });
        freeSql.CodeFirst.ConfigEntity<Role>(i =>
        {
            i.Name("Roles");
        });
        freeSql.CodeFirst.ConfigEntity<UserRole>(i =>
        {
            i.Name("UserRole");
            i.Property(s => s.ID).IsIgnore(true);
        });
        freeSql.CodeFirst.ConfigEntity<NavigationRole>(i =>
        {
            i.Name("NavigationRole");
            i.Property(s => s.ID).IsIgnore(true);
        });
        freeSql.CodeFirst.ConfigEntity<UserGroup>(i =>
        {
            i.Name("UserGroup");
            i.Property(s => s.ID).IsIgnore(true);
        });
        freeSql.CodeFirst.ConfigEntity<RoleGroup>(i =>
        {
            i.Name("RoleGroup");
            i.Property(s => s.ID).IsIgnore(true);
        });
    }
}
