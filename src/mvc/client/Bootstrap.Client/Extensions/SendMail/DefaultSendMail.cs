using Microsoft.Extensions.Options;
using System;
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
    /// 默认邮件发送操作类
    /// </summary>
    internal class DefaultSendMail : ISendMail
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public DefaultSendMail(IOptionsMonitor<SmtpOption> option)
        {
            Option = option.CurrentValue;
            option.OnChange(op => Option = op);
            ProcessLogQueue().ConfigureAwait(false);
        }

        private SmtpOption Option { get; set; }

        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="format"></param>
        /// <param name="mailBody"></param>
        /// <returns></returns>
        public Task<bool> SendMailAsync(MessageFormat format, string mailBody)
        {
            if (!string.IsNullOrEmpty(Option.Password) && !_messageQueue.IsAddingCompleted && !FilterByBody(format, mailBody)) _messageQueue.Add(new SmtpMessage() { Title = format.ToTitle(), Message = mailBody }, _cancellationTokenSource.Token);
            return Task.FromResult(true);
        }

        private bool FilterByBody(MessageFormat format, string mailBody)
        {
            var ret = false;
            if (format == MessageFormat.Exception)
            {
                // MachineName: 172_17_0_10
                var findKey = "MachineName: ";
                var first = mailBody.IndexOf(findKey) + findKey.Length;
                var last = mailBody.IndexOf("<br>", first);
                var machineName = mailBody[first..last];

                // 通过 配置文件过滤
                ret = Option.BlackList.Any(i => i.Equals(machineName, StringComparison.OrdinalIgnoreCase));
            }
            return ret;
        }

        private readonly BlockingCollection<SmtpMessage> _messageQueue = new BlockingCollection<SmtpMessage>(new ConcurrentQueue<SmtpMessage>());
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly List<SmtpMessage> _currentBatch = new List<SmtpMessage>();

        private async Task ProcessLogQueue()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var limit = 100;
                while (limit > 0 && _messageQueue.TryTake(out var message))
                {
                    _currentBatch.Add(message);
                    limit--;
                }

                if (_currentBatch.Any())
                {
                    await SendAsync(_currentBatch);
                    _currentBatch.Clear();
                }
#if DEBUG
                await Task.Delay(2000, _cancellationTokenSource.Token);
#else
                await Task.Delay(60000, _cancellationTokenSource.Token);
#endif
            }
            _cancellationTokenSource.Dispose();

            // flush message to file
            while (_messageQueue.TryTake(out var message)) _currentBatch.Add(message);
            await SendAsync(_currentBatch);
        }

        private async Task SendAsync(IEnumerable<SmtpMessage> messages)
        {
            if (messages.Any())
            {
                var content = new StringBuilder();
                try
                {
                    using var sender = new SmtpClient(Option.Host)
                    {
                        Credentials = new NetworkCredential(Option.From, Option.Password)
                    };
                    if (Option.EnableSsl)
                    {
                        sender.EnableSsl = true;
                        sender.Port = Option.Port;
                    }
                    if (Option.Timeout > 2000) sender.Timeout = Option.Timeout;

                    var mail = new MailMessage(new MailAddress(Option.From, Option.DisplayName), new MailAddress(Option.To))
                    {
                        IsBodyHtml = true,
                        Subject = messages.First().Title
                    };

                    foreach (var msg in messages)
                    {
                        // 合并消息
                        content.AppendLine(msg.Message);
                    }

                    mail.Body = content.ToString();
                    await sender.SendMailAsync(mail);
                }
                catch (Exception ex)
                {
                    // 发送邮件失败
                    ex.Log();
                }
            }
        }
    }
}
