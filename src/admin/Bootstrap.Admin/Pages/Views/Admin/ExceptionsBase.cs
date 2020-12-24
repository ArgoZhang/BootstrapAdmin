using Bootstrap.Admin.Pages.Components;
using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

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
        /// 获得 错误日志文件集合
        /// </summary>
        protected IEnumerable<string> Items { get; set; } = new string[0];

        private bool show;
        /// <summary>
        /// 显示异常明细方法
        /// </summary>
        protected void ShowDetail()
        {
            Items = ExceptionsHelper.RetrieveLogFiles();
            show = true;
            StateHasChanged();
        }

        /// <summary>
        /// OnAfterRender 方法
        /// </summary>
        protected override void OnAfterRender(bool firstRender)
        {
            if (show)
            {
                show = false;
                Modal?.Toggle();
            }
        }

        /// <summary>
        /// 获得/设置 Modal 实例
        /// </summary>
        protected ModalBase? Modal { get; set; }

        /// <summary>
        /// 显示指定文件内容明细
        /// </summary>
        protected void ShowLog(string fileName)
        {

        }
    }
}
