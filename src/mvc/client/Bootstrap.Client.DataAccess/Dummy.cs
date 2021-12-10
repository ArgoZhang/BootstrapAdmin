using PetaPoco;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 示例实体类
    /// </summary>
    [TableName("Dummy")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class Dummy
    {
        /// <summary>
        /// 获得/设置 数据库主键 ID 列值
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 数据库 Item1 列
        /// </summary>
        public string Item1 { get; set; } = "";

        /// <summary>
        /// 获得/设置 数据库 Item2 列
        /// </summary>
        public string Item2 { get; set; } = "";

        /// <summary>
        /// 获得/设置 数据库 Item3 列
        /// </summary>
        public int Item3 { get; set; }

        /// <summary>
        /// 通过当前用户名获得所有菜单
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Dummy> Retrieves()
        {
            using var db = DbManager.Create("client");
            return db.Fetch<Dummy>();
        }
    }
}
