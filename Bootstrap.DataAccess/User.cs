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
        /// 获得/设置 登陆账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 获取/设置 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 获取/设置 密码盐
        /// </summary>
        public string PassSalt { get; set; }
        /// <summary>
        /// 获取/设置 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 获取/设置 角色用户关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; }
    }
}
