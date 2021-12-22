namespace BootstrapAdmin.Web.Components;

public partial class SMSButton : IDisposable
{
    private bool IsSendCode { get; set; } = true;

    private string SendCodeText { get; set; } = "发送验证码";

    private CancellationTokenSource? CancelToken { get; set; }

    async Task OnSendCode()
    {
        IsSendCode = false;
        var count = 60;
        CancelToken ??= new CancellationTokenSource();
        while (!CancelToken.IsCancellationRequested && count > 0)
        {
            SendCodeText = $"发送验证码 ({count--})";
            StateHasChanged();
            await Task.Delay(1000, CancelToken.Token);
        }
        SendCodeText = "发送验证码";
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
