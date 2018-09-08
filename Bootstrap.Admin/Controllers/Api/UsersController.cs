using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<User> Get(QueryUserOption value)
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
            if (User.IsInRole("Administrators")) return false;

            var ret = false;
            if (value.UserStatus == UserStates.ChangeTheme)
            {
                return UserHelper.SaveUserCssByName(value.UserName, value.Css);
            }
            if (value.UserName.Equals(User.Identity.Name, System.StringComparison.OrdinalIgnoreCase))
            {
                if (value.UserStatus == UserStates.ChangeDisplayName)
                    ret = BootstrapUser.SaveDisplayName(value.UserName, value.DisplayName);
                else if (value.UserStatus == UserStates.ChangePassword)
                    ret = BootstrapUser.ChangePassword(value.UserName, value.Password, value.NewPassword);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
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
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]User value)
        {
            value.Description = string.Format("管理员{0}创建用户", User.Identity.Name);
            value.ApprovedBy = User.Identity.Name;
            value.ApprovedTime = DateTime.Now;
            return UserHelper.SaveUser(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
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
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]IEnumerable<int> value)
        {
            return UserHelper.DeleteUser(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpOptions]
        public string Options()
        {
            return null;
        }
    }
}