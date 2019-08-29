using Bootstrap.Security;
using Longbow.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Dict : DataAccess.Dict
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<BootstrapDict> RetrieveDicts() => DbManager.Dicts.Find(FilterDefinition<BootstrapDict>.Empty).ToList();

        private static string RetrieveAppName(string name, string defaultValue = "未设置")
        {
            var dicts = DictHelper.RetrieveDicts();
            var platName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == ConfigurationManager.AppSettings["AppId"])?.Name;
            return dicts.FirstOrDefault(d => d.Category == platName && d.Name == name)?.Code ?? $"{name}{defaultValue}";
        }

        /// <summary>
        /// 获得系统设置地址
        /// </summary>
        /// <returns></returns>
        public override string RetrieveSettingsUrl() => RetrieveAppName("系统设置地址");

        /// <summary>
        /// 获得系统个人中心地址
        /// </summary>
        /// <returns></returns>
        public override string RetrieveProfilesUrl() => RetrieveAppName("个人中心地址");

        /// <summary>
        /// 获得系统通知地址地址
        /// </summary>
        /// <returns></returns>
        public override string RetrieveNotisUrl() => RetrieveAppName("系统通知地址");
    }
}
