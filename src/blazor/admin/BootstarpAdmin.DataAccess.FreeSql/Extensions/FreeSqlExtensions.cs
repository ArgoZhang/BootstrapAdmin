// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.FreeSql.Extensions;

static class FreeSqlExtensions
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
        freeSql.CodeFirst.ConfigEntity<Trace>(i =>
        {
            i.Name("Traces");
        });
        freeSql.CodeFirst.ConfigEntity<Group>(i =>
        {
            i.Name("Groups");
        });
        freeSql.CodeFirst.ConfigEntity<Role>(i =>
        {
            i.Name("Roles");
        });
        freeSql.CodeFirst.ConfigEntity<Error>(i =>
        {
            i.Name("Exceptions");
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
