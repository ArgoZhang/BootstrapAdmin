// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class SelectedItemExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<User> users) => users.Select(i => new SelectedItem { Value = i.Id!, Text = i.ToString() }).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<Role> roles) => roles.Select(i => new SelectedItem { Value = i.Id!, Text = i.RoleName }).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groups"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<Group> groups) => groups.Select(i => new SelectedItem { Value = i.Id!, Text = i.ToString() }).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this Dictionary<string, string> dict) => dict.Select(i => new SelectedItem { Value = i.Key, Text = i.Value }).ToList();
}
