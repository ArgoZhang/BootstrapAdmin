﻿using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.Extensions.Configuration;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services
{
    class DictsService : BaseDatabase, IDicts
    {
        private string AppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="configuration"></param>
        public DictsService(IDatabase db, IConfiguration configuration)
        {
            Database = db;
            AppId = configuration.GetValue("AppId", "BA");
        }

        private List<Dict> GetAll() => Database.Fetch<Dict>();

        /// <summary>
        /// 获取 站点 Title 配置信息
        /// </summary>
        /// <returns></returns>
        public string GetWebTitle()
        {
            var dicts = GetAll();
            var title = "网站标题";
            var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                title = dicts.First(d => d.Category == name && d.Name == "网站标题")?.Code ?? "网站标题";
            }
            return title;
        }

        /// <summary>
        /// 获取站点 Footer 配置信息
        /// </summary>
        /// <returns></returns>
        public string GetWebFooter()
        {
            var dicts = GetAll();
            var title = "网站页脚";
            var name = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == AppId)?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                title = dicts.First(d => d.Category == name && d.Name == "网站页脚")?.Code ?? "网站页脚";
            }
            return title;
        }
    }
}
