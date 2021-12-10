using Microsoft.AspNetCore.Components.Routing;

namespace BootstrapAdmin.DataAccess.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MenuItemExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static MenuItem Parse(this Models.Menu menu) => new()
        {
            Text = menu.Name,
            Url = menu.Url.Replace("~", ""),
            Icon = menu.Icon,
            Match = NavLinkMatch.All,
            Target = menu.Target,
            Id = menu.Id,
            ParentId = menu.ParentId
        };
    }
}
