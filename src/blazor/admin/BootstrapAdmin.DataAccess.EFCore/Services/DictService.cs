using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Longbow.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

class DictService : IDict
{
        private IDbContextFactory<BootstrapAdminContext> DbFactory { get; set; }

        private string AppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="configuration"></param>
        public DictService(IDbContextFactory<BootstrapAdminContext> factory, IConfiguration configuration)
        {
            DbFactory = factory;
            AppId = configuration.GetValue("AppId", "BA");
        }

        private List<Dict> GetAll()
        {
            using var context = DbFactory.CreateDbContext();
            return context.Dicts.ToList();
        }

        public Dictionary<string, string> GetApps()
        {
            var dicts = GetAll();
            return dicts.Where(d => d.Category == "应用程序").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
        }

        public Dictionary<string, string> GetLogins()
        {
            var dicts = GetAll();
            return dicts.Where(d => d.Category == "系统首页").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
        }

        public Dictionary<string, string> GetThemes()
        {
            var dicts = GetAll();
            return dicts.Where(d => d.Category == "网站样式").Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
        }

        public string GetWebFooter()
        {
            var dicts = GetAll();
            var title = "网站页脚";
            var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站页脚") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站页脚");
                title = dict?.Code ?? "网站标题";
            }
            return title;
        }

        public string GetWebTitle()
        {
            var dicts = GetAll();
            var title = "网站标题";
            var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == "网站标题") ?? dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "网站标题");
                title = dict?.Code ?? "网站标题";
            }
            return title;
        }

        public bool IsDemo()
        {
            var dicts = GetAll();
            var code = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "演示系统" && d.Define == EnumDictDefine.System)?.Code ?? "0";
            return code == "1";
        }

        public bool SaveDemo(bool isDemo)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateDemo(string code)
        {
            var ret = false;
            if (!string.IsNullOrEmpty(code))
            {
                var dicts = GetAll();
                var salt = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "授权盐值" && d.Define == EnumDictDefine.System)?.Code;
                var authCode = dicts.FirstOrDefault(d => d.Category == "网站设置" && d.Name == "哈希结果" && d.Define == EnumDictDefine.System)?.Code;
                if (!string.IsNullOrEmpty(salt))
                {
                    ret = LgbCryptography.ComputeHash(code, salt) == authCode;
                }
            }
            return ret;
        }

        public bool SaveHealthCheck(bool enable = true)
        {
            return true;
        }
    }
