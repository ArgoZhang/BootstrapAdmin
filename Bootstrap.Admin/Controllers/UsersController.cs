using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<User> Get([FromUri]QueryUserOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<User> Post(int id, [FromBody]JObject value)
        {
            var ret = new List<User>();
            dynamic json = value;
            switch ((string)json.type)
            {
                case "role":
                    ret = UserHelper.RetrieveUsersByRoleId(id).ToList();
                    break;
                case "group":
                    ret = UserHelper.RetrieveUsersByGroupId(id).ToList();
                    break;
                default:
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public User Get(int id)
        {
            return UserHelper.RetrieveUsers().FirstOrDefault(t => t.ID == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]User value)
        {
            value.ApprovedTime = DateTime.Now;
            value.Description = string.Format("管理员{0}创建用户", User.Identity.Name);
            return UserHelper.SaveUser(value);
        }

        [HttpPut]
        public bool Put(int id, [FromBody]JObject value)
        {
            var ret = false;
            dynamic json = value;
            string userIds = json.userIds;
            switch ((string)json.type)
            {
                case "role":
                    ret = UserHelper.SaveUsersByRoleId(id, userIds);
                    break;
                case "group":
                    ret = UserHelper.SaveUsersByGroupId(id, userIds);
                    break;
                default:
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            return UserHelper.DeleteUser(value);
        }
    }
}