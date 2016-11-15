using System.IO;
using System.Web;
using Bootstrap.DataAccess;
using System.Web.Script.Serialization;
using System.Web.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Bootstrap.Admin.Controllers
{
    public class InfosController : ApiController
    {
        [HttpPost]
        public string Post()
        {
            bool ret = false;
            var msg = new { Result = ret, img_str = "" };

            var files = HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                string userName = HttpContext.Current.User.Identity.Name;
                if (userName.ToLower() != "argo" && userName.ToLower() != "test")
                {
                    string iconUrl = HttpContext.Current.Server.MapPath("~/Content/images/uploader/");
                    using (Stream inputStream = files[0].InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        Bitmap bmp = new Bitmap(memoryStream);
                        if (!Directory.Exists(iconUrl))
                            Directory.CreateDirectory(iconUrl);
                        string fileName = DateTime.Now.ToShortDateString().Replace("/", "") + DateTime.Now.Hour.ToString()
                        + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        + DateTime.Now.Millisecond.ToString() + userName + ".jpg";  //图片名称

                        bmp.Save(iconUrl + fileName, ImageFormat.Jpeg);    //保存图片

                        string headImg = DictHelper.RetrieveUrl() + fileName;
                        ret = UserHelper.SaveUserHeadImgByName(headImg, userName);
                        msg = new { Result = ret, img_str = headImg };
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(msg);
        }
    }
}