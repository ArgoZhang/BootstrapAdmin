using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.ExceptionManagement;
using Longbow.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户表相关操作类
    /// </summary>
    public static class UserHelper
    {
        private const string UserDataKey = "UserData-CodeUserHelper";
        private const string UserDisplayNameDataKey = "UserData-CodeUserHelper-";
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="pIds"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsers(string tId = null)
        {
            string sql = "select ID, UserName, DisplayName from Users";
            var ret = CacheManager.GetOrAdd(UserDataKey, CacheSection.RetrieveIntervalByKey(UserDataKey), key =>
            {
                List<User> Users = new List<User>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Users.Add(new User()
                            {
                                ID = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Users;
            }, CacheSection.RetrieveDescByKey(UserDataKey));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.ID.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static User RetrieveUsersByName(string userName)
        {
            string key = string.Format("{0}{1}", UserDisplayNameDataKey, userName);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(UserDisplayNameDataKey), k =>
            {
                User user = null;
                string sql = "select ID, UserName, [Password], PassSalt, DisplayName from Users where UserName = @UserName";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", userName, ParameterDirection.Input));
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            user = new User()
                            {
                                ID = (int)reader[0],
                                UserName = (string)reader[1],
                                Password = (string)reader[2],
                                PassSalt = (string)reader[3],
                                DisplayName = (string)reader[4]
                            };
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return user;
            }, CacheSection.RetrieveDescByKey(UserDisplayNameDataKey));
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteUser(string ids)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
            try
            {
                string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Users where ID in ({0})", ids);
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
        /// 保存新建/更新的用户信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveUser(User p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            if (p.UserName.Length > 50) p.UserName.Substring(0, 50);
            p.PassSalt = LgbCryptography.GenerateSalt();
            p.Password = LgbCryptography.ComputeHash(p.Password, p.PassSalt);
            if (p.DisplayName.Length > 50) p.DisplayName.Substring(0, 50);
            string sql = p.ID == 0 ?
                "Insert Into Users (UserName, Password, PassSalt, DisplayName) Values (@UserName, @Password, @PassSalt, @DisplayName)" :
                "Update Users set UserName = @UserName, Password = @Password, PassSalt = @PassSalt, DisplayName = @DisplayName where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", p.UserName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Password", p.Password, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@PassSalt", p.PassSalt, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DisplayName", p.DisplayName, ParameterDirection.Input));
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
        /// <summary>
        /// 验证用户登陆账号与密码正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Authenticate(string userName, string password)
        {
            var user = RetrieveUsersByName(userName);
            return user != null && user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }
        // 更新缓存
        private static void ClearCache()
        {
            CacheManager.Clear(key => key == UserDataKey);
            CacheManager.Clear(key => key.Contains(UserDisplayNameDataKey));
        }
    }
}
