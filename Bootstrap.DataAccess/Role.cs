namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 获得/设置 角色主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 获得/设置 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 获得/设置 角色描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 获取/设置 用户角色关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; }
    }
}
