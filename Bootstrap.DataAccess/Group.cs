namespace Bootstrap.DataAccess
{
    /// <summary>
    /// author:liuchun
    /// date:2016.10.22
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 获得/设置 群组主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 获得/设置 群组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 获得/设置 群组描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取/设置 用户群组关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; }
    }
}
