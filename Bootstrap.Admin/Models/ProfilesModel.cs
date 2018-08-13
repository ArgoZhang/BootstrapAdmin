using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfilesModel : ThemeModel
    {
        /// <summary>
        /// 获得/设置 头像文件大小
        /// </summary>
        public long Size { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public ProfilesModel(ControllerBase controller) : base(controller)
        {
            // TODO: 找到MapPath方法
            var fileName = AppContext.BaseDirectory + Icon;
            if (File.Exists(fileName))
            {
                Size = new FileInfo(fileName).Length;
            }
        }
    }
}