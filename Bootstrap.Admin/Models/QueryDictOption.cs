using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class QueryDictOption : PaginationOption
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字典种类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 字典表查询
        /// </summary>
        /// <returns></returns>
        public QueryData<Dict> RetrieveData()
        {
            var data = DictHelper.RetrieveDicts(string.Empty);
            if (!string.IsNullOrEmpty(Name))
            {
                data = data.Where(t => t.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Category))
            {
                data = data.Where(t => t.Category.Contains(Category));
            }
            var ret = new QueryData<Dict>();
            ret.total = data.Count();
            // TODO: 通过option.Sort属性判断对那列进行排序，现在统一对名称列排序
            data = Order == "asc" ? data.OrderBy(t => t.Name) : data.OrderByDescending(t => t.Name);
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}