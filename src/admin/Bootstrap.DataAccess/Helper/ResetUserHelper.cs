using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 重置用户操作类
    /// </summary>
    public static class ResetUserHelper
    {
        /// <summary>
        /// 保存需要重置用户
        /// </summary>
        /// <returns></returns>
        public static bool Save(ResetUser user)
        {
            user.ResetTime = DateTime.Now;
            return DbContextManager.Create<ResetUser>()?.Save(user) ?? false;
        }
    }
}
