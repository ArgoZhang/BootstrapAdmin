using Bootstrap.Security;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// Group 实体类
    /// </summary>
    internal class Group : BootstrapGroup
    {
        /// <summary>
        /// 获得/设置 当前组授权角色数据集合
        /// </summary>
        public IEnumerable<string> Roles { get; set; } = new string[0];
    }
}
