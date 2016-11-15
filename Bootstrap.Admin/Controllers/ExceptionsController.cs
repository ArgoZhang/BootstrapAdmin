using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class ExceptionsController : ApiController
    {
        /// <summary>
        /// 显示所有异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Exceptions> Get([FromUri]QueryExceptionOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> Post()
        {
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog");
            return Directory.GetFiles(filePath).Select(f => Path.GetFileNameWithoutExtension(f)).OrderByDescending(s => s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public dynamic Put([FromBody]string fileName)
        {
            var logName = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/ErrorLog/{0}.log", fileName));
            if (!File.Exists(logName)) return new { content = string.Empty };
            using (StreamReader reader = new StreamReader(logName))
            {
                return new { content = reader.ReadToEnd().Replace("\r\n", "</br>") };
            }
        }
    }
}
