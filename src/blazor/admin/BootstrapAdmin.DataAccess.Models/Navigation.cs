// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.

namespace BootstrapAdmin.DataAccess.Models
{
    /// <summary>
    /// Bootstrap Admin 后台管理菜单相关操作实体类
    /// </summary>
    public class Navigation
    {
        /// <summary>
        /// 获得/设置 菜单主键ID
        /// </summary>
        public string? Id { set; get; }

        /// <summary>
        /// 获得/设置 父级菜单ID 默认为 0
        /// </summary>
        public string ParentId { set; get; } = "0";

        /// <summary>
        /// 获得/设置 父级菜单名称
        /// </summary>
        public string ParentName { get; set; } = "";

        /// <summary>
        /// 获得/设置 菜单名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 获得/设置 菜单序号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 获得/设置 菜单图标
        /// </summary>
        public string Icon { get; set; } = "";

        /// <summary>
        /// 获得/设置 菜单URL地址
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// 获得/设置 菜单分类, 0 表示系统菜单 1 表示用户自定义菜单
        /// </summary>
        public string Category { get; set; } = "0";

        /// <summary>
        /// 获得 菜单分类名称，取字典表中的Name category="菜单"
        /// </summary>
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// 获得/设置 是否当前被选中 active为选中
        /// </summary>
        public string Active { get; set; } = "";

        /// <summary>
        /// 获得/设置 链接目标
        /// </summary>
        public string Target { get; set; } = "";

        /// <summary>
        /// 获得/设置 是否为资源文件, 0 表示菜单 1 表示资源 2 表示按钮
        /// </summary>
        public int IsResource { get; set; }

        /// <summary>
        /// 获得/设置 所属应用程序，此属性由BA后台字典表分配
        /// </summary>
        public string Application { get; set; } = "";

        /// <summary>
        /// 获得/设置 当前菜单项的所有子菜单集合
        /// </summary>
        public List<Navigation> Menus { get; } = new List<Navigation>();
    }
}
