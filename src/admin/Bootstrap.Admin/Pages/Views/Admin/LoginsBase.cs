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
    public class LoginsBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 编辑类型实例
        /// </summary>
        protected LoginUser DataContext { get; set; } = new LoginUser();

        /// <summary>
        /// 获得/设置 查询绑定类型实例
        /// </summary>
        protected LoginUser QueryModel { get; set; } = new LoginUser();

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
        protected QueryData<LoginUser> Query(QueryPageOptions options)
        {
            var data = LoginHelper.RetrievePages(options.ToPaginationOption(), StartTime, EndTime, QueryModel.Ip);
            return data.ToQueryData();
        }

        /// <summary>
        /// 格式化登录结果方法
        /// </summary>
        protected MarkupString FormatResult(string result)
        {
            var css = result == "登录成功" ? "success" : "danger";
            var icon = css == "success" ? "check" : "remove";
            return new MarkupString($"<span class=\"badge badge-md badge-{css}\"><i class=\"fa fa-{icon}\"></i>{result}</span>");
        }

        /// <summary>
        /// 重置搜索方法
        /// </summary>
        protected void ResetSearch()
        {
            QueryModel.Ip = "";
        }
    }
}
