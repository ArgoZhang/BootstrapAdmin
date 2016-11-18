using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 消息表实体
    /// author:liuchun
    /// date:2016.11.12
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 消息主键 数据库自增
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发消息人
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 收消息人
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 消息状态：0-未读，1-已读 和Dict表的通知消息关联
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 标旗状态：0-未标旗，1-已标旗
        /// </summary>
        public int Mark { get; set; }
        /// <summary>
        /// 删除状态：0-未删除，1-已删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 消息标签：0-一般，1-紧要 和Dict表的消息标签关联
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 获得/设置 标签名称
        /// </summary>
        public string LabelName { get; set; }
        /// <summary>
        /// 获得/设置 时间描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }
        /// <summary>
        /// 获得/设置 发件人头像
        /// </summary>
        public string FromIcon { get; set; }
        /// <summary>
        /// 获得/设置 发件人昵称
        /// </summary>
        public string FromDisplayName { get; set; }
    }
}
