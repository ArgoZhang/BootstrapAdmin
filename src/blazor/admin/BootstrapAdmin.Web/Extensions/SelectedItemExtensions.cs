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
    /// <param name="users"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<Role> roles) => roles.Select(i => new SelectedItem { Value = i.Id!, Text = i.RoleName }).ToList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public static List<SelectedItem> ToSelectedItemList(this IEnumerable<Group> groups) => groups.Select(i => new SelectedItem { Value = i.Id!, Text = i.ToString() }).ToList();
}
