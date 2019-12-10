using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 字典表维护组件
    /// </summary>
    public class DictsBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected BootstrapDict QueryModel { get; set; } = new BootstrapDict() { Define = -1 };

        /// <summary>
        /// 
        /// </summary>
        protected List<SelectedItem> DefineItems { get; set; } = new List<SelectedItem>(new SelectedItem[] { new SelectedItem() { Text = "系统使用", Value = "0" }, new SelectedItem() { Text = "自定义", Value = "1" } });

        /// <summary>
        /// 
        /// </summary>
        protected List<SelectedItem> QueryDefine { get; set; } = new List<SelectedItem>(new SelectedItem[] { new SelectedItem() { Text = "全部", Value = "-1", Active = true }, new SelectedItem() { Text = "系统使用", Value = "0" }, new SelectedItem() { Text = "自定义", Value = "1" } });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageItems"></param>
        protected QueryData<BootstrapDict> Query(int pageIndex, int pageItems)
        {
            var data = DataAccess.DictHelper.RetrieveDicts();
            if (QueryModel.Define != -1) data = data.Where(d => d.Define == QueryModel.Define);
            if (QueryModel.Name != "") data = data.Where(d => d.Name == QueryModel.Name);
            if (QueryModel.Category != "") data = data.Where(d => d.Category == QueryModel.Category);
            var totalCount = data.Count();
            var items = data.Skip((pageIndex - 1) * pageItems).Take(pageItems);
            return new QueryData<BootstrapDict>() { Items = items, TotalCount = totalCount, PageIndex = pageIndex, PageItems = pageItems };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected BootstrapDict Add()
        {
            return new BootstrapDict();
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool Save(BootstrapDict dict) => DataAccess.DictHelper.Save(dict);

        /// <summary>
        /// 
        /// </summary>
        protected bool Delete(IEnumerable<BootstrapDict> items) => DataAccess.DictHelper.Delete(items.Select(item => item.Id ?? ""));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldRender() => false;
    }
}
