// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Services.SMS
{
    /// <summary>
    /// 
    /// </summary>
    internal class AutoExpireValidateCode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="expires"></param>
        /// <param name="expiredCallback"></param>
        public AutoExpireValidateCode(string phone, string code, TimeSpan expires, Action<string> expiredCallback)
        {
            Phone = phone;
            Code = code;
            Expires = expires;
            ExpiredCallback = expiredCallback;
            RunAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        protected Action<string> ExpiredCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Expires { get; set; }

        private CancellationTokenSource? _tokenSource;

        private Task RunAsync() => Task.Run(() =>
        {
            _tokenSource = new CancellationTokenSource();
            if (!_tokenSource.Token.WaitHandle.WaitOne(Expires)) ExpiredCallback(Phone);
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public AutoExpireValidateCode Reset(string code)
        {
            Code = code;
            _tokenSource?.Cancel();
            RunAsync();
            return this;
        }
    }
}
