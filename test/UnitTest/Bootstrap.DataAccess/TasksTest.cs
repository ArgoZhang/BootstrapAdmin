// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class TasksTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            TaskHelper.Save(new Task() { TaskName = "UnitTest", AssignName = "User", UserName = "Admin", TaskTime = 0, TaskProgress = 20, AssignTime = DateTime.Now });
            Assert.NotEmpty(TaskHelper.Retrieves());
        }
    }
}
