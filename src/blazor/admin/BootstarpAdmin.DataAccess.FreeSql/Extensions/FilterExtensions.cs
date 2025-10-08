// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using FreeSql;

namespace BootstrapAdmin.DataAccess.FreeSql.Extensions;

/// <summary>
/// 
/// </summary>
static class FilterExtensions
{
    public static ISelect<TModel> PageIf<TModel>(this ISelect<TModel> source, int pageIndex, int pageItems, bool isPage) => isPage
        ? source.Page(pageIndex, pageItems)
        : source;
}
