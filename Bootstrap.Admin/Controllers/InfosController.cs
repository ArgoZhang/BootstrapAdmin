using Bootstrap.DataAccess;
using Longbow.ExceptionManagement;
using Longbow.Security.Principal;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class InfosController : ApiController
    {
        [HttpPost]
        public string Post()
        {
            var ret = string.Empty;
            var userName = User.Identity.Name;
            if (LgbPrincipal.IsAdmin(userName)) userName = "default";
            var files = HttpContext.Current.Request.Files;
            if (files.Count > 0 && !LgbPrincipal.IsAdmin(userName))
            {
                var webSiteUrl = DictHelper.RetrieveIconFolderPath().Code;
                var fileName = string.Format("{0}{1}", userName, Path.GetExtension(files[0].FileName));
                var fileUrl = string.Format("{0}{1}", webSiteUrl, fileName);
                var filePath = HttpContext.Current.Server.MapPath(fileUrl);
                var fileFolder = Path.GetDirectoryName(filePath);
                try
                {
                    if (!Directory.Exists(fileFolder)) Directory.CreateDirectory(fileFolder);
                    files[0].SaveAs(filePath);
                    ret = string.Format("{0}?q={1}", Url.Content(fileUrl), DateTime.Now.Ticks);
                    UserHelper.SaveUserIconByName(userName, fileName);
                }
                catch (Exception ex)
                {
                    var nv = new NameValueCollection();
                    nv.Add("UpLoadFileName", filePath);
                    ExceptionManager.Publish(ex, nv);
                }
            }
            return ret;
        }
    }
}