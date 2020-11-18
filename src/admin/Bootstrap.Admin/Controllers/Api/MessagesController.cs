using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        /// <summary>
        /// 根据Id返回不同的消息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IEnumerable<Message> Get(string id)
        {
            var ret = new List<Message>();
            switch (id)
            {
                case "inbox":
                    ret = MessageHelper.Inbox(User.Identity!.Name).ToList();
                    break;
                case "sendmail":
                    ret = MessageHelper.SendMail(User.Identity!.Name).ToList();
                    break;
                case "mark":
                    ret = MessageHelper.Mark(User.Identity!.Name).ToList();
                    break;
                case "trash":
                    ret = MessageHelper.Trash(User.Identity!.Name).ToList();
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 返回各个消息列表的文件个数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageCountModel Get()
        {
            var mcm = new MessageCountModel
            {
                InboxCount = MessageHelper.Inbox(User.Identity!.Name).Count(),
                SendmailCount = MessageHelper.SendMail(User.Identity!.Name).Count(),
                MarkCount = MessageHelper.Mark(User.Identity!.Name).Count(),
                TrashCount = MessageHelper.Trash(User.Identity!.Name).Count()
            };
            return mcm;
        }
    }
}
