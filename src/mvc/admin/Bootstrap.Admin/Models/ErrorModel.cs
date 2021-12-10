namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorModel : ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string Image { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>

        public string Detail { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ErrorModel CreateById(int id)
        {
            var model = new ErrorModel
            {
                Id = id,
                Title = "服务器内部错误",
                Content = "服务器内部错误",
                Detail = "相关错误信息已经记录到日志中，请登录服务器或后台管理中查看",
                Image = "~/images/error_icon.png",
                ReturnUrl = "~/Admin/Index"
            };

            switch (id)
            {
                case 0:
                    model.Content = "未处理服务器内部错误";
                    break;
                case 404:
                    model.Title = "资源未找到";
                    model.Content = "请求资源未找到";
                    model.Image = "~/images/404_icon.png";
                    break;
                case 403:
                    model.Title = "未授权请求";
                    model.Content = "您的访问受限！";
                    model.Detail = "服务器拒绝处理您的请求！您可能没有访问此操作的权限";
                    break;
            }
            return model;
        }
    }
}
