// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 个人中心控制器
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
        public JsonResult Post(string id, [FromServices] IWebHostEnvironment env, [FromForm] DeleteFileCollection files)
        {
            if (!id.Equals("Delete", StringComparison.OrdinalIgnoreCase))
            {
                return new JsonResult(new object());
            }
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return new JsonResult(new object());
            }

            // check file
            var fileName = files.Key;
            if (!CheckFileName(userName, fileName))
            {
                return new JsonResult(new object());
            }
            fileName = Path.Combine(env.WebRootPath, $"images{Path.DirectorySeparatorChar}uploader{Path.DirectorySeparatorChar}{fileName}");
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            fileName = "default.jpg";
            var webSiteUrl = DictHelper.RetrieveIconFolderPath();
            var filePath = Path.Combine(env.WebRootPath, webSiteUrl.Replace("~", string.Empty).Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar) + fileName);
            var fileSize = new FileInfo(filePath).Length;
            var iconName = $"{fileName}?v={DateTime.Now.Ticks}";
            var previewUrl = Url.Content($"{webSiteUrl}{iconName}") ?? string.Empty;
            if (!string.IsNullOrEmpty(userName)) UserHelper.SaveUserIconByName(userName, iconName);

            return new JsonResult(new
            {
                initialPreview = new string[] { previewUrl },
                initialPreviewConfig = new object[] {
                    new { caption = "", size = fileSize, showZoom = true, key = "default.jpg" }
                },
                append = false
            });
        }

        private static bool CheckFileName(string userName, string fileName)
        {
            var ret = false;
            var user = UserHelper.RetrieveUserByUserName(userName);
            var icon = user?.Icon;
            if (!string.IsNullOrEmpty(icon))
            {
                var file = icon.SpanSplit("?").FirstOrDefault();
                ret = fileName == file;
            }
            return ret;
        }

        /// <summary>
        /// 待删除文件集合类
        /// </summary>
        public class DeleteFileCollection
        {
            /// <summary>
            /// 获得/设置 文件名称
            /// </summary>
            public string Key { get; set; } = "";
        }

        /// <summary>
        /// 上传头像按钮调用
        /// </summary>
        /// <param name="env"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Profiles", Auth = "saveIcon")]
        public async Task<JsonResult> Post([FromServices] IWebHostEnvironment env, IFormCollection files)
        {
            string? previewUrl = null;
            long fileSize = 0;
            var userName = User.Identity!.Name;
            var fileName = string.Empty;
            if (files.Files.Count > 0)
            {
                var uploadFile = files.Files[0];
                var webSiteUrl = DictHelper.RetrieveIconFolderPath();
                if (webSiteUrl.StartsWith('~'))
                {
                    fileName = $"{userName}{Path.GetExtension(uploadFile.FileName)}";
                    var filePath = Path.Combine(env.WebRootPath, webSiteUrl.Replace("~", string.Empty).Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar) + fileName);
                    fileSize = uploadFile.Length;
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadFile.CopyToAsync(fs);
                    }
                    var iconName = $"{fileName}?v={DateTime.Now.Ticks}";
                    previewUrl = Url.Content($"{webSiteUrl}{iconName}");
                    if (!string.IsNullOrEmpty(userName)) UserHelper.SaveUserIconByName(userName, iconName);
                }
            }
            return new JsonResult(new
            {
                initialPreview = new string[] { previewUrl ?? string.Empty },
                initialPreviewConfig = new object[] {
                    new { caption = "新头像", size = fileSize, showZoom = true, key = fileName }
                },
                append = false
            });
        }

        /// <summary>
        /// 个人中心操作方法 更改样式 更改显示名称 更改默认应用
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ButtonAuthorize(Url = "~/Admin/Profiles", Auth = "saveDisplayName,savePassword,saveApp,saveTheme")]
        public bool Put([FromBody] User value)
        {
            var ret = false;
            if (value.UserName.Equals(User.Identity!.Name, StringComparison.OrdinalIgnoreCase))
            {
                ret = value.UserStatus switch
                {
                    UserStates.ChangeTheme => UserHelper.SaveUserCssByName(value.UserName, value.Css),
                    UserStates.ChangeDisplayName => UserHelper.SaveDisplayName(value.UserName, value.DisplayName),
                    UserStates.ChangePassword => UserHelper.ChangePassword(value.UserName, value.Password, value.NewPassword),
                    UserStates.SaveApp => UserHelper.SaveApp(value.UserName, value.App),
                    _ => false
                };
            }
            return ret;
        }
    }
}
