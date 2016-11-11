using System;
namespace Bootstrap.DataAccess
{
    public class Notification
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime ProcessTime { get; set; }
        public string ProcessBy { get; set; }
        public string ProcessResult { get; set; }
        public string Status { get; set; }
    }
}
