using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 字典表实体类
    /// </summary>
    public class Dict : BootstrapDict
    {
        /// <summary>
        /// 获取系统网站标题
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveWebTitle() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站标题" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "后台管理系统" }).Code;

        /// <summary>
        /// 获取系统网站页脚
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveWebFooter() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站页脚" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "2016 © 通用后台管理系统" }).Code;

        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveActiveTheme()
        {
            var theme = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "使用样式" && d.Category == "当前样式" && d.Define == 0);
            return theme == null ? string.Empty : (theme.Code.Equals("site.css", StringComparison.OrdinalIgnoreCase) ? string.Empty : theme.Code);
        }

        /// <summary>
        /// 通过数据库获得所有字典表配置信息，缓存Key=DictHelper-RetrieveDicts
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveDicts() => DbHelper.RetrieveDicts();

        /// <summary>
        /// 获得 IP地理位置
        /// </summary>
        /// <returns></returns>
        public string RetrieveLocaleIPSvr() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "系统设置" && d.Name == "IP地理位置接口" && d.Define == 0)?.Code;

        /// <summary>
        /// 获取 IP 地理位置查询服务缓存时长
        /// </summary>
        /// <returns></returns>
        public string RetrieveLocaleIPSvrCachePeriod() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "系统设置" && d.Name == "IP请求缓存时长" && d.Define == 0)?.Code;

        /// <summary>
        /// 获得 项目是否获取登录地点 默认为 false
        /// </summary>
        /// <param name="ipSvr">服务提供名称</param>
        /// <returns></returns>
        public string RetrieveLocaleIPSvrUrl(string ipSvr) => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "系统设置" && d.Name == ipSvr && d.Define == 0)?.Code;

        /// <summary>
        /// 获得 是否为演示系统 默认为 false 不是演示系统
        /// </summary>
        /// <returns></returns>
        public bool RetrieveSystemModel() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "系统设置" && d.Name == "演示系统" && d.Define == 0)?.Code, "0") == "1";

        /// <summary>
        /// 获得 验证码图床地址
        /// </summary>
        /// <returns></returns>
        public string RetrieveImagesLibUrl() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "系统设置" && d.Name == "验证码图床" && d.Define == 0)?.Code ?? "http://images.sdgxgz.com/";

        /// <summary>
        /// 获得 数据库标题是否显示
        /// </summary>
        /// <returns></returns>
        public bool RetrieveCardTitleStatus() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "卡片标题状态" && d.Define == 0)?.Code ?? "1") == "1";

        /// <summary>
        /// 获得 是否显示侧边栏 为真时显示
        /// </summary>
        /// <returns></returns>
        public bool RetrieveSidebarStatus() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "侧边栏状态" && d.Define == 0)?.Code ?? "1") == "1";

        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveIconFolderPath() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == 0) ?? new BootstrapDict { Code = "~/images/uploader/" }).Code;

        /// <summary>
        /// 获得系统设置地址
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveSettingsUrl() => DbHelper.RetrieveSettingsUrl();

        /// <summary>
        /// 获得系统个人中心地址
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveProfilesUrl() => DbHelper.RetrieveProfilesUrl();

        /// <summary>
        /// 获得系统通知地址地址
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveNotisUrl() => DbHelper.RetrieveNotisUrl();
    }
}
