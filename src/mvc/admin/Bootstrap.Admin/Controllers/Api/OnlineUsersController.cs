using Longbow.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 在线用户接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OnlineUsersController : ControllerBase
    {
        /// <summary>
        /// 获取所有在线用户数据
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IEnumerable<OnlineUser> Get([FromServices] IOnlineUsers onlineUSers)
        {
            return onlineUSers.OnlineUsers.OrderByDescending(u => u.LastAccessTime);
        }

        /// <summary>
        /// 获取指定会话的在线用户请求地址明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onlineUSers"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IEnumerable<KeyValuePair<DateTime, string>> Get(string id, [FromServices] IOnlineUsers onlineUSers)
        {
            var user = onlineUSers.OnlineUsers.FirstOrDefault(u => u.ConnectionId == id);
            return user?.RequestUrls ?? new KeyValuePair<DateTime, string>[0];
        }

        /// <summary>
        /// 登录页面检查调用
        /// </summary>
        /// <returns>返回真时 启用行为验证码</returns>
        [HttpPut]
        [AllowAnonymous]
        public bool Put()
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress?.ToIPv4String() ?? "";
            if (_loginUsers.TryGetValue(ip, out var user))
            {
                user.Reset();
                user.User.Count++;
                return user.User.Count > 2;
            }

            var loginUser = new LoginUser() { Ip = ip };
            _loginUsers.TryAdd(ip, new LoginUserCache(loginUser, () => _loginUsers.TryRemove(ip, out _)));
            return false;
        }

        private static ConcurrentDictionary<string, LoginUserCache> _loginUsers = new ConcurrentDictionary<string, LoginUserCache>();

        /// <summary>
        /// 
        /// </summary>
        private class LoginUser
        {
            /// <summary>
            /// 
            /// </summary>
            public string? Ip { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int Count { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        private class LoginUserCache : IDisposable
        {
            private Timer dispatcher;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="user"></param>
            /// <param name="action"></param>
            public LoginUserCache(LoginUser user, Action action)
            {
                User = user;
                dispatcher = new Timer(_ => action(), null, TimeSpan.FromSeconds(30), Timeout.InfiniteTimeSpan);
            }

            /// <summary>
            /// 
            /// </summary>
            public LoginUser User { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                dispatcher.Change(TimeSpan.FromSeconds(30), Timeout.InfiniteTimeSpan);
            }

            #region Impletement IDispose
            /// <summary>
            /// 
            /// </summary>
            /// <param name="disposing"></param>
            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (dispatcher != null)
                    {
                        dispatcher.Dispose();
                    }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion
        }
    }
}
