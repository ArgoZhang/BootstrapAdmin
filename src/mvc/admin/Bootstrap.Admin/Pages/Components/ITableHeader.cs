using System.Collections.Generic;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// ITableHeader 接口
    /// </summary>
    public interface ITableHeader
    {
        /// <summary>
        /// 获取绑定字段显示名称方法
        /// </summary>
        IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        /// <summary>
        /// 获取绑定字段显示名称方法
        /// </summary>
        string GetDisplayName();

        /// <summary>
        /// 获取绑定字段信息方法
        /// </summary>
        string GetFieldName();

        /// <summary>
        /// 获得/设置 是否允许排序 默认为 false
        /// </summary>
        bool Sort { get; set; }
    }
}
