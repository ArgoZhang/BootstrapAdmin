using Bootstrap.Client.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Linq;

namespace Bootstrap.Client.Query
{
    /// <summary>
    /// 演示例子查询类
    /// </summary>
    public class QueryDummyOption : PaginationOption
    {
        /// <summary>
        /// 获得/设置 查询条件1
        /// </summary>
        public string? Item1 { get; set; }

        /// <summary>
        /// 获得/设置 查询条件2
        /// </summary>
        public string? Item2 { get; set; }

        /// <summary>
        /// 获得/设置 查询条件3
        /// </summary>
        /// <remark>数据库定义此字段为数值型，查询类为何定义为 string? 类型？因为这里可以设置为全部</remark>
        public string? Item3 { get; set; }

        /// <summary>
        /// 字典表查询
        /// </summary>
        /// <returns></returns>
        public QueryData<Dummy> Retrieves()
        {
            if (string.IsNullOrEmpty(Order)) Order = "asc";
            if (string.IsNullOrEmpty(Sort)) Sort = "Item1";

            var data = DummyHelper.Retrieves();
            if (!string.IsNullOrEmpty(Item1))
            {
                data = data.Where(t => t.Item1.Contains(Item1, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(Item2))
            {
                data = data.Where(t => t.Item2.Contains(Item2, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(Item3))
            {
                // 此列为数值型
                data = data.Where(t => t.Item3.ToString() == Item3);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                // 处理快捷搜索文本条件
                data = data.Where(t => t.Item1.Contains(Search, StringComparison.OrdinalIgnoreCase) || t.Item2.Contains(Search, StringComparison.OrdinalIgnoreCase));
            }
            var ret = new QueryData<Dummy>();
            ret.total = data.Count();
            // 通过option.Sort属性判断对那列进行排序
            switch (Sort)
            {
                case "Item1":
                    data = Order == "asc" ? data.OrderBy(t => t.Item1) : data.OrderByDescending(t => t.Item1);
                    break;
                case "Item2":
                    data = Order == "asc" ? data.OrderBy(t => t.Item2) : data.OrderByDescending(t => t.Item2);
                    break;
                case "Item3":
                    data = Order == "asc" ? data.OrderBy(t => t.Item3) : data.OrderByDescending(t => t.Item3);
                    break;
            }
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}
