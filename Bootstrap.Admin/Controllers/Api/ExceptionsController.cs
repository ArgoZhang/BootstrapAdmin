using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ExceptionsController : Controller
    {
        /// <summary>
        /// 显示所有异常
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Exceptions> Get(QueryExceptionOption value)
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
            var filePath = Path.Combine(AppContext.BaseDirectory, "Error");
            if (!Directory.Exists(filePath)) return new List<string>();
            return Directory.GetFiles(filePath)
                .Where(f => Path.GetExtension(f).Equals(".log", StringComparison.OrdinalIgnoreCase))
                .Select(f => Path.GetFileNameWithoutExtension(f)).OrderByDescending(s => s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public JsonResult Put([FromBody]ExceptionFileQuery exceptionFile)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Error");
            var logName = $"{Path.Combine(filePath, exceptionFile.FileName)}.log";
            if (!System.IO.File.Exists(logName)) return new JsonResult("无此日志文件");
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
            return new JsonResult(sb.ToString());
        }

        public class ExceptionFileQuery
        {
            public string FileName { get; set; }
        }
    }
}
