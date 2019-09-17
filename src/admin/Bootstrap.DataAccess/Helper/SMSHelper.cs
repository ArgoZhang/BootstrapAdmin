using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 手机号登陆帮助类
    /// </summary>
    public static class SMSHelper
    {
        private static ConcurrentDictionary<string, AutoExpireValidateCode> _pool = new ConcurrentDictionary<string, AutoExpireValidateCode>();

        /// <summary>
        /// 下发验证码方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<bool> SendCode(this HttpClient client, SMSOptions option)
        {
            option.Timestamp = (DateTimeOffset.UtcNow.Ticks - 621355968000000000) / 10000000;
            var requestParameters = new Dictionary<string, string>()
            {
                { "CompanyCode", option.CompanyCode },
                { "Phone", option.Phone },
                { "TimeStamp", option.Timestamp.ToString() },
                { "Sign", option.Sign() }
            };

            var url = QueryHelpers.AddQueryString("http://open.bluegoon.com/api/sms/sendcode", requestParameters);
            var req = await client.GetAsync(url);
            var content = await req.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SMSResult>(content);
            var ret = false;
            if (result.Code == "1")
            {
                _pool.AddOrUpdate(option.Phone, key => new AutoExpireValidateCode(option.Phone, result.Data, option.Expires), (key, v) => v.Reset(result.Data));
                ret = true;
            }
            else
            {
                new Exception("SMS Send Fail").Log(new NameValueCollection()
                {
                    ["UserId"] = option.Phone,
                    ["url"] = url,
                    ["content"] = content
                });
            }
            return ret;
        }

        /// <summary>
        /// 验证验证码方法
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static bool Validate(string phone, string code, string secret) => _pool.TryGetValue(phone, out var signKey) && Hash($"{code}{secret}") == signKey.Code;

        private static string Sign(this SMSOptions option)
        {
            return Hash($"{option.CompanyCode}{option.Phone}{option.Timestamp}{option.MD5Key}");
        }

        private static string Hash(string data)
        {
            using (var md5 = MD5.Create())
            {
                var sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(data)));
                sign = sign.Replace("-", "").ToLowerInvariant();
                return sign;
            }
        }

        private class SMSResult
        {
            public string Code { get; set; }

            public string Data { get; set; }
        }

        private class AutoExpireValidateCode
        {
            public AutoExpireValidateCode(string phone, string code, TimeSpan expires)
            {
                Phone = phone;
                Code = code;
                Expires = expires;
                RunAsync();
            }

            /// <summary>
            /// 
            /// </summary>
            public string Code { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string Phone { get; }

            /// <summary>
            /// 
            /// </summary>
            public TimeSpan Expires { get; set; }

            private CancellationTokenSource _tokenSource;

            private System.Threading.Tasks.Task RunAsync() => System.Threading.Tasks.Task.Run(() =>
            {
                _tokenSource = new CancellationTokenSource();
                if (!_tokenSource.Token.WaitHandle.WaitOne(Expires)) _pool.TryRemove(Phone, out var _);
            });

            /// <summary>
            /// 
            /// </summary>
            /// <param name="code"></param>
            public AutoExpireValidateCode Reset(string code)
            {
                Code = code;
                _tokenSource.Cancel();
                RunAsync();
                return this;
            }
        }
    }

    /// <summary>
    /// 短信网关配置类
    /// </summary>
    public class SMSOptions
    {
        /// <summary>
        /// 获得/设置 公司编码
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 获得/设置 下发手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 获得/设置 签名密钥
        /// </summary>
        public string MD5Key { get; set; }

        /// <summary>
        /// 获得/设置 时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 获得/设置 验证码有效时长
        /// </summary>
        public TimeSpan Expires { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 获得/设置 角色集合
        /// </summary>
        public ICollection<string> Roles { get; } = new HashSet<string>();

        /// <summary>
        /// 获得/设置 登陆后首页
        /// </summary>
        public string HomePath { get; set; }

        /// <summary>
        /// 获得/设置 默认授权 App
        /// </summary>
        public string App { get; set; }
    }
}
