using Bootstrap.DataAccess;
using Longbow.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ProfilesController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> Post([FromServices]IHostingEnvironment env, IFormCollection files)
        {
            var previewUrl = string.Empty;
            long fileSize = 0;
            var userName = User.Identity.Name;
            var error = string.Empty;
            if (User.IsInRole("Administrators")) userName = "default";
            if (files.Count > 0)
            {
                var uploadFile = files.Files[0];
                var webSiteUrl = DictHelper.RetrieveIconFolderPath().Code;
                var fileName = string.Format("{0}{1}", userName, Path.GetExtension(uploadFile.FileName));
                var fileUrl = string.Format("{0}{1}", webSiteUrl, fileName);
                var filePath = Path.Combine(env.WebRootPath, webSiteUrl.Replace("~", string.Empty).Replace("/", "\\").TrimStart('\\') + fileName);
                var fileFolder = Path.GetDirectoryName(filePath);
                fileSize = uploadFile.Length;
                try
                {
                    if (!Directory.Exists(fileFolder)) Directory.CreateDirectory(fileFolder);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadFile.CopyToAsync(fs);
                    }
                    previewUrl = string.Format("{0}?q={1}", Url.Content(fileUrl), DateTime.Now.Ticks);
                    UserHelper.SaveUserIconByName(userName, fileName);
                }
                catch (Exception ex)
                {
                    var nv = new NameValueCollection
                    {
                        { "UpLoadFileName", filePath }
                    };
                    error = ex.Message;
                    ExceptionManager.Publish(ex, nv);
                }
            }
            return new JsonResult(new
            {
                error = string.IsNullOrEmpty(error) ? error : $"服务器端错误-{error}",
                initialPreview = new string[] { previewUrl },
                initialPreviewConfig = new object[] {
                    new { caption= "新头像", size= fileSize, showZoom= true }
                },
                append = false
            });
        }
    }
}