using Bootstrap.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User : DataAccess.User
    {
        /// <summary>
        /// 获得/设置 用户主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获得/设置 用户被批复时间
        /// </summary>
        public DateTime? ApprovedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Groups { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override BootstrapUser RetrieveUserByUserName(string userName)
        {
            var project = Builders<User>.Projection.Include(u => u.Id)
               .Include(u => u.UserName)
               .Include(u => u.DisplayName)
               .Include(u => u.Icon)
               .Include(u => u.Css)
               .Include(u => u.App);
            var ret = DbManager.Users.Find(user => user.UserName.ToLowerInvariant() == userName.ToLowerInvariant()).Project<User>(project).FirstOrDefault();
            if (ret != null)
            {
                if (string.IsNullOrEmpty(ret.Icon)) ret.Icon = "default.jpg";
                if (string.IsNullOrEmpty(ret.App)) ret.App = "0";
            }
            return ret;
        }
    }
}
