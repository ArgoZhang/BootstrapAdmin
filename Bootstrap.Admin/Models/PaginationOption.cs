namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 分页配置项类
    /// </summary>
    public class PaginationOption
    {
        /// <summary>
        /// 获得/设置 每页显示行数
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 获得/设置 当前页数
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 获得/设置 排序列名称
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 获得/设置 排序方式 asc/desc
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 获得/设置 搜索内容
        /// </summary>
        public string Search { get; set; }
    }
}
