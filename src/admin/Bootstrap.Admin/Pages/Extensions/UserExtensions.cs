using Bootstrap.DataAccess;

namespace Bootstrap.Admin.Pages.Extensions
{
    /// <summary>
    /// 获得 用户显示名称
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// 获得 用户显示名称
        /// </summary>
        public static string FormatDisplayName(this User user)
        {
            var displayName = user.DisplayName;
            if (string.IsNullOrEmpty(displayName)) displayName = user.UserName;
            return displayName;
        }
    }
}
