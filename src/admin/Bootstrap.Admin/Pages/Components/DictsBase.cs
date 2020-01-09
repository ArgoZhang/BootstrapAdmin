using Bootstrap.Admin.Components;
using Bootstrap.Security;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Pages.Admin.Components
{
    /// <summary>
    /// 字典表维护组件
    /// </summary>
    public class DictsBase : PageBase
    {
        /// <summary>
        /// 获得/设置 BootstrapDict 实例
        /// </summary>
        protected BootstrapDict QueryModel { get; set; } = new BootstrapDict() { Define = -1 };

        /// <summary>
        /// 获得/设置 字典类别集合
        /// </summary>
        protected List<SelectedItem> DefineItems { get; set; } = new List<SelectedItem>(new SelectedItem[] { new SelectedItem() { Text = "系统使用", Value = "0" }, new SelectedItem() { Text = "自定义", Value = "1" } });

        /// <summary>
        /// 获得/设置 查询条件集合
        /// </summary>
        protected List<SelectedItem> QueryDefine { get; set; } = new List<SelectedItem>(new SelectedItem[] { new SelectedItem() { Text = "全部", Value = "-1", Active = true }, new SelectedItem() { Text = "系统使用", Value = "0" }, new SelectedItem() { Text = "自定义", Value = "1" } });

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageItems">每页显示数据条目数量</param>
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
        /// 新建方法
        /// </summary>
        /// <returns></returns>
        protected BootstrapDict Add()
        {
            return new BootstrapDict();
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        protected bool Save(BootstrapDict dict) => DataAccess.DictHelper.Save(dict);

        /// <summary>
        /// 删除方法
        /// </summary>
        protected bool Delete(IEnumerable<BootstrapDict> items) => DataAccess.DictHelper.Delete(items.Select(item => item.Id ?? ""));
    }
}
