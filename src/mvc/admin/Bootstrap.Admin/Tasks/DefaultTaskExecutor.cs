// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Threading;
using System.Threading.Tasks;
using Longbow.Tasks;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 默认任务执行体实体类
    /// </summary>
    public class DefaultTaskExecutor : ITask
    {
        /// <summary>
        /// 任务执行方法
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Execute(CancellationToken cancellationToken) => Task.Delay(1000, cancellationToken);
    }
}
