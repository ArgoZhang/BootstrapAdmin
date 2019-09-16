using Bootstrap.Security;
using Longbow.Configuration;
using Longbow.OAuth;
using Longbow.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// Gitee 授权帮助类
    /// </summary>
    public static class OAuthHelper
    {
        private static readonly ConcurrentDictionary<string, OAuthUser> _pool = new ConcurrentDictionary<string, OAuthUser>();

        /// <summary>
        /// 设置 GiteeOptions.Events.OnCreatingTicket 方法
        /// </summary>
        /// <param name="option"></param>
        public static void Configure<TOptions>(TOptions option) where TOptions : LgbOAuthOptions
        {
            option.Events.OnCreatingTicket = async context =>
            {
                var user = context.User.ToObject<OAuthUser>();
                user.Schema = context.Scheme.Name;
                _pool.AddOrUpdate(user.Login, userName => user, (userName, u) => { u = user; return user; });

                // call webhook
                var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var webhookUrl = config.GetValue($"{option.GetType().Name}:StarredUrl", "");
                if (!string.IsNullOrEmpty(webhookUrl))
                {
                    var webhookParameters = new Dictionary<string, string>()
                    {
                        { "access_token", context.AccessToken }
                    };
                    var url = QueryHelpers.AddQueryString(webhookUrl, webhookParameters);
                    var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
                    requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    await context.Backchannel.SendAsync(requestMessage, context.HttpContext.RequestAborted);
                }
            };
        }

        /// <summary>
        /// 插入 Gitee 授权用户到数据库中
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser RetrieveUserByUserName<TOptions>(string userName) where TOptions : LgbOAuthOptions
        {
            User ret = null;
            var user = _pool.TryGetValue(userName, out var giteeUser) ? giteeUser : null;
            if (user != null)
            {
                var option = ConfigurationManager.Get<TOptions>();
                ret = new User()
                {
                    ApprovedBy = "OAuth",
                    ApprovedTime = DateTime.Now,
                    DisplayName = user.Name,
                    UserName = user.Login,
                    Password = LgbCryptography.GenerateSalt(),
                    Icon = user.Avatar_Url,
                    Description = $"{user.Schema}({user.Id})",
                    App = option.App
                };
                DbContextManager.Create<User>().Save(ret);
                CacheCleanUtility.ClearCache(cacheKey: UserHelper.RetrieveUsersDataKey);

                // 根据配置文件设置默认角色
                var usr = UserHelper.Retrieves().First(u => u.UserName == userName);
                var roles = RoleHelper.Retrieves().Where(r => option.Roles.Any(rl => rl.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase))).Select(r => r.Id);
                RoleHelper.SaveByUserId(usr.Id, roles);
            }
            return ret;
        }
    }
}
