using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class App : DataAccess.App
    {
        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.App> RetrievesByRoleId(string roleId)
        {
            var apps = DictHelper.RetrieveApps().Where(d => d.Key.CompareTo("0") > 0).Select(d => new DataAccess.App()
            {
                Id = d.Key,
                AppName = d.Value
            }).ToList();
            var role = RoleHelper.Retrieves().Cast<Role>().FirstOrDefault(r => r.Id == roleId) ?? new Role();
            apps.ForEach(p => p.Checked = (role.Apps.Contains(p.Id) || role.RoleName.Equals("Administrators", StringComparison.OrdinalIgnoreCase)) ? "checked" : "");
            return apps;
        }

        /// <summary>
        /// 根据角色ID以及选定的App ID，保到角色应用表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="appIds"></param>
        /// <returns></returns>
        public override bool SaveByRoleId(string roleId, IEnumerable<string> appIds)
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(nameof(roleId));

            if (appIds == null) appIds = new string[0];
            var ret = DbManager.Roles.UpdateOne(md => md.Id == roleId, Builders<Role>.Update.Set(md => md.Apps, appIds));
            return true;
        }

        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUserName(string userName)
        {
            var ret = new List<string>();
            var roles = RoleHelper.RetrievesByUserName(userName);
            if (roles.Contains("Administrators", StringComparer.OrdinalIgnoreCase))
            {
                ret.AddRange(DictHelper.RetrieveApps().Select(kv => kv.Key));
            }
            else
            {
                RoleHelper.Retrieves().Cast<Role>().Where(r => roles.Any(rn => rn == r.RoleName)).ToList().ForEach(r => ret.AddRange(r.Apps));
            }
            return ret;
        }
    }
}
