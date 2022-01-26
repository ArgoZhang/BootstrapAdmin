// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageCountModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int InboxCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SendmailCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MarkCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TrashCount { get; set; }
    }
}