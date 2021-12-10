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
    public class LogsBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 编辑类型实例
        /// </summary>
        protected Log DataContext { get; set; } = new Log();

        /// <summary>
        /// 获得/设置 查询绑定类型实例
        /// </summary>
        protected Log QueryModel { get; set; } = new Log();

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
        protected QueryData<Log> Query(QueryPageOptions options)
        {
            var data = LogHelper.RetrievePages(options.ToPaginationOption(), StartTime, EndTime, QueryModel.CRUD);
            return data.ToQueryData();
        }

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {
            QueryModel.CRUD = "";
        }
    }
}
