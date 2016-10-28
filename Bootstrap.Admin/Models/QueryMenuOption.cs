using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bootstrap.Admin.Models
{
    public class QueryMenuOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }

        public QueryData<Menu> RetrieveData()
        {
            var data = MenuHelper.RetrieveMenus();
            if (!string.IsNullOrEmpty(Name))
            {
                data = data.Where(t => t.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Category))
            {
                data = data.Where(t => t.Category.ToString().Equals(Category));
            }
            var ret = new QueryData<Menu>();
            ret.total = data.Count();
            // TODO: 通过option.Sort属性判断对那列进行排序，现在统一对名称列排序
            data = Order == "asc" ? data.OrderBy(t => t.Name) : data.OrderByDescending(t => t.Name);
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}