using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Delete(IEnumerable<string> value)
        {
            var list = new List<WriteModel<BootstrapDict>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<BootstrapDict>(Builders<BootstrapDict>.Filter.Eq(md => md.Id, id)));
            }
            DbManager.Dicts.BulkWrite(list);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(BootstrapDict p)
        {
            if (string.IsNullOrEmpty(p.Id))
            {
                p.Id = null;
                DbManager.Dicts.InsertOne(p);
                p.Id = DbManager.Dicts.Find(d => d.Name == p.Name && d.Category == p.Category && d.Define == p.Define && d.Code == p.Code).FirstOrDefault().Id;
            }
            else
            {
                DbManager.Dicts.UpdateOne(md => md.Id == p.Id, Builders<BootstrapDict>.Update.Set(md => md.Category, p.Category)
                    .Set(md => md.Define, p.Define)
                    .Set(md => md.Name, p.Name)
                    .Set(md => md.Code, p.Code));
            }
            return true;
        }

        /// <summary>
        /// 保存网站设置方法
        /// </summary>
        /// <param name="dicts"></param>
        /// <returns></returns>
        public override bool SaveSettings(IEnumerable<BootstrapDict> dicts)
        {
            dicts.ToList().ForEach(dict => DbManager.Dicts.FindOneAndUpdate(md => md.Category == dict.Category && md.Name == dict.Name, Builders<BootstrapDict>.Update.Set(md => md.Code, dict.Code)));
            return true;
        }

        private static string RetrieveAppName(string name, string appId = "", string defaultValue = "未设置")
        {
            var dicts = DictHelper.RetrieveDicts();
            var platName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId)?.Name;
            return dicts.FirstOrDefault(d => d.Category == platName && d.Name == name)?.Code ?? $"{name}{defaultValue}";
        }

        /// <summary>
        /// 获得网站标题设置
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public override string RetrieveWebTitle(string appId)
        {
            var code = RetrieveAppName("网站标题", appId);
            if (code == "网站标题未设置") code = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站标题" && d.Category == "网站设置" && d.Define == 0)?.Code ?? "后台管理系统";
            return code;
        }

        /// <summary>
        /// 获得网站页脚设置
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public override string RetrieveWebFooter(string appId)
        {
            var code = RetrieveAppName("网站页脚", appId);
            if (code == "网站页脚未设置") code = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站页脚" && d.Category == "网站设置" && d.Define == 0)?.Code ?? "2016 © 通用后台管理系统";
            return code;
        }
    }
}
