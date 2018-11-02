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
        /// 删除菜单信息
        /// </summary>
        /// <param name="value"></param>
        public override bool DeleteMenu(IEnumerable<string> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, $"delete from NavigationRole where NavigationID in ({ids})"))
                {
                    try
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from Navigations where ID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        transaction.CommitTransaction();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackTransaction();
                        throw ex;
                    }
                }
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
