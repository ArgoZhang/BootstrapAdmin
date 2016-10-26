using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public static class MenuHelper
    {
        private const string MenuDataKey = "MenuData-CodeMenuHelper";
        /// <summary>
        /// 查询所有菜单信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveMenus(string tId = null)
        {
            string sql = "select * from Navigations";
            var ret = CacheManager.GetOrAdd(MenuDataKey, CacheSection.RetrieveIntervalByKey(MenuDataKey), key =>
            {
                List<Menu> Menus = new List<Menu>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Menus.Add(new Menu()
                            {
                                ID = (int)reader[0],
                                ParentId = (int)reader[1],
                                Name = (string)reader[2],
                                Order = (int)reader[3],
                                Icon = LgbConvert.ReadValue(reader[4], string.Empty),
                                Url = LgbConvert.ReadValue(reader[5], string.Empty),
                                Category = (int)reader[6]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Menus;
            }, CacheSection.RetrieveDescByKey(MenuDataKey));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.ID.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveMenusByUserId(int userId)
        {
            //UNDONE: 通过用户ID获得到当前用户配置的菜单
            return RetrieveMenus();
        }
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteMenu(string ids)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
            try
            {
                string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Navigations where ID in ({0})", ids);
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ClearCache();
                ret = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的菜单信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveMenu(Menu p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            if (string.IsNullOrEmpty(p.Name)) return ret;
            if (p.Name.Length > 50) p.Name.Substring(0, 50);
            if (p.Icon != null && p.Icon.Length > 50) p.Icon.Substring(0, 50);
            if (p.Url != null && p.Url.Length > 50) p.Url.Substring(0, 50);
            string sql = p.ID == 0 ?
                "Insert Into Navigations (ParentId, Name, [Order], Icon, Url, Category) Values (@ParentId, @Name, @Order, @Icon, @Url, @Category)" :
                "Update Navigations set ParentId = @ParentId, Name = @Name, [Order] = @Order, Icon = @Icon, Url = @Url, Category = @Category where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ParentId", p.ParentId, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", p.Name, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Order", p.Order, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Icon", DBAccess.ToDBValue(p.Icon), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Url", DBAccess.ToDBValue(p.Url), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", p.Category, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;
                ClearCache();
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        // 更新缓存
        private static void ClearCache()
        {
            CacheManager.Clear(key => key == MenuDataKey);
        }
    }
}
