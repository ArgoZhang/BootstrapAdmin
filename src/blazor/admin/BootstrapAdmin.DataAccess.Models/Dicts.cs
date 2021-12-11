// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.

namespace BootstrapAdmin.DataAccess.Models
{
    /// <summary>
    /// 字典配置项
    /// </summary>
    public class Dicts
    {
        /// <summary>
        /// 获得/设置 字典主键 数据库自增列
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 字典分类
        /// </summary>
        public string Category { get; set; } = "";

        /// <summary>
        /// 获得/设置 字典名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 获得/设置 字典字典值
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 获得/设置 字典定义值 0 表示系统使用，1 表示用户自定义 默认为 1
        /// </summary>
        public int Define { get; set; } = 1;
    }
}
