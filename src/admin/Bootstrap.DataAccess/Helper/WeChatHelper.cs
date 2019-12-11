using Longbow.OAuth;
using Longbow.Security.Cryptography;
using Longbow.WeChatAuth;
using Microsoft.AspNetCore.Authentication.OAuth;
using System;
using System.Linq;
using System.Text.Json;

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
        /// <param name="context"></param>
        /// <returns></returns>
        private static User ParseUser(OAuthCreatingTicketContext context)
        {
            var user = context.User.ToObject<WeChatUser>();
            return new User()
            {
                ApprovedBy = "OAuth",
                ApprovedTime = DateTime.Now,
                DisplayName = user?.NickName ?? "",
                UserName = user?.UnionId ?? "",
                Password = LgbCryptography.GenerateSalt(),
                Icon = user?.HeadImgUrl ?? "",
                Description = $"{context.Scheme.Name}"
            };
        }

        private static T? ToObject<T>(this JsonElement element) where T : WeChatUser
        {
            var user = new WeChatUser();
            var target = element.EnumerateObject();
            user.OpenId = target.TryGetValue("OpenId");
            user.UnionId = target.TryGetValue("UnionId");
            user.NickName = target.TryGetValue("NickName");
            user.Privilege = target.TryGetValue("Privilege");
            user.Sex = target.TryGetValue("Sex");
            user.Province = target.TryGetValue("Province");
            user.City = target.TryGetValue("City");
            user.Country = target.TryGetValue("Country");
            user.HeadImgUrl = target.TryGetValue("HeadImgUrl");
            user.Privilege = target.TryGetValue("Privilege");
            return user as T;
        }

        private static string TryGetValue(this JsonElement.ObjectEnumerator target, string propertyName)
        {
            var ret = string.Empty;
            var property = target.FirstOrDefault(t => t.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            ret = property.Value.ToString();
            return ret;
        }
    }
}
