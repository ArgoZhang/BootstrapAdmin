using BootstrapAdmin.Web.Core;
using BootstrapAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    class NavigationsService : INavigation
    {
        private IDbContextFactory<BootstrapAdminContext> DbFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        public NavigationsService(IDbContextFactory<BootstrapAdminContext> factory) => DbFactory = factory;

        /// <summary>
        /// 获得指定用户名可访问的所有菜单集合
        /// </summary>
        /// <param name="userName">当前用户名</param>
        /// <returns>未层次化的菜单集合</returns>
        public List<Navigation> GetAllMenus(string userName)
        {
            using var context = DbFactory.CreateDbContext();

            var user = context.Set<User>().Include(s => s.Roles).ThenInclude(s => s.Navigations.Where(s => s.IsResource == EnumResource.Navigation)).AsSplitQuery().FirstOrDefault(s => s.UserName == userName);

            if (user == null)
                return new List<Navigation>();
            return user.Roles.SelectMany(s => s.Navigations).ToList();
        }

        public List<string?> GetMenusByRoleId(string? roleId)
        {
            using var context = DbFactory.CreateDbContext();

            return context.NavigationRole.Where(s => s.RoleId == roleId).Select(s => s.NavigationId).ToList();
        }

        public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
        {
            using var dbcontext = DbFactory.CreateDbContext();
            var currentrole = dbcontext.Roles.Include(s => s.Navigations).Where(s => s.Id == roleId).FirstOrDefault();
            if (currentrole != null)
            {
                currentrole.Navigations = dbcontext.Navigations.Where(s => menuIds.Contains(s.Id!)).ToList();
                return dbcontext.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
