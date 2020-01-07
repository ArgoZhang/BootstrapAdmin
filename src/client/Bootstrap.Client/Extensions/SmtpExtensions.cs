using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bootstrap.Client.Extensions
{
    /// <summary>
    /// SmtpExtensions 扩展类
    /// </summary>
    public static class SmtpExtensions
    {
        /// <summary>
        /// 异步发送方法
        /// </summary>
        public static Task<bool> SendAsync(this SmtpMessage message)
        {
            message.Push();
            return Task.FromResult(true);
        }
    }

    /// <summary>
    /// SmtpMessage 邮件实体类
    /// </summary>
    public class SmtpMessage
    {
        /// <summary>
        /// 获得/设置 主机地址
        /// </summary>
        public string Host { get; set; } = "";

        /// <summary>
        /// 获得/设置 邮箱密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// 获得/设置 发件人地址
        /// </summary>
        public string From { get; set; } = "";

        /// <summary>
        /// 获得/设置 收件人地址
        /// </summary>
        public string To { get; set; } = "";

        /// <summary>
        /// 获得/设置 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获得/设置 是否启用 SSL
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// 获得/设置 邮件标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 获得/设置 邮件正文
        /// </summary>
        public string Message { get; set; } = "";

        private static BlockingCollection<SmtpMessage> _messageQueue = new BlockingCollection<SmtpMessage>(new ConcurrentQueue<SmtpMessage>());
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static readonly List<SmtpMessage> _currentBatch = new List<SmtpMessage>();
        private static Task _logTask = Task.Run(ProcessLogQueue);

        /// <summary>
        /// 将邮件添加到队列中
        /// </summary>
        public void Push()
        {
            if (!string.IsNullOrEmpty(Password) && !_messageQueue.IsAddingCompleted) _messageQueue.Add(this, _cancellationTokenSource.Token);
        }

        private static async Task ProcessLogQueue()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var limit = 100;
                while (limit > 0 && _messageQueue.TryTake(out var message))
                {
                    _currentBatch.Add(message);
                    limit--;
                }

                try
                {
                    if (_currentBatch.Any())
                    {
                        await SendAsync(_currentBatch);
                        _currentBatch.Clear();
                    }
                    await Task.Delay(60000, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
            _cancellationTokenSource.Dispose();

            // flush message to file
            while (_messageQueue.TryTake(out var message)) _currentBatch.Add(message);
            await SendAsync(_currentBatch);
        }

        private static async Task SendAsync(IEnumerable<SmtpMessage> messages)
        {
            if (messages.Any())
            {
                var content = new StringBuilder();
                SmtpClient? sender = null;
                string from = "";
                string to = "";
                string title = "";
                foreach (var msg in messages)
                {
                    if (sender == null)
                    {
                        from = msg.From;
                        to = msg.To;
                        title = msg.Title;
                        sender = new SmtpClient(msg.Host)
                        {
                            Credentials = new NetworkCredential(msg.From, msg.Password),
                            Port = msg.Port,
                            EnableSsl = msg.EnableSsl
                        };
                    }

                    // 合并消息
                    content.AppendLine(msg.Message);
                }
                if (sender != null) await sender.SendMailAsync(from, to, title, content.ToString());
            }
        }
    }
}
