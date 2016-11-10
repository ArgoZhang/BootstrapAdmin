using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User : Longbow.Security.Principal.LgbUser
    {
        /// <summary>
        /// 获得/设置 用户主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获取/设置 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 获取/设置 密码盐
        /// </summary>
        public string PassSalt { get; set; }
        /// <summary>
        /// 获取/设置 角色用户关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; }
        /// <summary>
        /// 获得/设置 用户注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 获得/设置 用户被批复时间
        /// </summary>
        public DateTime ApprovedTime { get; set; }
        /// <summary>
        /// 获得/设置 用户的申请理由
        /// </summary>
        public string Description { get; set; }
    }
}
