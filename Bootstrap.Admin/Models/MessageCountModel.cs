using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bootstrap.Admin.Models
{
    public class MessageCountModel
    {
        public int inboxCount { get; set; }
        public int sendmailCount { get; set; }
        public int markCount { get; set; }
        public int trashCount { get; set; }
    }
}