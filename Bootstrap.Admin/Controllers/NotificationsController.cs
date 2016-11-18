using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace Bootstrap.Admin.Controllers
{
    public class NotificationsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Notifications Get()
        {
            var ret = new Notifications();
            // New Users
            var user = UserHelper.RetrieveNewUsers();
            ret.Users = user.Take(6).OrderByDescending(u => u.RegisterTime).ToList();
            ret.Users.AsParallel().ForAll(n =>
              {
                  var ts = DateTime.Now - n.RegisterTime;
                  if (ts.TotalMinutes < 5) n.Period = "刚刚";
                  else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                  else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                  else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
              });
            ret.NewUsersCount = user.Count();

            // Tasks
            var task = TaskHelper.RetrieveTasks();
            ret.Tasks = task.Take(6).OrderByDescending(u => u.AssignTime).ToList();
            ret.TasksCount = task.Count();

            //Message
            var message = MessageHelper.RetrieveMessagesHeader(User.Identity.Name);
            ret.Messages = message.Take(6).ToList();
            ret.MessagesCount = message.Count();

            //Apps
            var apps = ExceptionHelper.RetrieveExceptions().Where(n => n.ExceptionType != "Longbow.Data.DBAccessException");
            ret.Apps = apps.Take(6).OrderByDescending(a => a.LogTime).ToList();
            ret.Apps.AsParallel().ForAll(n =>
            {
                var ts = DateTime.Now - n.LogTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });
            ret.AppExceptionsCount = apps.Count();

            //Dbs
            var dbs = ExceptionHelper.RetrieveExceptions().Where(n => n.ExceptionType == "Longbow.Data.DBAccessException");
            ret.Dbs = dbs.Take(6).OrderByDescending(d => d.LogTime).ToList();
            ret.Dbs.AsParallel().ForAll(n =>
            {
                var ts = DateTime.Now - n.LogTime;
                if (ts.TotalMinutes < 5) n.Period = "刚刚";
                else if (ts.Days > 0) n.Period = string.Format("{0}天", ts.Days);
                else if (ts.Hours > 0) n.Period = string.Format("{0}小时", ts.Hours);
                else if (ts.Minutes > 0) n.Period = string.Format("{0}分钟", ts.Minutes);
            });
            ret.DbExceptionsCount = dbs.Count();

            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public Notifications Get(string id)
        {
            var ret = new Notifications();
            if (id == "newusers" || id == "all") ret.Users = UserHelper.RetrieveNewUsers().OrderByDescending(u => u.RegisterTime).ToList();
            else if (id == "apps" || id == "all") ret.Apps = ExceptionHelper.RetrieveExceptions().Where(n => n.ExceptionType != "Longbow.Data.DBAccessException").OrderByDescending(a => a.LogTime).ToList();
            else if (id == "dbs" || id == "all") ret.Dbs = ExceptionHelper.RetrieveExceptions().Where(n => n.ExceptionType == "Longbow.Data.DBAccessException").OrderByDescending(d => d.LogTime).ToList();
            return ret;
        }

        public class Notifications
        {
            public Notifications()
            {
                Users = new List<User>();
                Apps = new List<Exceptions>();
                Dbs = new List<Exceptions>();
                Tasks = new List<Task>();
                Messages = new List<Message>();
            }
            /// <summary>
            /// 
            /// </summary>
            public List<User> Users { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Exceptions> Apps { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Exceptions> Dbs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Task> Tasks { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Message> Messages { get; set; }
            /// <summary>
            /// 获得/设置 消息数量
            /// </summary>
            public int MessagesCount { get; set; }
            /// <summary>
            /// 获得/设置 新用户数量
            /// </summary>
            public int NewUsersCount { get; set; }
            /// <summary>
            /// 获取/设置 任务数量
            /// </summary>
            public int TasksCount { get; set; }
            /// <summary>
            /// 获取/设置 应用程序错误数量
            /// </summary>
            public int AppExceptionsCount { get; set; }
            /// <summary>
            /// 获取/设置 数据库错误数量
            /// </summary>
            public int DbExceptionsCount { get; set; }
        }
    }
}