using Bootstrap.Admin.Pages.Components;
using Microsoft.AspNetCore.Components;
using Longbow.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 部门维护组件
    /// </summary>
    public class TasksBase : ComponentBase
    {
        /// <summary>
        /// 获得/设置 编辑类型实例
        /// </summary>
        protected DefaultScheduler DataContext { get; set; } = new DefaultScheduler();

        /// <summary>
        /// 数据查询方法
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected QueryData<DefaultScheduler> Query(QueryPageOptions options)
        {
            var data = (string.IsNullOrEmpty(options.SearchText) ? TaskServicesManager.ToList() : TaskServicesManager.ToList().Where(t => t.Name.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase))).OrderBy(s => s.Name).Select(s => new DefaultScheduler()
            {
                Name = s.Name,
                Status = s.Status,
                NextRuntime = s.NextRuntime,
                CreatedTime = s.CreatedTime,
                Triggers = s.Triggers,
                LastRuntime = s.LastRuntime,
                LastRunResult = s.Triggers.First().LastResult,
                TriggerExpression = s.Triggers.First()?.ToString() ?? ""
            });
            return new QueryData<DefaultScheduler>()
            {
                Items = data,
                TotalCount = data.Count(),
                PageIndex = 1,
                PageItems = data.Count()
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected MarkupString FormatterStatus(SchedulerStatus status)
        {
            var content = status switch
            {
                SchedulerStatus.Ready => ("info", "fa", "未开始"),
                SchedulerStatus.Running => ("success", "play-circle", "运行中"),
                SchedulerStatus.Disabled => ("danger", "times-circle", "已停止"),
                _ => ("info", "fa", "未设置")
            };
            return new MarkupString($"<button class=\"btn btn-sm btn-{content.Item1}\"><i class=\"fa fa-{content.Item2}\"></i><span>{content.Item3}<span></button>");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected MarkupString FormatterResult(TriggerResult result)
        {
            var content = result switch
            {
                TriggerResult.Success => ("success", "成功"),
                TriggerResult.Error => ("danger", "故障"),
                TriggerResult.Cancelled => ("info", "取消"),
                TriggerResult.Timeout => ("warning", "超时"),
                _ => ("info", "未设置")
            };
            return new MarkupString($"<button class=\"btn btn-sm btn-{content.Item1}\"><span>{content.Item2}<span></button>");
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class DefaultScheduler : IScheduler
    {
        /// <summary>
        ///
        /// </summary>
        [DisplayName("名称")]
        public string Name { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        [DisplayName("状态")]
        public SchedulerStatus Status { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DisplayName("下次执行时间")]
        public DateTimeOffset? NextRuntime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DisplayName("上次执行时间")]
        public DateTimeOffset? LastRuntime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DisplayName("执行结果")]
        public TriggerResult LastRunResult { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DisplayName("创建时间")]
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<ITrigger> Triggers { get; set; } = new DefaultTrigger[0];

        /// <summary>
        ///
        /// </summary>
        public ITask? Task { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DisplayName("触发条件")]
        public string TriggerExpression { get; set; } = "";
    }

    /// <summary>
    ///
    /// </summary>
    public class DefaultTrigger : ITrigger
    {
        /// <summary>
        ///
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public Action<bool>? EnabeldChanged { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTimeOffset? LastRuntime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTimeOffset? NextRuntime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public TimeSpan LastRunElapsedTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public TriggerResult LastResult { get; set; }

        /// <summary>
        ///
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Action<ITrigger>? PulseCallback { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datas"></param>
        public void LoadData(Dictionary<string, object> datas)
        {

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public bool Pulse(CancellationToken cancellationToken = default)
        {
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> SetData()
        {
            return new Dictionary<string, object>();
        }
    }
}
