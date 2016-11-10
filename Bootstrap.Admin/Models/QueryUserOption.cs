using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryUserOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        public QueryData<User> RetrieveData()
        {
            // int limit, int offset, string name, string price, string sort, string order
            var data = UserHelper.RetrieveUsers();
            if (!string.IsNullOrEmpty(Name))
            {
                data = data.Where(t => t.UserName.Contains(Name));
            }
            if (!string.IsNullOrEmpty(DisplayName))
            {
                data = data.Where(t => t.DisplayName.Contains(DisplayName));
            }
            var ret = new QueryData<User>();
            ret.total = data.Count();
            switch (Sort)
            {
                case "UserName":
                    data = Order == "asc" ? data.OrderBy(t => t.UserName) : data.OrderByDescending(t => t.UserName);
                    break;
                case "RegisterTime":
                    data = Order == "asc" ? data.OrderBy(t => t.RegisterTime) : data.OrderByDescending(t => t.RegisterTime);
                    break;
                default:
                    break;
            }
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
    }
}