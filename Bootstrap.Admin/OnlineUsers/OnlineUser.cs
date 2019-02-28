using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Bootstrap.Admin
{

    /// <summary>
    /// 
    /// </summary>
    public class OnlineUser
    {
        private ConcurrentQueue<KeyValuePair<DateTime, string>> _requestUrls;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="userName"></param>
        public OnlineUser(string ip, string userName)
        {
            Ip = ip;
            UserName = userName;
            FirstAccessTime = DateTime.Now;
            LastAccessTime = DateTime.Now;
            _requestUrls = new ConcurrentQueue<KeyValuePair<DateTime, string>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FirstAccessTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<KeyValuePair<DateTime, string>> RequestUrls
        {
            get
            {
                return _requestUrls.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void AddRequestUrl(string url)
        {
            _requestUrls.Enqueue(new KeyValuePair<DateTime, string>(DateTime.Now, url));
            if (_requestUrls.Count > 5)
            {
                _requestUrls.TryDequeue(out _);
            }
        }
    }
}
