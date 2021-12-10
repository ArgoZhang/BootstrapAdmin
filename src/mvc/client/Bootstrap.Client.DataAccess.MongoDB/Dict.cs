using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 字典表实体类
    /// </summary>
    internal class Dict : DataAccess.Dict
    {
        /// <summary>
        /// 获取所有字典表数据方法
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<BootstrapDict> RetrieveDicts() => DbManager.Dicts.Find(FilterDefinition<BootstrapDict>.Empty).ToList();

        /// <summary>
        /// 获取应用程序配置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appId"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string RetrieveAppName(string name, string appId = "", string defaultValue = "未设置")
        {
            var dicts = DictHelper.RetrieveDicts();
            var platName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId)?.Name;
            return dicts.FirstOrDefault(d => d.Category == platName && d.Name == name)?.Code ?? $"{name}{defaultValue}";
        }

        /// <summary>
        /// 获取系统网站标题
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public override string RetrieveWebTitle(string appId) => RetrieveAppName("网站标题", appId);

        /// <summary>
        /// 获取系统网站页脚
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public override string RetrieveWebFooter(string appId) => RetrieveAppName("网站页脚", appId);

        /// <summary>
        /// 获得系统设置地址
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public override string RetrieveSettingsUrl(string appId) => RetrieveAppName("系统设置地址", appId);

        /// <summary>
        /// 获得系统个人中心地址
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public override string RetrieveProfilesUrl(string appId) => RetrieveAppName("个人中心地址", appId);

        /// <summary>
        /// 获得系统通知地址地址
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public override string RetrieveNotisUrl(string appId) => RetrieveAppName("系统通知地址", appId);
    }
}
