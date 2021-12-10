namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// DBLog 实体类
    /// </summary>
    public class DBLog : DataAccess.DBLog
    {
        /// <summary>
        /// 保存数据库脚本日志方法 MongoDB 无脚本
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.DBLog p) => true;
    }
}
