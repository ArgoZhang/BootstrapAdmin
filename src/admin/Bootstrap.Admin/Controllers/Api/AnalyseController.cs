using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AnalyseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult<AnalyseData> Get([FromQuery]string logType = "")
        {
            var ret = new AnalyseData();
            if (logType.Equals("LoginUsers", StringComparison.OrdinalIgnoreCase))
            {
                var loginUsers = LoginHelper.RetrieveAll(null, null, null);
                var temp = loginUsers.GroupBy(usr => usr.LoginTime.ToString("yyyy-MM-dd"));
                var max = temp.Any() ? temp.Max(g => g.Count()) : 1;
                ret.Polylines = temp.Select((g, index) =>
                {
                    ret.Datas.Add(new KeyValuePair<string, string>(g.Key, g.Count().ToString()));
                    return $"{index * 2},{Math.Round(g.Count() * 28.0 / max, 2)}";
                });
            }
            if (logType.Equals("trace", StringComparison.OrdinalIgnoreCase))
            {
                var loginUsers = TraceHelper.RetrieveAll(null, null, null);
                var temp = loginUsers.GroupBy(usr => usr.LogTime.ToString("yyyy-MM-dd"));
                var max = temp.Any() ? temp.Max(g => g.Count()) : 1;
                ret.Polylines = temp.Select((g, index) =>
                {
                    ret.Datas.Add(new KeyValuePair<string, string>(g.Key, g.Count().ToString()));
                    return $"{index * 5},{Math.Round(g.Count() * 28.0 / max, 2)}";
                });
            }
            if (logType.Equals("log", StringComparison.OrdinalIgnoreCase))
            {
                var loginUsers = LogHelper.RetrieveAll(null, null, null);
                var temp = loginUsers.GroupBy(usr => usr.LogTime.ToString("yyyy-MM-dd"));
                var max = temp.Any() ? temp.Max(g => g.Count()) : 1;
                ret.Polylines = temp.Select((g, index) =>
                {
                    ret.Datas.Add(new KeyValuePair<string, string>(g.Key, g.Count().ToString()));
                    return $"{index * 5},{Math.Round(g.Count() * 28.0 / max, 2)}";
                });
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public class AnalyseData
        {
            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<string> Polylines { get; set; } = new List<string>();

            /// <summary>
            /// 
            /// </summary>
            public List<KeyValuePair<string, string>> Datas { get; set; } = new List<KeyValuePair<string, string>>();
        }
    }
}
