using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services
{
    class UsersService : BaseDatabase, IUsers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public UsersService(IDatabase db) => Database = db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> GetApps(string userName) => Database.Fetch<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {Database.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0) r on ra.RoleId = r.ID union select Code from Dicts where Category = @1 and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 and r.RoleName = @2 union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {Database.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0 and r.RoleName = @2)", userName, "应用程序", "Administrators");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> GetRoles(string userName) => Database.Fetch<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @0 union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {Database.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName=@0", userName);
    }
}
