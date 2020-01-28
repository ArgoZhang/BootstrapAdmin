using Bootstrap.Admin.Pages.Components;
using Bootstrap.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 字典表维护组件
    /// </summary>
    public class DictsBase : QueryPageBase<BootstrapDict>
    {
        /// <summary>
        /// 获得/设置 字典类别集合
        /// </summary>
        protected List<SelectedItem> DefineItems { get; set; } = new List<SelectedItem>(new SelectedItem[] { new SelectedItem() { Text = "系统使用", Value = "0" }, new SelectedItem() { Text = "自定义", Value = "1" } });

        /// <summary>
        /// 获得/设置 查询条件集合
        /// </summary>
        protected List<SelectedItem> QueryDefine { get; set; } = new List<SelectedItem>(new SelectedItem[] { new SelectedItem() { Text = "全部", Value = "-1", Active = true }, new SelectedItem() { Text = "系统使用", Value = "0" }, new SelectedItem() { Text = "自定义", Value = "1" } });

        /// <summary>
        /// 获得/设置 查询条件集合
        /// </summary>
        protected override void OnInitialized()
        {
            QueryModel.Define = -1;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="options"></param>
        protected override QueryData<BootstrapDict> Query(QueryPageOptions options)
        {
            var data = DataAccess.DictHelper.RetrieveDicts();
            // filter
            if (QueryModel.Define != -1) data = data.Where(d => d.Define == QueryModel.Define);
            if (!string.IsNullOrEmpty(QueryModel.Name)) data = data.Where(d => d.Name.Contains(QueryModel.Name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(QueryModel.Category)) data = data.Where(d => d.Category.Contains(QueryModel.Category, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(options.SearchText)) data = data.Where(d => d.Category.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || d.Name.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) || d.Code.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase));

            // sort
            data = options.SortName switch
            {
                nameof(BootstrapDict.Category) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.Category) : data.OrderByDescending(d => d.Category),
                nameof(BootstrapDict.Name) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.Name) : data.OrderByDescending(d => d.Name),
                nameof(BootstrapDict.Code) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.Code) : data.OrderByDescending(d => d.Code),
                nameof(BootstrapDict.Define) => options.SortOrder == SortOrder.Asc ? data.OrderBy(d => d.Define) : data.OrderByDescending(d => d.Define),
                _ => data
            };
            var totalCount = data.Count();
            var items = data.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems);
            return new QueryData<BootstrapDict>() { Items = items, TotalCount = totalCount, PageIndex = options.PageIndex, PageItems = options.PageItems };
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        protected override bool Save(BootstrapDict dict) => DataAccess.DictHelper.Save(dict);

        /// <summary>
        /// 删除方法
        /// </summary>
        protected override bool Delete(IEnumerable<BootstrapDict> items) => DataAccess.DictHelper.Delete(items.Select(item => item.Id ?? ""));

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {
            QueryModel.Define = -1;
            QueryModel.Category = "";
            QueryModel.Name = "";
        }
    }
}
