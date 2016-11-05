using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class QueryDictOption : PaginationOption
    {
        /// <summary>
        /// 字典分项
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 字典名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字典种类
        /// </summary>
        public string Define { get; set; }
        /// <summary>
        /// 字典表查询
        /// </summary>
        /// <returns></returns>
        public QueryData<Dict> RetrieveData()
        {
            var data = DictHelper.RetrieveDicts();
            if (!string.IsNullOrEmpty(Category))
            {
                data = data.Where(t => t.Category.Contains(Category));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                data = data.Where(t => t.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Define))
            {
                data = data.Where(t => t.Define.ToString() == Define);
            }
            var ret = new QueryData<Dict>();
            ret.total = data.Count();
            // 通过option.Sort属性判断对那列进行排序，现在对字典表Category列排序
            data = Order == "asc" ? data.OrderBy(t => t.Category) : data.OrderByDescending(t => t.Category);
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}