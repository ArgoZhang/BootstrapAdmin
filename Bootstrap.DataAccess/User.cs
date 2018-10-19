using Bootstrap.Security;
using Longbow.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User : BootstrapUser
    {
        public const string RetrieveUsersDataKey = "BootstrapUser-RetrieveUsers";
        public const string RetrieveUsersByRoleIdDataKey = "BootstrapUser-RetrieveUsersByRoleId";
        public const string RetrieveUsersByGroupIdDataKey = "BootstrapUser-RetrieveUsersByGroupId";
        public const string RetrieveNewUsersDataKey = "UserHelper-RetrieveNewUsers";
        protected const string RetrieveUsersByNameDataKey = "BootstrapUser-RetrieveUsersByName";
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
        /// 获得/设置 用户当前状态 0 表示管理员注册用户 1 表示用户注册 2 表示更改密码 3 表示更改个人皮肤 4 表示更改显示名称 5 批复新用户注册操作
        /// </summary>
        public UserStates UserStatus { get; set; }
        /// <summary>
        /// 获得/设置 通知描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }
        /// <summary>
        /// 获得/设置 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 验证用户登陆账号与密码正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool Authenticate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password)) return false;
            string oldPassword = null;
            string passwordSalt = null;
            string sql = "select [Password], PassSalt from Users where ApprovedTime is not null and UserName = @UserName";
            var db = DBAccessManager.DBAccess;
            using (DbCommand cmd = db.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(db.CreateParameter("@UserName", userName));
                using (DbDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        oldPassword = (string)reader[0];
                        passwordSalt = (string)reader[1];
                    }
                }
            }
            return !string.IsNullOrEmpty(passwordSalt) && oldPassword == LgbCryptography.ComputeHash(password, passwordSalt);
        }
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IEnumerable<User> RetrieveUsers() => throw new NotImplementedException();
        /// <summary>
        /// 查询所有的新注册用户
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<User> RetrieveNewUsers() => throw new NotImplementedException();
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="value"></param>
        public virtual bool DeleteUser(IEnumerable<int> value) => throw new NotImplementedException();
        /// <summary>
        /// 保存新建
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveUser(User p) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public virtual bool UpdateUser(int id, string password, string displayName) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        public virtual bool ApproveUser(int id, string approvedBy) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public virtual bool ChangePassword(string userName, string password, string newPass)
        {
            bool ret = false;
            if (Authenticate(userName, password))
            {
                string sql = "Update Users set Password = @Password, PassSalt = @PassSalt where UserName = @userName";
                var passSalt = LgbCryptography.GenerateSalt();
                var newPassword = LgbCryptography.ComputeHash(newPass, passSalt);
                using (DbCommand cmd = DBAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@Password", newPassword));
                    cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@PassSalt", passSalt));
                    cmd.Parameters.Add(DBAccessManager.DBAccess.CreateParameter("@userName", userName));
                    ret = DBAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
                }
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rejectBy"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual bool RejectUser(int id, string rejectBy) => throw new NotImplementedException();
        /// <summary>
        /// 通过roleId获取所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<User> RetrieveUsersByRoleId(int roleId) => throw new NotImplementedException();
        /// <summary>
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public virtual bool SaveUsersByRoleId(int id, IEnumerable<int> userIds) => throw new NotImplementedException();
        /// <summary>
        /// 通过groupId获取所有用户
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public virtual IEnumerable<User> RetrieveUsersByGroupId(int groupId) => throw new NotImplementedException();
        /// <summary>
        /// 通过部门ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public virtual bool SaveUsersByGroupId(int id, IEnumerable<int> userIds) => throw new NotImplementedException();
        /// <summary>
        /// 根据用户名修改用户头像
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public virtual bool SaveUserIconByName(string userName, string iconName) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param namve="userName"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public virtual bool SaveDisplayName(string userName, string displayName) => throw new NotImplementedException();
        /// <summary>
        /// 根据用户名更改用户皮肤
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public virtual bool SaveUserCssByName(string userName, string cssName) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual BootstrapUser RetrieveUserByUserName(string name) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", UserName, DisplayName);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum UserStates
    {
        /// <summary>
        /// 
        /// </summary>
        ChangePassword,
        /// <summary>
        /// 
        /// </summary>
        ChangeTheme,
        /// <summary>
        /// 
        /// </summary>
        ChangeDisplayName,
        /// <summary>
        /// 
        /// </summary>
        ApproveUser,
        /// <summary>
        /// 
        /// </summary>
        RejectUser
    }
}
