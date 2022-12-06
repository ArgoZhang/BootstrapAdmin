// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BootstrapAdmin.Web.Services.SMS.Tencent
{
    /// <summary>
    /// 腾讯云短信平台接口
    /// </summary>
    public class TencentSMSProvider : ISMSProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public SMSOptions Options { get { return _options; } }

        private readonly HttpClient _client;
        private readonly TencentSMSOptions _options;
        private readonly Random _random;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="factory"></param>
        public TencentSMSProvider(IConfiguration configuration, IHttpClientFactory factory)
        {
            _options = configuration.GetSection(nameof(TencentSMSOptions)).Get<TencentSMSOptions>() ?? throw new InvalidOperationException("Please config the section TencentSMSOptions in appsettings.json");
            Options.RequestUrl = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms";
            _client = factory.CreateClient();
            _random = new Random();
        }

        private static readonly ConcurrentDictionary<string, AutoExpireValidateCode> _pool = new ConcurrentDictionary<string, AutoExpireValidateCode>();
        /// <summary>
        /// 手机下发验证码方法
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<SMSResult> SendCodeAsync(string phoneNumber)
        {
            // post https://yun.tim.qq.com/v5/tlssmssvr/sendsms?sdkappid=xxxxx&random=xxxx
            Options.Timestamp = (DateTimeOffset.UtcNow.Ticks - 621355968000000000) / 10000000;
            Options.Phone = phoneNumber;
            var requestParameters = new Dictionary<string, string?>()
            {
                { "sdkappid", _options.AppId },
                { "random", Options.Timestamp.ToString() }
            };

            var url = QueryHelpers.AddQueryString(Options.RequestUrl, requestParameters);
            var postData = new TencentSendData()
            {
                Sig = Sign(),
                Sign = _options.Sign,
                Time = Options.Timestamp,
                Tel = new TencentPhone() { Mobile = Options.Phone },
                Tpl_id = _options.TplId
            };
            var code = _random.Next(1000, 9999).ToString();
            postData.Params.Add(code);
            postData.Params.Add(Options.Expires.Minutes.ToString());

            var result = _options.Debug ? await Task.FromResult(new TencenResponse() { Result = 0 }) : await RequestSendCodeUrl(url, postData);
            var ret = new SMSResult() { Result = result.Result == 0, Msg = result.Errmsg };

            // debug 模式下发验证码到客户端
            if (_options.Debug) ret.Data = code;
            if (ret.Result)
            {
                _pool.AddOrUpdate(Options.Phone, key => new AutoExpireValidateCode(Options.Phone, code, Options.Expires, phone => _pool.TryRemove(phone, out var _)), (key, v) => v.Reset(code));
            }
            return ret;
        }

        private async Task<TencenResponse> RequestSendCodeUrl(string url, TencentSendData postData)
        {
            var req = await _client.PostAsJsonAsync(url, postData, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = await req.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TencenResponse>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            if (result!.Result != 0)
            {
                new Exception(result.Errmsg).Format(new NameValueCollection()
                {
                    ["UserId"] = Options.Phone,
                    ["url"] = url,
                    ["content"] = content
                });
            }
            return result;
        }

        private string Sign()
        {
            return Hash($"appkey={_options.AppKey}&random={Options.Timestamp}&time={Options.Timestamp}&mobile={Options.Phone}");
        }

        private static string Hash(string data)
        {
            using var algo = SHA256.Create();
            var sign = BitConverter.ToString(algo.ComputeHash(Encoding.UTF8.GetBytes(data)));
            sign = sign.Replace("-", "").ToLowerInvariant();
            return sign;
        }

        /// <summary>
        /// 验证手机验证码是否正确方法
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool Validate(string phoneNumber, string code) => _pool.TryGetValue(phoneNumber, out var signKey) && code == signKey.Code;

        /// <summary>
        /// 文档 https://cloud.tencent.com/document/product/382/5976
        /// </summary>
        private class TencentSendData
        {
            public string Ext { get; set; } = "";

            public string Extend { get; set; } = "";

            public ICollection<string> Params { get; } = new HashSet<string>();

            public string Sig { get; set; } = "";

            public string Sign { get; set; } = "";

            public TencentPhone Tel { get; set; } = new TencentPhone();

            public long Time { get; set; }

            public int Tpl_id { get; set; }
        }

        private class TencentPhone
        {
            public string Mobile { get; set; } = "";

            public string Nationcode { get; set; } = "86";
        }

        private class TencenResponse
        {
            public int Result { get; set; } = -1;

            public string Errmsg { get; set; } = "";

            public string Ext { get; set; } = "";

            public int Fee { get; set; }

            public string Sid { get; set; } = "";
        }
    }
}
