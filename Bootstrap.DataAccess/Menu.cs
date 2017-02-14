using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 获得/设置 菜单主键ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 获得/设置 父级菜单ID
        /// </summary>
        public int ParentId { set; get; }
        /// <summary>
        /// 获得/设置 父级菜单名称
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 获得/设置 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获得/设置 菜单序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 获得/设置 菜单图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 获得/设置 菜单URL地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 获得/设置 菜单分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 获得 菜单分类名称，取字典表中的Name category="菜单"
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 获得/设置 是否当前被选中 active为选中
        /// </summary>
        public string Active { get; set; }
        /// <summary>
        /// 获得/设置 链接目标
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 获得/设置 是否为资源文件
        /// </summary>
        public int IsResource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Menu> Menus { get; set; }
    }
}
