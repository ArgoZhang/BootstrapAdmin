using Longbow.AlipayAuth;
using Longbow.OAuth;
using Longbow.Security.Cryptography;
using Longbow.TencentAuth;
using Longbow.WeChatAuth;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        /// <summary>
        /// 设置 GiteeOptions.Events.OnCreatingTicket 方法
        /// </summary>
        /// <param name="option"></param>
        public static void Configure<TOptions>(TOptions option) where TOptions : LgbOAuthOptions
        {
            option.Events.OnCreatingTicket = async context =>
            {
                // call webhook
                var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var webhookUrls = config.GetSection<TOptions>().GetValue("StarredUrl", "").SpanSplit(";", StringSplitOptions.RemoveEmptyEntries);
                foreach (var webhookUrl in webhookUrls)
                {
                    var webhookParameters = new Dictionary<string, string?>()
                    {
                        { "access_token", context.AccessToken }
                    };
                    var url = QueryHelpers.AddQueryString(webhookUrl, webhookParameters);
                    var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
                    requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    await context.Backchannel.SendAsync(requestMessage, context.HttpContext.RequestAborted);
                }

                // 生成用户
                var user = ParseUser(context);
                user.App = option.App;
                SaveUser(user, option.Roles);

                // 记录登陆日志
                context.HttpContext.Log(user.UserName, true);
            };
        }

        /// <summary>
        /// 插入 Gitee 授权用户到数据库中
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static User ParseUser(OAuthCreatingTicketContext context)
        {
            var user = context.Scheme.DisplayName switch
            {
                WeChatDefaults.DisplayName => context.User.ToWeChatUser(),
                TencentDefaults.DisplayName => context.User.ToTencentUser(),
                AlipayDefaults.DisplayName => context.User.ToAlipayUser(),
                _ => context.User.ToAuthUser()
            };
            return new User()
            {
                ApprovedBy = "OAuth",
                ApprovedTime = DateTime.Now,
                DisplayName = user?.Name ?? "",
                UserName = user?.Login ?? "",
                Password = LgbCryptography.GenerateSalt(),
                Icon = user?.Avatar_Url ?? "",
                Description = $"{context.Scheme.Name}({user?.Id})"
            };
        }

        /// <summary>
        /// 保存用户到数据库中
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="roles"></param>
        internal static void SaveUser(User newUser, IEnumerable<string> roles)
        {
            if (string.IsNullOrEmpty(newUser.Id))
            {
                var uid = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == newUser.UserName)?.Id;
                var user = DbContextManager.Create<User>();
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(uid)) user.Delete(new string[] { uid });
                    if (user.Save(newUser))
                    {
                        // 根据配置文件设置默认角色
                        var role = DbContextManager.Create<Role>();
                        if (role != null)
                        {
                            var roleIds = role.Retrieves().Where(r => roles.Any(rl => rl.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase))).Select(r => r.Id);
                            if (roleIds.Any())
                            {
#nullable disable
                                role.SaveByUserId(newUser.Id, roleIds);
#nullable restore
                                CacheCleanUtility.ClearCache(userIds: new string[0], roleIds: new string[0], cacheKey: $"{UserHelper.RetrieveUsersByNameDataKey}-{newUser.UserName}");
                            }
                        }
                    }
                }
            }
        }
    }
}
