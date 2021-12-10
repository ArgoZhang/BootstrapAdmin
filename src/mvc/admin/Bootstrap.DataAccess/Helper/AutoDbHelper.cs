namespace Bootstrap.DataAccess.Helper
{
    /// <summary>
    /// 数据库自动生成帮助类
    /// </summary>
    public static class AutoDbHelper
    {
        /// <summary>
        /// 数据库检查方法
        /// </summary>
        public static void EnsureCreated(string folder) => DbContextManager.Create<AutoDB>()?.EnsureCreated(folder);
    }
}
