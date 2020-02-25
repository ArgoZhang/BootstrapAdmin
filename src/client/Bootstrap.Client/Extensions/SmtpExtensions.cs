using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
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

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="message"></param>
        /// <param name="webRootPath"></param>
        /// <returns></returns>
        public static string FormatHealths(this string message, string webRootPath)
        {
            var cate = new Dictionary<string, string>()
            {
                ["db"] = "数据库",
                ["file"] = "组件文件",
                ["mem"] = "内存",
                ["Gitee"] = "Gitee 接口",
                ["gc"] = "垃圾回收器",
                ["dotnet-runtime"] = "运行时",
                ["environment"] = "环境变量"
            };
            var state = new Dictionary<string, string>()
            {
                { "0", "不健康" },
                { "1", "亚健康" },
                { "2", "健康" }
            };
            var styleFile = System.IO.File.ReadAllText(Path.Combine(webRootPath, "html\\healths.html".ReplaceOSPlatformPath()));
            var itemTemplate = System.IO.File.ReadAllText(Path.Combine(webRootPath, "html\\item.html".ReplaceOSPlatformPath()));
            var trTemplate = "<tr><td>{0}</td><td>{1}</td></tr>";

            var sb = new StringBuilder();
            sb.Append(styleFile);
            var root = JsonDocument.Parse(message).RootElement;
            var items = root.GetProperty("Keys").EnumerateArray();
            foreach (var item in items)
            {
                var itemName = item.GetString();

                // 通过 itemName 读取检查项明细内容
                var data = root.GetProperty("Report").GetProperty("Entries").GetProperty(itemName);
                var rowData = new StringBuilder();
                foreach (var row in data.GetProperty("Data").EnumerateObject())
                {
                    rowData.AppendFormat(trTemplate, row.Name, row.Value.GetRawText().Replace("\\r\\n", "<br>").Replace("\\n", "<br>"));
                }
                sb.Append(string.Format(itemTemplate, cate[itemName], data.GetProperty("Duration").GetString(), state[data.GetProperty("Status").GetRawText()], rowData.ToString()));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 消息格式枚举
    /// </summary>
    public enum MessageFormat
    {
        /// <summary>
        /// 程序异常
        /// </summary>
        Exception,

        /// <summary>
        /// 健康检查结果
        /// </summary>
        Healths
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
                MailMessage? mail = null;
                foreach (var msg in messages)
                {
                    if (mail == null)
                    {
                        mail = new MailMessage(new MailAddress(msg.From, "Bootstrap Admin"), new MailAddress(msg.To))
                        {
                            Subject = msg.Title,
                            IsBodyHtml = true
                        };
                    }
                    if (sender == null)
                    {
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
                if (sender != null && mail != null)
                {
                    mail.Body = content.ToString();
                    await sender.SendMailAsync(mail);
                }
            }
        }
    }
}
