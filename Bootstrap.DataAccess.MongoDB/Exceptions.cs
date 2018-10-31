using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Exceptions : DataAccess.Exceptions
    {
        private static void ClearExceptions()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                MongoDbAccessManager.Exceptions.DeleteMany(ex => ex.LogTime < DateTime.Now.AddDays(-7));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Exceptions> RetrieveExceptions()
        {
            return MongoDbAccessManager.Exceptions.Find(ex => ex.LogTime >= DateTime.Now.AddDays(-7)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public override bool Log(Exception ex, NameValueCollection additionalInfo)
        {
            var excep = new DataAccess.Exceptions
            {
                Id = null,
                AppDomainName = AppDomain.CurrentDomain.FriendlyName,
                ErrorPage = additionalInfo?["ErrorPage"],
                ExceptionType = ex.GetType().FullName,
                LogTime = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                UserId = additionalInfo?["UserId"],
                UserIp = additionalInfo?["UserIp"]
            };
            MongoDbAccessManager.Exceptions.InsertOne(excep);
            ClearExceptions();
            return true;
        }
    }
}
