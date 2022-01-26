// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageBody
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}", Category, Message);
        }
    }
}
