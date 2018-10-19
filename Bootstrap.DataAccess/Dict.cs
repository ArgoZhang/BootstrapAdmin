using Bootstrap.Security;
using Longbow;
using Longbow.Cache;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Dict : BootstrapDict
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveCategoryDataKey = "DictHelper-RetrieveDictsCategory";
        /// <summary>
        /// 缓存索引，BootstrapAdmin后台清理缓存时使用
        /// </summary>
        public const string RetrieveDictsDataKey = "BootstrapDict-RetrieveDicts";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<string, string>> RetrieveApps() => throw new NotImplementedException();
        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="value">需要删除的IDs</param>
        /// <returns></returns>
        public virtual bool DeleteDict(IEnumerable<int> value) => throw new NotImplementedException();
        /// <summary>
        /// 保存新建/更新的字典信息
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public virtual bool SaveDict(BootstrapDict dict) => throw new NotImplementedException();
        /// <summary>
        /// 保存网站个性化设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public virtual bool SaveSettings(BootstrapDict dict) => throw new NotImplementedException();
        /// <summary>
        /// 获取字典分类名称
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveCategories() => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveWebTitle() => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveWebFooter() => throw new NotImplementedException();
        /// <summary>
        /// 获得系统中配置的可以使用的网站样式
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveThemes() => throw new NotImplementedException();
        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveActiveTheme() => throw new NotImplementedException();
        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public virtual BootstrapDict RetrieveIconFolderPath() => throw new NotImplementedException();
        /// <summary>
        /// 获得默认的前台首页地址，默认为~/Home/Index
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveHomeUrl() => throw new NotImplementedException();
        /// <summary>
        /// 通过数据库获得所有字典表配置信息，缓存Key=DictHelper-RetrieveDicts
        /// </summary>
        /// <param name="db">数据库连接实例</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveDicts() => CacheManager.GetOrAdd(RetrieveDictsDataKey, key =>
        {
            string sql = "select ID, Category, Name, Code, Define from Dicts";
            var Dicts = new List<BootstrapDict>();
            var db = DBAccessManager.DBAccess;
            var cmd = db.CreateCommand(CommandType.Text, sql);
            using (var reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    Dicts.Add(new BootstrapDict
                    {
                        Id = LgbConvert.ReadValue(reader[0], 0),
                        Category = (string)reader[1],
                        Name = (string)reader[2],
                        Code = (string)reader[3],
                        Define = LgbConvert.ReadValue(reader[4], 0)
                    });
                }
            }
            return Dicts;
        });
    }
}
