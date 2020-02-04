using Bootstrap.Admin.Pages.Components;
using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Components;
using System;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 部门维护组件
    /// </summary>
    public class ExceptionsBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 编辑类型实例
        /// </summary>
        protected Bootstrap.DataAccess.Exceptions DataContext { get; set; } = new Bootstrap.DataAccess.Exceptions();

        /// <summary>
        /// 获得/设置 查询绑定类型实例
        /// </summary>
        protected Bootstrap.DataAccess.Exceptions QueryModel { get; set; } = new Bootstrap.DataAccess.Exceptions();

        /// <summary>
        /// 获得/设置 开始时间
        /// </summary>
        protected DateTime? StartTime { get; set; }

        /// <summary>
        /// 获得/设置 开始时间
        /// </summary>
        protected DateTime? EndTime { get; set; }

        /// <summary>
        /// 数据查询方法
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected QueryData<Bootstrap.DataAccess.Exceptions> Query(QueryPageOptions options)
        {
            var data = ExceptionsHelper.RetrievePages(options.ToPaginationOption(), StartTime, EndTime);
            return data.ToQueryData();
        }

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {

        }

        /// <summary>
        /// 显示异常明细方法
        /// </summary>
        protected void ShowDetail()
        {

        }
    }
}
