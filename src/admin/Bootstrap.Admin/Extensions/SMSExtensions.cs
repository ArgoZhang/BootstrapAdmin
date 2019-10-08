using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 短信登录扩展类 
    /// </summary>
    public static class SMSExtensions
    {
        /// <summary>
        /// 注入短信登录服务到容器中
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSMSProvider(this IServiceCollection services)
        {
            services.AddTransient<ISMSProvider, DefaultSMSProvider>();
            return services;
        }
    }

    /// <summary>
    /// 短信登录接口
    /// </summary>
    public interface ISMSProvider
    {
        /// <summary>
        ///  手机下发验证码方法
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        Task<bool> SendCodeAsync(string phoneNumber);

        /// <summary>
        ///  验证手机验证码是否正确方法
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        bool Validate(string phoneNumber, string code);

        /// <summary>
        /// 获得 配置信息
        /// </summary>
        SMSOptions Option { get; }
    }

    /// <summary>
    /// 手机号登陆帮助类
    /// </summary>
    internal class DefaultSMSProvider : ISMSProvider
    {
        private static ConcurrentDictionary<string, AutoExpireValidateCode> _pool = new ConcurrentDictionary<string, AutoExpireValidateCode>();

        /// <summary>
        /// 获得 短信配置信息
        /// </summary>
        public SMSOptions Option { get; protected set; }

        private HttpClient _client;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="factory"></param>
        public DefaultSMSProvider(IConfiguration configuration, IHttpClientFactory factory)
        {
            Option = configuration.GetSection<SMSOptions>().Get<SMSOptions>();
            _client = factory.CreateClient();
        }

        /// <summary>
        /// 下发验证码方法
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<bool> SendCodeAsync(string phoneNumber)
        {
            Option.Timestamp = (DateTimeOffset.UtcNow.Ticks - 621355968000000000) / 10000000;
            Option.Phone = phoneNumber;
            var requestParameters = new Dictionary<string, string>()
            {
                { "CompanyCode", Option.CompanyCode },
                { "Phone", Option.Phone },
                { "TimeStamp", Option.Timestamp.ToString() },
                { "Sign", Sign() }
            };

            var url = QueryHelpers.AddQueryString(Option.RequestUrl, requestParameters);
            var req = await _client.GetAsync(url);
            var content = await req.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SMSResult>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var ret = false;
            if (result.Code == 1)
            {
                _pool.AddOrUpdate(Option.Phone, key => new AutoExpireValidateCode(Option.Phone, result.Data, Option.Expires), (key, v) => v.Reset(result.Data));
                ret = true;
            }
            else
            {
                new Exception("SMS Send Fail").Log(new NameValueCollection()
                {
                    ["UserId"] = Option.Phone,
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
        /// <returns></returns>
        public bool Validate(string phone, string code) => _pool.TryGetValue(phone, out var signKey) && Hash($"{code}{Option.MD5Key}") == signKey.Code;

        private string Sign()
        {
            return Hash($"{Option.CompanyCode}{Option.Phone}{Option.Timestamp}{Option.MD5Key}");
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
            public int Code { get; set; }

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

            private Task RunAsync() => Task.Run(() =>
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

        /// <summary>
        /// 获得/设置 短信下发网关地址
        /// </summary>
        public string RequestUrl { get; set; } = "http://open.bluegoon.com/api/sms/sendcode";
    }
}
