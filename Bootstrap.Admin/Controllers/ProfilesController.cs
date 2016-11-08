using Bootstrap.DataAccess;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Bootstrap.Admin.Controllers
{
    public class ProfilesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            string sysName = DictHelper.RetrieveWebTitle();
            string footer = DictHelper.RetrieveWebFooter();
            return new string[] { sysName, footer };
        }

        // POST api/<controller>
        public bool Post([FromBody]JObject value)
        {
            //保存个性化设置
            dynamic json = value;
            return DictHelper.SaveProfiles((string)json.type, (string)json.dvalue);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}