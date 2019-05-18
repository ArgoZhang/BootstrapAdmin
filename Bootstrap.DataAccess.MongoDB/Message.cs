using MongoDB.Driver;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Message : DataAccess.Message
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected override IEnumerable<DataAccess.Message> Retrieves(string userName)
        {
            return DbManager.Messages.Find(message => message.To.ToLowerInvariant() == userName.ToLowerInvariant() || message.From.ToLowerInvariant() == userName.ToLowerInvariant()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.Message msg)
        {
            DbManager.Messages.InsertOne(msg);
            return true;
        }
    }
}
