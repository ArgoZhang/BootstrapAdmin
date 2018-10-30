using MongoDB.Driver;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    public class Message : DataAccess.Message
    {
        protected override IEnumerable<DataAccess.Message> RetrieveMessages(string userName)
        {
            //TODO: 完善其他字段信息
            var msg = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.Message>("Messages");
            return msg.Find(message => message.To == userName || message.From == userName).ToList();
        }
    }
}
