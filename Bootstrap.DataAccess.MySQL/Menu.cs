using Bootstrap.Security;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.MySQL
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu : DataAccess.Menu
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName)
        {
            var menus = new List<BootstrapMenu>();
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, "select n.ID, n.ParentId, n.Name, n.Order, n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.Application, d.Name as CategoryName, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = @Category and d.Define = 0 left join Navigations ln on n.ParentId = ln.ID inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserName", userName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Category", "菜单"));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleName", "Administrators"));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        menus.Add(new BootstrapMenu
                        {
                            Id = reader[0].ToString(),
                            ParentId = reader[1].ToString(),
                            Name = (string)reader[2],
                            Order = reader.IsDBNull(3) ? 0 : (int)reader[3],
                            Icon = reader.IsDBNull(4) ? string.Empty : (string)reader[4],
                            Url = reader.IsDBNull(5) ? string.Empty : (string)reader[5],
                            Category = (string)reader[6],
                            Target = (string)reader[7],
                            IsResource = reader.IsDBNull(8) ? 0 : (int)reader[8] > 0 ? 1 : 0,
                            ApplicationCode = reader.IsDBNull(9) ? string.Empty : (string)reader[9],
                            CategoryName = (string)reader[10],
                            ParentName = reader.IsDBNull(11) ? string.Empty : (string)reader[11],
                        });
                    }
                }
            }
            return menus;
        }

        /// <summary>
        /// Saves the menu.
        /// </summary>
        /// <returns><c>true</c>, if menu was saved, <c>false</c> otherwise.</returns>
        /// <param name="p">P.</param>
        public override bool SaveMenu(BootstrapMenu p)
        {
            if (string.IsNullOrEmpty(p.Name)) return false;
            bool ret = false;
            if (p.Name.Length > 50) p.Name = p.Name.Substring(0, 50);
            if (p.Icon != null && p.Icon.Length > 50) p.Icon = p.Icon.Substring(0, 50);
            if (p.Url != null && p.Url.Length > 4000) p.Url = p.Url.Substring(0, 4000);
            string sql = string.IsNullOrEmpty(p.Id) ?
                "Insert Into Navigations (ParentId, Name, `Order`, Icon, Url, Category, Target, IsResource, Application) Values (@ParentId, @Name, @Order, @Icon, @Url, @Category, @Target, @IsResource, @ApplicationCode)" :
                "Update Navigations set ParentId = @ParentId, Name = @Name, `Order` = @Order, Icon = @Icon, Url = @Url, Category = @Category, Target = @Target, IsResource = @IsResource, Application = @ApplicationCode where ID = @ID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ParentId", p.ParentId));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Name", p.Name));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Order", p.Order));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Icon", DbAdapterManager.ToDBValue(p.Icon)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Url", DbAdapterManager.ToDBValue(p.Url)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Category", p.Category));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Target", p.Target));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@IsResource", p.IsResource));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ApplicationCode", p.ApplicationCode));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            return ret;
        }

        /// <summary>
        /// <summary>
        /// 通过角色ID保存当前授权菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public override bool SaveMenusByRoleId(string roleId, IEnumerable<string> menuIds)
        {
            bool ret = false;
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除菜单角色表该角色所有的菜单
                    string sql = $"delete from NavigationRole where RoleID = {roleId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入菜单角色表
                        menuIds.ToList().ForEach(mId =>
                        {
                            cmd.CommandText = $"Insert Into NavigationRole (NavigationID, RoleID) Values ( {mId}, {roleId})";
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        });
                        transaction.CommitTransaction();
                    }
                    ret = true;
                }
                catch (Exception ex)
                {
                    transaction.RollbackTransaction();
                    throw ex;
                }
            }
            return ret;
        }
    }
}
