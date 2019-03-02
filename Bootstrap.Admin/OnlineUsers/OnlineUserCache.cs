using System;
using System.Threading;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class OnlineUserCache : IDisposable
    {
        private Timer dispatcher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        public OnlineUserCache(OnlineUser user, Action action)
        {
            User = user;
            dispatcher = new Timer(_ => action(), null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// 
        /// </summary>
        public OnlineUser User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            if (dispatcher != null) dispatcher.Change(TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
        }

        #region Impletement IDispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dispatcher != null)
                {
                    dispatcher.Dispose();
                    dispatcher = null;
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
        #endregion
    }
}
