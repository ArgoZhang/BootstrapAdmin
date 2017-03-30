using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Role> Post(int id, [FromBody]JObject value)
        {
            var ret = new List<Role>();
            dynamic json = value;
            switch ((string)json.type)
            {
                case "user":
                    ret = RoleHelper.RetrieveRolesByUserId(id).ToList();
                    break;
                case "group":
                    ret = RoleHelper.RetrieveRolesByGroupId(id).ToList();
                    break;
                case "menu":
                    ret = RoleHelper.RetrieveRolesByMenuId(id).ToList();
                    break;
                default:
                    break;
            }
            return ret;
        }

        /// <summary>根据GroupID获取
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public bool Put(int id, [FromBody]JObject value)
        {
            var ret = false;
            dynamic json = value;
            string roleIds = json.roleIds;
            switch ((string)json.type)
            {
                case "user":
                    ret = RoleHelper.SaveRolesByUserId(id, roleIds);
                    break;
                case "group":
                    ret = RoleHelper.SaveRolesByGroupId(id, roleIds);
                    break;
                case "menu":
                    ret = RoleHelper.SavaRolesByMenuId(id, roleIds);
                    break;
                default:
                    break;
            }
            return ret;
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
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            return RoleHelper.DeleteRole(value);
        }
    }
}
