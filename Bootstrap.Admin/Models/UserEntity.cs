using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class UserEntity
    {
        public int total { get; private set; }

        public IEnumerable<User> rows { get; private set; }

        public void RetrieveUsers(TerminalsPageOption option)
        {
            // int limit, int offset, string name, string price, string sort, string order
            var data = UserHelper.RetrieveUsers(string.Empty);
            if (!string.IsNullOrEmpty(option.Name))
            {
                data = data.Where(t => t.UserName.Contains(option.Name));
            }
            total = data.Count();
            // TODO: 通过option.Sort属性判断对那列进行排序，现在统一对名称列排序
            data = option.Order == "asc" ? data.OrderBy(t => t.UserName) : data.OrderByDescending(t => t.UserName);
            rows = data.Skip(option.Offset).Take(option.Limit);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TerminalsPageOption : PaginationOption
    {
        public string Name { get; set; }
    }
}