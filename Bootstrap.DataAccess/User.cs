namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User
    {
        /// <summary>
        /// 获得/设置 用户主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获得/设置 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 获取/设置 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 获取/设置 验证信息
        /// </summary>
        public string PassSalt { get; set; }
    }
}
