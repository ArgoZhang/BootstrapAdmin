using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var url = DictHelper.RetrieveHomeUrl();
            return url.Equals("~/Home/Index", System.StringComparison.OrdinalIgnoreCase) ? (IActionResult)View(new HeaderBarModel(User.Identity)) : Redirect(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Error(int id)
        {
            var returnUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].ToString();
            var model = new ErrorModel() { ReturnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Content("~/Home/Index") : returnUrl };
            model.Title = "服务器内部错误";
            model.Content = "服务器内部错误";
            model.Image = "error_icon.png";
            if (id == 0)
            {
                model.Content = "未处理服务器内部错误";
            }
            else if (id == 404)
            {
                model.Title = "资源未找到";
                model.Content = "请求资源未找到";
                model.Image = "404_icon.png";
            }
            else if(id == 403)
            {
                model.Title = "拒绝响应";
                model.Content = "请求资源的访问被服务器拒绝";
            }
            return View(model);
        }
    }
}