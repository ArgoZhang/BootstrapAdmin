using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Longbow.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 系统通知控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        /// <summary>
        /// 后台 Header 状态条调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get()
        {
            if (!User.IsInRole("Administrators")) return false;

            // New Users
            var user = UserHelper.RetrieveNewUsers();
            var usersCount = user.Count();

            user = user.Take(6);
            user.AsParallel().ForAll(n =>
            {
                var ts = DateTime.Now - n.RegisterTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });

            // Tasks
            var task = TaskServicesManager.ToList().Where(s => s.NextRuntime != null).Select(s => new { s.Name, s.LastRuntime, s.LastRunResult });
            var tasksCount = task.Count();

            //Message
            var message = MessageHelper.Retrieves(User.Identity!.Name);
            var messagesCount = message.Count();

            message = message.Take(6);
            message.AsParallel().ForAll(m => m.FromIcon = Url.Content(m.FromIcon) ?? string.Empty);

            //Apps
            var apps = ExceptionsHelper.Retrieves().Where(n => n.Category != "DB");
            var appExceptionsCount = apps.Count();

            apps = apps.Take(6);
            apps.AsParallel().ForAll(n =>
            {
                n.ExceptionType = n.ExceptionType?.Split('.').Last();
                var ts = DateTime.Now - n.LogTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });

            //Dbs
            var dbs = ExceptionsHelper.Retrieves().Where(n => n.Category == "DB");
            var dbExceptionsCount = dbs.Count();

            dbs = dbs.Take(6);
            dbs.AsParallel().ForAll(n =>
            {
                var ts = DateTime.Now - n.LogTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });

            return new
            {
                NewUsersCount = usersCount,
                TasksCount = tasksCount,
                MessagesCount = messagesCount,
                AppExceptionsCount = appExceptionsCount,
                DbExceptionsCount = dbExceptionsCount,
                Users = user.Select(i => new { i.Period, i.UserName, i.DisplayName, i.Description }),
                Tasks = task.Take(6),
                Messages = message,
                Apps = apps.Select(n => new { n.ExceptionType, n.Message, n.Period }),
                Dbs = dbs.Select(n => new { n.ErrorPage, n.Message, n.Period })
            };
        }
    }
}
