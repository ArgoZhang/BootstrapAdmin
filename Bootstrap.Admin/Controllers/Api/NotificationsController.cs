using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        /// <summary>
        /// 
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
            var task = TaskHelper.RetrieveTasks();
            var tasksCount = task.Count();

            //Message
            var message = MessageHelper.RetrieveMessagesHeader(User.Identity.Name);
            var messagesCount = message.Count();

            message = message.Take(6);
            message.AsParallel().ForAll(m => m.FromIcon = Url.Content(m.FromIcon));

            //Apps
            var apps = ExceptionsHelper.RetrieveExceptions().Where(n => n.ExceptionType != "Longbow.Data.DBAccessException");
            var appExceptionsCount = apps.Count();

            apps = apps.Take(6);
            apps.AsParallel().ForAll(n =>
            {
                n.ExceptionType = n.ExceptionType.Split('.').Last();
                var ts = DateTime.Now - n.LogTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });

            //Dbs
            var dbs = ExceptionsHelper.RetrieveExceptions().Where(n => n.ExceptionType == "Longbow.Data.DBAccessException");
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