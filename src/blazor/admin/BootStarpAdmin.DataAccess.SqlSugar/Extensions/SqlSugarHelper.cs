// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.DataAccess.SqlSugar.Extensions;

static class SqlSugarHelper
{
    public static ConfigureExternalServices InitConfigureExternalServices()
    {
        return new ConfigureExternalServices()
        {
            EntityNameService = (type, entity) =>
            {
                if (entity.DbTableName == nameof(User))
                {
                    entity.DbTableName = "Users";
                }
                else if (entity.DbTableName == nameof(Dict))
                {
                    entity.DbTableName = "Dicts";
                }
                else if (entity.DbTableName == nameof(Navigation))
                {
                    entity.DbTableName = "Navigations";
                }
                else if (entity.DbTableName == nameof(Group))
                {
                    entity.DbTableName = "Groups";
                }
                else if (entity.DbTableName == nameof(Role))
                {
                    entity.DbTableName = "Roles";
                }
                else if (entity.DbTableName == nameof(Error))
                {
                    entity.DbTableName = "Errors";
                }
                else if (entity.DbTableName == nameof(Trace))
                {
                    entity.DbTableName = "Traces";
                }
                else if (entity.DbTableName == nameof(LoginLog))
                {
                    entity.DbTableName = "LoginLogs";
                }
            },
            EntityService = (type, column) =>
            {
                if (column.DbTableName == "Users")
                {
                    if (column.DbColumnName == nameof(User.Period)
                    || column.DbColumnName == nameof(User.ConfirmPassword)
                    || column.DbColumnName == nameof(User.NewPassword)
                    || column.DbColumnName == nameof(User.IsReset)
                    )
                    {
                        column.IsIgnore = true;
                    }
                }
                else if (column.DbTableName == "Navigations")
                {
                    if (column.DbColumnName == nameof(Navigation.HasChildren))
                    {
                        column.IsIgnore = true;
                    }
                }
                if (column.DbColumnName.ToUpper() == "ID")
                {
                    column.IsPrimarykey = true;
                    column.IsIdentity = true;
                }
            }
        };
    }
}
