using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class RolesController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Role> Get([FromUri]QueryRoleOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Role> Post(int id, [FromBody]string value)
        {
            if (value == "user")
            {
                return RoleHelper.RetrieveRolesByUserId(id.ToString());
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut]
        public bool Put(int id, [FromBody]JObject value)
        {
            dynamic json = value;
            string roleIds = json.roleIds;
            if (json.type == "user")
                return RoleHelper.SaveRolesByUserId(id, roleIds);
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]Role value)
        {
            return RoleHelper.SaveRole(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            return RoleHelper.DeleteRole(value);
        }
    }
}
