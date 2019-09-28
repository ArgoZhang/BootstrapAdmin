using Longbow.OAuth;
using Longbow.Security.Cryptography;
using Longbow.WeChatAuth;
using Microsoft.AspNetCore.Authentication.OAuth;
using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 微信登陆帮助类
    /// </summary>
    public static class WeChatHelper
    {
        /// <summary>
        /// 微信登陆配置方法
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="option"></param>
        public static void Configure<TOptions>(TOptions option) where TOptions : LgbOAuthOptions
        {
            option.Events.OnCreatingTicket = context =>
            {
                // 生成用户
                var user = ParseUser(context);
                user.App = option.App;
                OAuthHelper.SaveUser(user, option.Roles);

                // 记录登陆日志
                context.HttpContext.Log(user.DisplayName, true);
                return System.Threading.Tasks.Task.CompletedTask;
            };
        }

        /// <summary>
        /// 插入 Gitee 授权用户到数据库中
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static User ParseUser(OAuthCreatingTicketContext context)
        {
            var user = context.User.ToObject<WeChatUser>();
            return new User()
            {
                ApprovedBy = "OAuth",
                ApprovedTime = DateTime.Now,
                DisplayName = user.NickName,
                UserName = user.UnionId,
                Password = LgbCryptography.GenerateSalt(),
                Icon = user.HeadImgUrl,
                Description = $"{context.Scheme.Name}"
            };
        }
    }
}
