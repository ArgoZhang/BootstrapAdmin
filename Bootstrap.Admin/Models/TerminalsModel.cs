using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class TerminalsModel
    {
        public int total { get; private set; }

        public IEnumerable<Terminal> rows { get; private set; }

        public void RetrieveTerminals(TerminalsPageOption option)
        {
            // int limit, int offset, string name, string price, string sort, string order
            var data = TerminalHelper.RetrieveTerminals(string.Empty);
            if (!string.IsNullOrEmpty(option.Name))
            {
                data = data.Where(t => t.Name.Contains(option.Name));
            }
            if (!string.IsNullOrEmpty(option.Ip))
            {
                data = data.Where(t => t.ServerIP.Contains(option.Ip));
            }
            total = data.Count();
            // TODO: 通过option.Sort属性判断对那列进行排序，现在统一对名称列排序
            data = option.Order == "asc" ? data.OrderBy(t => t.Name) : data.OrderByDescending(t => t.Name);
            rows = data.Skip(option.Offset).Take(option.Limit);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TerminalsPageOption : PaginationOption
    {
        public string Name { get; set; }

        public string Ip { get; set; }
    }
}