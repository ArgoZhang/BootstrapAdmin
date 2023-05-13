// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using PetaPoco;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess;

static class OrderByHelper
{
    private static List<string> OrderByDirection { get; } = new List<string>() { "", "Asc", "Desc" };
    public static Sql SafeOrderBy<T>(this Sql sql, string columnName, string order)
    {
        var properties = typeof(T).GetProperties();
        if (OrderByDirection.Any(i => i.Equals(order, System.StringComparison.OrdinalIgnoreCase))
            && properties.Any(i => i.Name.Equals(columnName, System.StringComparison.OrdinalIgnoreCase)))
        {
            sql.OrderBy($"{columnName} {order}");
        }
        return sql;
    }
}
