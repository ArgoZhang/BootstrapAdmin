using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class ExceptionsController : ApiController
    {
        /// <summary>
        /// 显示所有异常
        /// </summary>
        /// <param name="value"></param>
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
            return Directory.GetFiles(filePath)
                .Where(f => Path.GetExtension(f).Equals(".log", System.StringComparison.OrdinalIgnoreCase))
                .Select(f => Path.GetFileNameWithoutExtension(f)).OrderByDescending(s => s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public string Put([FromBody]string fileName)
        {
            var logName = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/ErrorLog/{0}.log", fileName));
            if (!File.Exists(logName)) return "无此日志文件";
            StringBuilder sb = new StringBuilder();
            using (StreamReader reader = new StreamReader(logName))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Replace("<", "&lt;").Replace(">", "&gt;");
                    if (line == "General Information ") sb.AppendFormat("<h4><b>{0}</b></h4>", line);
                    else if (line.StartsWith("TimeStamp:")) sb.AppendFormat("<div class='logTs'>{0}</div>", line);
                    else if (line.EndsWith("Exception Information")) sb.AppendFormat("<div class='logExcep'>{0}</div>", line);
                    else if (line.StartsWith("Message:")) sb.AppendFormat("<div class='logMsg'>{0}</div>", line);
                    else if (line.StartsWith("ErrorSql:")) sb.AppendFormat("<div class='logSql'>{0}</div>", line);
                    else if (line.StartsWith("Exception Type: Longbow.Data.DBAccessException")) sb.AppendFormat("<div class='logDbExcep'>{0}</div>", line);
                    else if (line.StartsWith("StackTrace Information")) sb.AppendFormat("<b>{0}</b><br>", line);
                    else sb.AppendFormat("{0}<br>", line);
                };
            }
            return sb.ToString();
        }
    }
}
