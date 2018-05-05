using Bootstrap.Security;
using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User : BootstrapUser
    {
        /// <summary>
        /// 获得/设置 用户主键ID
        /// </summary>
        public int Id { get; set; }
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
        /// 获得/设置 用户批复人
        /// </summary>
        public string ApprovedBy { get; set; }
        /// <summary>
        /// 获得/设置 用户的申请理由
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 获得/设置 用户当前状态 0 表示管理员注册用户 1 表示用户自己注册 2 表示管理员批复 3 表示更改个人皮肤 9 表示前台remote validate
        /// </summary>
        public int UserStatus { get; set; }
        /// <summary>
        /// 获得/设置 通知描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }
        /// <summary>
        /// 获得/设置 拒绝人
        /// </summary>
        public string RejectedBy { get; set; }
        /// <summary>
        /// 获得/设置 拒绝原因
        /// </summary>
        public string RejectedReason { get; set; }
        /// <summary>
        /// 获得/设置 拒绝时刻
        /// </summary>
        public string RejectedTime { get; set; }
        /// <summary>
        /// 获得/设置 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", UserName, DisplayName);
        }
    }
}
