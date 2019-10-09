using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        /// <summary>
        /// 删除头像按钮调用
        /// </summary>
        /// <param name="id">Delete</param>
        /// <param name="env"></param>
        /// <param name="files">表单数据集合</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        [ButtonAuthorize(Url = "~/Admin/Profiles", Auth = "saveIcon")]
        public JsonResult Post(string id, [FromServices]IWebHostEnvironment env, [FromForm]DeleteFileCollection files)
        {
            if (!id.Equals("Delete", StringComparison.OrdinalIgnoreCase)) return new JsonResult(new object());

            var previewUrl = string.Empty;
            long fileSize = 0;
            var userName = User.Identity.Name;
            var fileName = files.Key;

            fileName = Path.Combine(env.WebRootPath, $"images{Path.DirectorySeparatorChar}uploader{Path.DirectorySeparatorChar}{fileName}");
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            fileName = "default.jpg";
            var webSiteUrl = DictHelper.RetrieveIconFolderPath();
            var filePath = Path.Combine(env.WebRootPath, webSiteUrl.Replace("~", string.Empty).Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar) + fileName);
            fileSize = new FileInfo(filePath).Length;
            var iconName = $"{fileName}?v={DateTime.Now.Ticks}";
            previewUrl = Url.Content($"{webSiteUrl}{iconName}");
            UserHelper.SaveUserIconByName(userName, iconName);

            return new JsonResult(new
            {
                initialPreview = new string[] { previewUrl },
                initialPreviewConfig = new object[] {
                    new { caption = "", size = fileSize, showZoom = true, key = "default.jpg" }
                },
                append = false
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public class DeleteFileCollection
        {
            /// <summary>
            /// 
            /// </summary>
            public string Key { get; set; }
        }

        /// <summary>
        /// 上传头像按钮调用
        /// </summary>
        /// <param name="env"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Profiles", Auth = "saveIcon")]
        public async Task<JsonResult> Post([FromServices]IWebHostEnvironment env, IFormCollection files)
        {
            var previewUrl = string.Empty;
            long fileSize = 0;
            var userName = User.Identity.Name;
            var fileName = string.Empty;
            if (files.Files.Count > 0)
            {
                var uploadFile = files.Files[0];
                var webSiteUrl = DictHelper.RetrieveIconFolderPath();
                fileName = $"{userName}{Path.GetExtension(uploadFile.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, webSiteUrl.Replace("~", string.Empty).Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar) + fileName);
                var fileFolder = Path.GetDirectoryName(filePath);
                fileSize = uploadFile.Length;
                if (!Directory.Exists(fileFolder)) Directory.CreateDirectory(fileFolder);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fs);
                }
                var iconName = $"{fileName}?v={DateTime.Now.Ticks}";
                previewUrl = Url.Content($"{webSiteUrl}{iconName}");
                UserHelper.SaveUserIconByName(userName, iconName);
            }
            return new JsonResult(new
            {
                initialPreview = new string[] { previewUrl },
                initialPreviewConfig = new object[] {
                    new { caption = "新头像", size = fileSize, showZoom = true, key = fileName }
                },
                append = false
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ButtonAuthorize(Url = "~/Admin/Profiles", Auth = "saveDisplayName,savePassword,saveApp,saveTheme")]
        public bool Put([FromBody]User value)
        {
            var ret = false;
            if (value.UserName.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (value.UserStatus == UserStates.ChangeTheme)
                    ret = UserHelper.SaveUserCssByName(value.UserName, value.Css);
                else if (value.UserStatus == UserStates.ChangeDisplayName)
                    ret = UserHelper.SaveDisplayName(value.UserName, value.DisplayName);
                else if (value.UserStatus == UserStates.ChangePassword)
                    ret = UserHelper.ChangePassword(value.UserName, value.Password, value.NewPassword);
                else if (value.UserStatus == UserStates.SaveApp)
                    ret = UserHelper.SaveApp(value.UserName, value.App);
            }
            return ret;
        }
    }
}
