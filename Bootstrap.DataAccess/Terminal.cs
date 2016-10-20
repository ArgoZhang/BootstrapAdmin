namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Terminal
    {
        /// <summary>
        /// 获得/设置 输入口主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获得/设置 输入口名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获得/设置 公共机IP
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// 获得/设置 公共机Port
        /// </summary>
        public int ClientPort { get; set; }
        /// <summary>
        /// 获得/设置 服务器IP
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// 获得/设置 服务器Port
        /// </summary>
        public int ServerPort { get; set; }
        /// <summary>
        /// 获得/设置 比对设备IP
        /// </summary>
        public string DeviceIP { get; set; }
        /// <summary>
        /// 获得/设置 比对设备Port
        /// </summary>
        public int DevicePort { get; set; }
        /// <summary>
        /// 获得/设置 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// 获得/设置 数据库用户名
        /// </summary>
        public string DatabaseUserName { get; set; }
        /// <summary>
        /// 获得/设置 数据库密码
        /// </summary>
        public string DatabasePassword { get; set; }
        /// <summary>
        /// 获得/设置 输入口状态 真为开启，假为停止
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 获得/设置 规则名称
        /// </summary>
        public string RuleName { get; set; }
        /// <summary>
        /// 获得/设置 规则ID
        /// </summary>
        public int RuleID { get; set; }
        /// <summary>
        /// 获得/设置 与其相关联的DataGridViewRowIndex
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// 获得/设置 错误描述信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 获得/设置 条码信息
        /// </summary>
        public string BarCode { get; set; }
    }
}
