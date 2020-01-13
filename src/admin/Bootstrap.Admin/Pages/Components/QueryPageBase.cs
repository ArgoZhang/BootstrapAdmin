using System.Collections.Generic;
using Bootstrap.Admin.Components;

namespace Bootstrap.Pages.Admin.Components
{
    /// <summary>
    /// 页面组件基类
    /// </summary>
    public abstract class QueryPageBase<TItem> : PageBase where TItem : class, new()
    {
        /// <summary>
        /// 获得/设置 TItem 实例
        /// </summary>
        protected TItem QueryModel { get; set; } = new TItem();

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageItems">每页显示数据条目数量</param>
        protected abstract QueryData<TItem> Query(int pageIndex, int pageItems);

        /// <summary>
        /// 新建方法
        /// </summary>
        /// <returns></returns>
        protected TItem Add() => new TItem();

        /// <summary>
        /// 保存方法
        /// </summary>
        protected abstract bool Save(TItem dict);

        /// <summary>
        /// 删除方法
        /// </summary>
        protected abstract bool Delete(IEnumerable<TItem> items);
    }
}
