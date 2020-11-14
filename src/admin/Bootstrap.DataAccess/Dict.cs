using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 字典表实体类
    /// </summary>
    [TableName("Dicts")]
    public class Dict : BootstrapDict
    {
        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="value">需要删除的IDs</param>
        /// <returns></returns>
        public virtual bool Delete(IEnumerable<string> value)
        {
            if (!value.Any()) return true;
            var ids = string.Join(",", value);
            string sql = $"where ID in ({ids})";
            using var db = DbManager.Create();
            db.Delete<BootstrapDict>(sql);
            return true;
        }

        /// <summary>
        /// 保存新建/更新的字典信息
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public virtual bool Save(BootstrapDict dict)
        {
            if (dict.Category.Length > 50) dict.Category = dict.Category.Substring(0, 50);
            if (dict.Name.Length > 50) dict.Name = dict.Name.Substring(0, 50);
            if (dict.Code.Length > 2000) dict.Code = dict.Code.Substring(0, 2000);

            using var db = DbManager.Create();
            db.Save(dict);
            return true;
        }

        /// <summary>
        /// 保存网站个性化设置
        /// </summary>
        /// <param name="dicts"></param>
        /// <returns></returns>
        public virtual bool SaveSettings(IEnumerable<BootstrapDict> dicts)
        {
            using var db = DbManager.Create();
            dicts.ToList().ForEach(dict => db.Update<Dict>("set Code = @Code where Category = @Category and Name = @Name", dict));
            return true;
        }

        /// <summary>
        /// 获取字典分类名称
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveCategories() => DictHelper.RetrieveDicts().OrderBy(d => d.Category).Select(d => d.Category).Distinct();

        /// <summary>
        /// 获取系统网站标题
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public virtual string RetrieveWebTitle(string appId)
        {
            // 优先查找配置的应用程序网站标题
            var code = DbHelper.RetrieveTitle(appId);
            if (code == "网站标题未设置") code = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站标题" && d.Category == "网站设置" && d.Define == 0)?.Code ?? "后台管理系统";
            return code;
        }

        /// <summary>
        /// 获取系统网站页脚
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public virtual string RetrieveWebFooter(string appId)
        {
            // 优先查找配置的应用程序网站标题
            var code = DbHelper.RetrieveFooter(appId);
            if (code == "网站页脚未设置") code = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站页脚" && d.Category == "网站设置" && d.Define == 0)?.Code ?? "2016 © 通用后台管理系统";
            return code;
        }

        /// <summary>
        /// 获得系统中配置的可以使用的网站样式
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveThemes() => DictHelper.RetrieveDicts().Where(d => d.Category == "网站样式");

        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveActiveTheme()
        {
            var theme = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "使用样式" && d.Category == "网站设置" && d.Define == 0);
            return theme == null ? string.Empty : (theme.Code.Equals("site.css", StringComparison.OrdinalIgnoreCase) ? string.Empty : theme.Code);
        }

        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public virtual string? RetrieveIconFolderPath() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == 0)?.Code;

        /// <summary>
        /// 获得默认的前台首页地址，默认为~/Home/Index
        /// </summary>
        /// <param name="userName">登录用户名</param>
        /// <param name="appId">默认应用程序编码</param>
        /// <returns></returns>
        public virtual string RetrieveHomeUrl(string? userName, string appId)
        {
            // https://gitee.com/LongbowEnterprise/dashboard/issues?id=IS0WK
            // https://gitee.com/LongbowEnterprise/dashboard/issues?id=I17SD0
            var url = "~/Home/Index";
            var dicts = DictHelper.RetrieveDicts();

            if (appId.IsNullOrEmpty())
            {
                var defaultUrl = dicts.FirstOrDefault(d => d.Name == "前台首页" && d.Category == "网站设置" && d.Define == 0)?.Code;
                if (!string.IsNullOrEmpty(defaultUrl)) url = defaultUrl;
            }
            else if (appId.Equals("BA", StringComparison.OrdinalIgnoreCase))
            {
                // 使用配置项设置是否启用默认第一个应用是默认应用
                var defaultApp = (dicts.FirstOrDefault(d => d.Name == "默认应用程序" && d.Category == "网站设置" && d.Define == 0)?.Code ?? "0") == "1";
                if (defaultApp)
                {
                    var app = AppHelper.RetrievesByUserName(userName).FirstOrDefault(key => !key.Equals("BA", StringComparison.OrdinalIgnoreCase)) ?? "";
                    if (!string.IsNullOrEmpty(app))
                    {
                        // 指定应用程序的首页
                        var appUrl = RetrieveDefaultHomeUrlByApp(dicts, app);
                        if (!string.IsNullOrEmpty(appUrl)) url = appUrl;
                    }
                }
            }
            else
            {
                // 指定应用程序的首页
                var appUrl = RetrieveDefaultHomeUrlByApp(dicts, appId);
                if (!string.IsNullOrEmpty(appUrl)) url = appUrl;
            }
            return url;
        }

        /// <summary>
        /// 通过 appId 获取应用首页配置值
        /// </summary>
        /// <param name="dicts"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        protected virtual string RetrieveDefaultHomeUrlByApp(IEnumerable<BootstrapDict> dicts, string appId)
        {
            return dicts.FirstOrDefault(d => d.Name.Equals(appId, StringComparison.OrdinalIgnoreCase) && d.Category == "应用首页" && d.Define == 0)?.Code ?? "";
        }

        /// <summary>
        /// 获得字典表中配置的所有应用程序
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<string, string>> RetrieveApps() => DictHelper.RetrieveDicts().Where(d => d.Category == "应用程序" && d.Define == 0).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(d => d.Key);

        /// <summary>
        /// 通过数据库获得所有字典表配置信息，缓存Key=DictHelper-RetrieveDicts
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveDicts() => DbHelper.RetrieveDicts();

        /// <summary>
        /// 程序异常时长 默认1月
        /// </summary>
        /// <returns></returns>
        public int RetrieveExceptionsLogPeriod() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "程序异常保留时长" && d.Define == 0)?.Code, 1);

        /// <summary>
        /// 操作日志时长 默认12月
        /// </summary>
        /// <returns></returns>
        public int RetrieveLogsPeriod() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "操作日志保留时长" && d.Define == 0)?.Code, 12);

        /// <summary>
        /// 登录日志时长 默认12月
        /// </summary>
        /// <returns></returns>
        public int RetrieveLoginLogsPeriod() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "登录日志保留时长" && d.Define == 0)?.Code, 12);

        /// <summary>
        /// Cookie保存时长 默认7天
        /// </summary>
        /// <returns></returns>
        public int RetrieveCookieExpiresPeriod() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "Cookie保留时长" && d.Define == 0)?.Code, 7);

        /// <summary>
        /// 获得 IP地理位置
        /// </summary>
        /// <returns></returns>
        public string RetrieveLocaleIPSvr() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "IP地理位置接口" && d.Define == 0)?.Code ?? string.Empty;

        /// <summary>
        /// 获得 IP请求缓存时长配置值
        /// </summary>
        /// <returns></returns>
        public int RetrieveLocaleIPSvrCachePeriod()
        {
            var period = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "IP请求缓存时长" && d.Define == 0)?.Code;
            var ret = 10;
            if (!string.IsNullOrEmpty(period) && int.TryParse(period, out var svrPeriod)) ret = svrPeriod;
            return ret;
        }

        /// <summary>
        /// 获得 项目是否获取登录地点 默认为false
        /// </summary>
        /// <param name="ipSvr">服务提供名称</param>
        /// <returns></returns>
        public string? RetrieveLocaleIPSvrUrl(string ipSvr) => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "地理位置" && d.Name == ipSvr && d.Define == 0)?.Code;

        /// <summary>
        /// 获得 访问日志保留时长 默认为1个月
        /// </summary>
        /// <returns></returns>
        public int RetrieveAccessLogPeriod() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "访问日志保留时长" && d.Define == 0)?.Code, 1);

        /// <summary>
        /// 获得 是否为演示系统 默认为 false 不是演示系统
        /// </summary>
        /// <returns></returns>
        public bool RetrieveSystemModel() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "演示系统" && d.Define == 0)?.Code, "0") == "1";

        /// <summary>
        /// 获得 是否为演示系统 默认为 false 不是演示系统
        /// </summary>
        /// <returns></returns>
        public bool UpdateSystemModel(bool isDemo)
        {
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "演示系统" && d.Define == 0);
            dict!.Code = isDemo ? "1" : "0";
            return Save(dict);
        }

        /// <summary>
        /// 获得 验证码图床地址
        /// </summary>
        /// <returns></returns>
        public string RetrieveImagesLibUrl() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "验证码图床" && d.Define == 0)?.Code ?? "http://images.sdgxgz.com/";

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
        /// 获得是否允许短信验证码登录
        /// </summary>
        /// <returns></returns>
        public bool RetrieveMobileLogin() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "短信验证码登录" && d.Define == 0)?.Code ?? "1") == "1";

        /// <summary>
        /// 获得是否允许 OAuth 认证登录
        /// </summary>
        /// <returns></returns>
        public bool RetrieveOAuthLogin() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "OAuth 认证登录" && d.Define == 0)?.Code ?? "1") == "1";

        /// <summary>
        /// 获得自动锁屏时长 默认 30 秒
        /// </summary>
        /// <returns></returns>
        public int RetrieveAutoLockScreenPeriod() => LgbConvert.ReadValue(DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "自动锁屏时长" && d.Define == 0)?.Code, 30);

        /// <summary>
        /// 获得自动锁屏是否开启 默认关闭
        /// </summary>
        /// <returns></returns>
        public bool RetrieveAutoLockScreen() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "自动锁屏" && d.Define == 0)?.Code ?? "0") == "1";

        /// <summary>
        /// 获得默认应用是否开启 默认关闭
        /// </summary>
        /// <returns></returns>
        public bool RetrieveDefaultApp() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "默认应用程序" && d.Define == 0)?.Code ?? "0") == "1";

        /// <summary>
        /// 获得是否开启 Blazor 功能 默认关闭
        /// </summary>
        /// <returns></returns>
        public bool RetrieveEnableBlazor() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "Blazor" && d.Define == 0)?.Code ?? "0") == "1";

        /// <summary>
        /// 获得是否开启 固定表头 功能 默认开启
        /// </summary>
        /// <returns></returns>
        public bool RetrieveFixedTableHeader() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "固定表头" && d.Define == 0)?.Code ?? "1") == "1";

        /// <summary>
        /// 获得字典表地理位置配置信息集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BootstrapDict> RetireveLocators() => DictHelper.RetrieveDicts().Where(d => d.Category == "地理位置服务");

        /// <summary>
        /// 获得个人中心地址
        /// </summary>
        /// <returns></returns>
        public string RetrievePathBase() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "后台地址" && d.Define == 0)?.Code ?? "";

        /// <summary>
        /// 获得字典表健康检查是否开启
        /// </summary>
        /// <returns></returns>
        public bool RetrieveHealth() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "健康检查" && d.Define == 0)?.Code ?? "0") == "1";

        /// <summary>
        /// 获得字典表登录界面数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BootstrapDict> RetrieveLogins() => DictHelper.RetrieveDicts().Where(d => d.Category == "系统首页" && d.Define == 1);

        /// <summary>
        /// 获得使用中的登录视图名称
        /// </summary>
        /// <returns></returns>
        public string? RetrieveLoginView() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "登录界面" && d.Define == 1)?.Code;
    }
}
