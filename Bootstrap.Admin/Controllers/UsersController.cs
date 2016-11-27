using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.Security.Principal;
using Newtonsoft.Json.Linq;
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
        /// <returns></returns>
        [HttpPut]
        public bool Put([FromBody]User value)
        {
            if (value.UserStatus == 9)
            {
                // vlaidate userName
                return UserHelper.RetrieveUsersByName(value.UserName) == null;
            }
            var ret = false;
            var userName = User.Identity.Name;
            if (value.UserName == userName && !LgbPrincipal.IsAdmin(userName))
            {
                if (value.UserStatus == 1)
                    ret = UserHelper.SaveUserInfoByName(value);
                else if (value.UserStatus == 2)
                    ret = UserHelper.ChangePassword(value);
            }
            return ret;
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
        public User Get(string userName)
        {
            return UserHelper.RetrieveUsersByName(userName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]User value)
        {
            value.Description = string.Format("管理员{0}创建用户", User.Identity.Name);
            value.ApprovedBy = User.Identity.Name;
            return UserHelper.SaveUser(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
                case "user":
                    // 此时 userIds 存储的信息是操作结果 1 标示同意 0 标示拒绝
                    var user = new User() { ID = id, UserStatus = 2 };
                    if (userIds == "1")
                    {
                        user.ApprovedBy = User.Identity.Name;
                    }
                    else
                    {
                        user.RejectedReason = "无原因";
                        user.RejectedBy = User.Identity.Name;
                    }
                    ret = UserHelper.SaveUser(user);
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