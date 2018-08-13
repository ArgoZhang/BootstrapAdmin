using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Query
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
        public QueryData<BootstrapDict> RetrieveData()
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
            var ret = new QueryData<BootstrapDict>();
            ret.total = data.Count();
            // 通过option.Sort属性判断对那列进行排序
            switch (Sort)
            {
                case "Category":
                    data = Order == "asc" ? data.OrderBy(t => t.Category) : data.OrderByDescending(t => t.Category);
                    break;
                case "Name":
                    data = Order == "asc" ? data.OrderBy(t => t.Name) : data.OrderByDescending(t => t.Name);
                    break;
                case "Code":
                    data = Order == "asc" ? data.OrderBy(t => t.Code) : data.OrderByDescending(t => t.Code);
                    break;
                case "Define":
                    data = Order == "asc" ? data.OrderBy(t => t.Define) : data.OrderByDescending(t => t.Define);
                    break;
                default:
                    break;
            }
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}