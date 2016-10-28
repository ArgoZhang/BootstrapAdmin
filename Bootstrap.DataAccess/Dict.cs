namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 字典表实体
    /// Category与Code自关联
    /// author:renshuo
    /// date:2016.10.27
    /// </summary>
    public class Dict
    {
        /// <summary>
        /// 字典主键
        /// 数据库自增
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代号
        /// </summary>
        public string Code { get; set; }
    }
}
