namespace Bootstrap.Client.Tasks
{
    /// <summary>
    /// 自动发布配置类
    /// </summary>
    public class DeployOptions
    {
        /// <summary>
        /// 获得/设置 是否启用自动部署功能
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获得/设置 自动部署脚本文件
        /// </summary>
        public string DeployFile { get; set; }

        /// <summary>
        /// 获得/设置 自动部署分支
        /// </summary>
        public string Branch { get; set; } = "release";

        /// <summary>
        /// 获得/设置 自动部署平台
        /// </summary>
        public string OSPlatform { get; set; } = "Windows";
    }
}
