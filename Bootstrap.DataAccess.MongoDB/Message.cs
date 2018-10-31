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
        protected override IEnumerable<DataAccess.Message> RetrieveMessages(string userName)
        {
            var msg = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.Message>("Messages");
            return msg.Find(message => message.To == userName || message.From == userName).ToList();
        }
    }
}
