using Bootstrap.Client.Models;
using Longbow.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bootstrap.Client.Controllers
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
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Error(int id)
        {
            var uriBuilder = new UriBuilder(ConfigurationManager.AppSettings["AuthHost"]) { Query = QueryString.Create(CookieAuthenticationDefaults.ReturnUrlParameter, $"{Request.Scheme}://{Request.Host}{Request.PathBase}").ToString() };
            uriBuilder.Path = uriBuilder.Path == "/" ? Request.Path.Value : uriBuilder.Path + Request.Path.Value;
            return Redirect(uriBuilder.ToString());
        }
    }
}
