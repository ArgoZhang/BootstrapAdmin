using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 根据配置文件动态加载不同数据库实体静态类
    /// </summary>
    public static class DbContextManager
    {
        /// <summary>
        /// 创建数据库实体类时发生异常实例
        /// </summary>
        public static Exception? Exception { get; private set; }

        /// <summary>
        /// 根据配置文件动态创建数据库实体类方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T? Create<T>() where T : class
        {
            T? t = default;
            try
            {
                Exception = null;
                t = Longbow.Data.DbContextManager.Create<T>();
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            return t;
        }
    }
}
