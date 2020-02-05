using Bootstrap.Admin.Pages.Components;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 页面组件基类
    /// </summary>
    public abstract class QueryPageBase<TItem> : PageBase where TItem : class, new()
    {
        /// <summary>
        /// 获得/设置 是否固定表头 默认为 false 不固定表头
        /// </summary>
        [Parameter]
        public bool FixedHeader { get; set; }

        /// <summary>
        /// 获得/设置 EditPage 实例
        /// </summary>
        protected EditPageBase<TItem>? EditPage { get; set; }

        /// <summary>
        /// 获得/设置 TItem 实例
        /// </summary>
        protected TItem QueryModel { get; set; } = new TItem();

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="options"></param>
        protected abstract QueryData<TItem> Query(QueryPageOptions options);

        /// <summary>
        /// OnParametersSet 方法
        /// </summary>
        public override System.Threading.Tasks.Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            FixedHeader = DictHelper.RetrieveFixedTableHeader();
            return base.SetParametersAsync(ParameterView.Empty);
        }

        /// <summary>
        /// 新建方法
        /// </summary>
        /// <returns></returns>
        protected virtual TItem Add() => new TItem();

        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="item"></param>
        protected abstract bool Save(TItem item);

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="items"></param>
        protected abstract bool Delete(IEnumerable<TItem> items);
    }
}
