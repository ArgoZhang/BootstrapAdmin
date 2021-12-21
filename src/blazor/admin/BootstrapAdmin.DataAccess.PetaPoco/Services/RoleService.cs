using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services
{
    class RoleService : BaseDatabase, IRole
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public RoleService(IDatabase db) => Database = db;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Role> GetAll() => Database.Fetch<Role>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> GetUsersByRoleId(string? id) => Database.Fetch<string>("select UserID from UserRole where RoleID = @0", id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool SaveUsersByRoleId(string? id, IEnumerable<string> userIds)
        {
            var ret = false;
            try
            {
                Database.BeginTransaction();
                Database.Execute("delete from UserRole where RoleID = @0", id);
                Database.InsertBatch("UserRole", userIds.Select(g => new { UserID = g, RoleID = id }));
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
