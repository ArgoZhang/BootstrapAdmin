using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 前台应用查询类
    /// </summary>
    public class QueryAppOption
    {
        /// <summary>
        /// 应用操作 new 为新建 edit 为保存
        /// </summary>
        public string AppId { get; set; } = "edit";

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; } = "";

        /// <summary>
        /// 应用编码
        /// </summary>
        public string AppCode { get; set; } = "";

        /// <summary>
        /// 前台应用路径
        /// </summary>
        public string AppUrl { get; set; } = "#";

        /// <summary>
        /// 前台应用标题
        /// </summary>
        public string AppTitle { get; set; } = "未设置";

        /// <summary>
        /// 前台应用页脚
        /// </summary>
        public string AppFooter { get; set; } = "未设置";

        /// <summary>
        /// 前台应用图标
        /// </summary>
        public string AppIcon { get; set; } = "/favicon.ico";

        /// <summary>
        /// 前台应用收藏图标
        /// </summary>
        public string AppFavicon { get; set; } = "/favicon.png";

        /// <summary>
        /// 保存前台应用方法
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            var ret = DictHelper.SaveAppSettings(this);
            return true;
        }

        /// <summary>
        /// 通过指定 AppKey 获取前台应用配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static QueryAppOption RetrieveByKey(string key)
        {
            var ret = new QueryAppOption() { AppCode = key };
            var dicts = DictHelper.RetrieveDicts();
            ret.AppName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == key)?.Name ?? "";
            ret.AppUrl = dicts.FirstOrDefault(d => d.Category == "应用首页" && d.Name == key)?.Code ?? "";
            ret.AppTitle = dicts.FirstOrDefault(d => d.Category == ret.AppName && d.Name == "网站标题")?.Code ?? "";
            ret.AppFooter = dicts.FirstOrDefault(d => d.Category == ret.AppName && d.Name == "网站页脚")?.Code ?? "";
            ret.AppFavicon = dicts.FirstOrDefault(d => d.Category == ret.AppName && d.Name == "网站图标")?.Code ?? "";
            ret.AppIcon = dicts.FirstOrDefault(d => d.Category == ret.AppName && d.Name == "favicon")?.Code ?? "";
            return ret;
        }
    }
}
