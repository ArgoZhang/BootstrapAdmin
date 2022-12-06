// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BootstrapAdmin.Web.Services.SMS;

/// <summary>
/// 手机号登陆帮助类
/// </summary>
public class DefaultSMSProvider : ISMSProvider
{
    private static readonly ConcurrentDictionary<string, AutoExpireValidateCode> _pool = new ConcurrentDictionary<string, AutoExpireValidateCode>();

    /// <summary>
    /// 获得 短信配置信息
    /// </summary>
    public SMSOptions Options { get { return _options; } }

    private readonly DefaultSMSOptions _options;
    private readonly HttpClient _client;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="factory"></param>
    public DefaultSMSProvider(IConfiguration configuration, IHttpClientFactory factory)
    {
        _options = configuration.GetSection(nameof(SMSOptions)).Get<DefaultSMSOptions>() ?? throw new InvalidOperationException("Please config the section of SMSOptions in appsettings.json");
        _client = factory.CreateClient();
    }

    /// <summary>
    /// 下发验证码方法
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public async Task<SMSResult> SendCodeAsync(string phoneNumber)
    {
        Options.Timestamp = (DateTimeOffset.UtcNow.Ticks - 621355968000000000) / 10000000;
        Options.Phone = phoneNumber;
        var requestParameters = new Dictionary<string, string?>()
        {
            { "CompanyCode", _options.CompanyCode },
            { "Phone", Options.Phone },
            { "TimeStamp", Options.Timestamp.ToString() },
            { "Sign", Sign() }
        };

        var url = QueryHelpers.AddQueryString(Options.RequestUrl, requestParameters);
        var req = await _client.GetAsync(url);
        var content = await req.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DefaultSMSResult>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        var ret = new SMSResult() { Result = result!.Code == 1, Msg = result.Msg };
        if (ret.Result)
        {
            _pool.AddOrUpdate(Options.Phone, key => new AutoExpireValidateCode(Options.Phone, result.Data, Options.Expires, phone => _pool.TryRemove(phone, out var _)), (key, v) => v.Reset(result.Data));
        }
        else
        {
            new Exception(result.Msg).Format(new NameValueCollection()
            {
                ["UserId"] = Options.Phone,
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
    public bool Validate(string phone, string code) => _pool.TryGetValue(phone, out var signKey) && Hash($"{code}{_options.MD5Key}") == signKey.Code;

    private string Sign()
    {
        return Hash($"{_options.CompanyCode}{Options.Phone}{Options.Timestamp}{_options.MD5Key}");
    }

    private static string Hash(string data)
    {
        var sign = BitConverter.ToString(MD5.HashData(Encoding.UTF8.GetBytes(data)));
        sign = sign.Replace("-", "").ToLowerInvariant();
        return sign;
    }

    private class DefaultSMSResult
    {
        public int Code { get; set; }

        public string Data { get; set; } = "";

        public string Msg { get; set; } = "";
    }
}

/// <summary>
/// 
/// </summary>
public class DefaultSMSOptions : SMSOptions
{
    /// <summary>
    /// 获得/设置 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = "";

    /// <summary>
    /// 获得/设置 签名密钥
    /// </summary>
    public string MD5Key { get; set; } = "";
}
