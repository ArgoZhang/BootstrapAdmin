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
    }
}
