namespace Bootstrap.Admin
{
    /// <summary>
    /// 任务描述类
    /// </summary>
    public class TaskWidget
    {
        /// <summary>
        /// 获得/设置 任务名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 获得/设置 任务执行体名称
        /// </summary>
        public string TaskExecutorName { get; set; } = "";

        /// <summary>
        /// 获得/设置 Cron 任务表达式
        /// </summary>
        public string CronExpression { get; set; } = "";
    }
}
