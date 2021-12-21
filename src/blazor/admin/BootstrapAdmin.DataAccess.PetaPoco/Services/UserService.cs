using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services
{
    class UserService : BaseDatabase, IUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public UserService(IDatabase db) => Database = db;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll() => Database.Fetch<User>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Authenticate(string userName, string password)
        {
            var user = Database.SingleOrDefault<User>("select DisplayName, Password, PassSalt from Users where ApprovedTime is not null and UserName = @0", userName);

            var isAuth = false;
            if (user != null && !string.IsNullOrEmpty(user.PassSalt))
            {
                isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
            }
            return isAuth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string? GetDisplayName(string? userName) => string.IsNullOrEmpty(userName) ? "" : Database.ExecuteScalar<string>("select DisplayName from Users where UserName = @0", userName);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> GetUsersByGroupId(string? id) => Database.Fetch<string>("select UserID from UserGroup where GroupID = @0", id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool SaveUsersByGroupId(string? id, IEnumerable<string> userIds)
        {
            var ret = false;
            try
            {
                Database.BeginTransaction();
                Database.Execute("delete from UserGroup where GroupId = @0", id);
                Database.InsertBatch("UserGroup", userIds.Select(g => new { UserID = g, GroupID = id }));
                Database.CompleteTransaction();
                ret = true;
            }
            catch (Exception)
            {
                Database.AbortTransaction();
                throw;
            }
            return ret;
        }
    }
}
