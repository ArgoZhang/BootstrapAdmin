using System;
using System.Collections.Generic;

namespace Bootstrap.Client.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public class WebhookPostBody
    {
        /// <summary>
        /// 获得/设置 提交分支信息
        /// </summary>
        public string Ref { get; set; } = "";
    }

    /// <summary>
    /// Gitee 提交事件参数实体类
    /// </summary>
    public class GiteePushBody : WebhookPostBody
    {
        /// <summary>
        /// 获得/设置 提交信息集合
        /// </summary>
        public ICollection<GiteeCommit> Commits { get; set; } = new HashSet<GiteeCommit>();

        /// <summary>
        /// 获得/设置 提交信息数量
        /// </summary>
        public int Total_Commits_Count { get; set; }
    }

    /// <summary>
    /// 获得/设置 提交信息实体类
    /// </summary>
    public class GiteeCommit
    {
        /// <summary>
        /// 获得/设置 提交消息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 获得/设置 提交时间戳
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// 获得/设置 提交地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 获得/设置 提交作者
        /// </summary>
        public GiteeAuthor Author { get; set; } = new GiteeAuthor();
    }

    /// <summary>
    /// 获得/设置 提交作者信息
    /// </summary>
    public class GiteeAuthor
    {
        /// <summary>
        /// 获得/设置 提交时间
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// 获得/设置 提交人 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获得/设置 提交人名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 获得/设置 提交人邮件地址
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// 获得/设置 提交人名称
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 获得/设置 提交人 Gitee 地址
        /// </summary>
        public string Url { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class GiteeQueryBody
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string AllowBranchs { get; set; } = "master|dev";
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppveyorBuildPostBody
    {
        /// <summary>
        /// 
        /// </summary>
        public string AccountName { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string ProjectSlug { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string Branch { get; set; } = "";
    }
}
