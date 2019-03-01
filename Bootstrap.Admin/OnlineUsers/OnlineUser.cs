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
        private ConcurrentQueue<KeyValuePair<DateTime, string>> _requestUrls = new ConcurrentQueue<KeyValuePair<DateTime, string>>();

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FirstAccessTime { get; set; }

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
