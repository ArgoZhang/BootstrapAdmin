using BootstrapAdmin.DataAccess.Extensions;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuService : IMenu
    {
        private IDatabase _db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public MenuService(IDatabase db) => _db = db;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MenuItem> GetAdminMenusByUser(string userName)
        {
            var menus = RetrieveAllMenus(userName).Where(m => m.Category == "0" && m.IsResource == 0);
            return CascadeMenus(menus);
        }

        /// <summary>
        /// 获得带层次关系的菜单集合
        /// </summary>
        /// <param name="menus">未层次化菜单集合</param>
        /// <returns>带层次化的菜单集合</returns>
        public static IEnumerable<MenuItem> CascadeMenus(IEnumerable<Models.Menu> menus)
        {
            var root = menus.Where(m => m.ParentId == "0")
                            .OrderBy(m => m.Category).ThenBy(m => m.Application).ThenBy(m => m.Order)
                            .Select(m => m.Parse())
                            .ToList();
            CascadeMenus(menus, root);
            return root;
        }

        private static void CascadeMenus(IEnumerable<Models.Menu> navs, List<MenuItem> level)
        {
            level.ForEach(m =>
            {
                m.Items = navs.Where(sub => sub.ParentId == m.Id).OrderBy(sub => sub.Order).Select(sub => sub.Parse()).ToList();
                CascadeMenus(navs, m.Items.ToList());
            });
        }

        /// <summary>
        /// 获得指定用户名可访问的所有菜单集合
        /// </summary>
        /// <param name="userName">当前用户名</param>
        /// <returns>未层次化的菜单集合</returns>
        private List<Models.Menu> RetrieveAllMenus(string userName)
        {
            var order = _db.Provider.EscapeSqlIdentifier("Order");
            return _db.Fetch<Models.Menu>($"select n.ID, n.ParentId, n.Name, n.{order}, n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application, d.Name as CategoryName, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = @Category and d.Define = 0 left join Navigations ln on n.ParentId = ln.ID inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID", new { UserName = userName, Category = "菜单", RoleName = "Administrators" });
        }
    }
}
