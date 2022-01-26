// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Services.SMS;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class SMSLogin : IDisposable
{
    private bool IsSendCode { get; set; } = true;

    private string SendCodeText { get; set; } = "发送验证码";

    private CancellationTokenSource? CancelToken { get; set; }

    [NotNull]
    private string? PhoneNumber { get; set; }

    private string? Code { get; set; }

    [Inject]
    [NotNull]
    private ISMSProvider? SMSProvider { get; set; }

    async Task OnSendCode()
    {
        if (!string.IsNullOrEmpty(PhoneNumber))
        {
            var result = await SMSProvider.SendCodeAsync(PhoneNumber);
            if (result.Result)
            {
#if DEBUG
                Code = result.Data;
#endif
                IsSendCode = false;
                var count = 60;
                CancelToken ??= new CancellationTokenSource();
                while (CancelToken != null && !CancelToken.IsCancellationRequested && count > 0)
                {
                    SendCodeText = $"发送验证码 ({count--})";
                    StateHasChanged();
                    await Task.Delay(1000, CancelToken.Token);
                }
                SendCodeText = "发送验证码";
            }
            else
            {
                // 短信发送失败
            }
        }
        else
        {
            // 手机号不可为空
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (CancelToken != null)
            {
                CancelToken.Cancel();
                CancelToken.Dispose();
                CancelToken = null;
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
}
