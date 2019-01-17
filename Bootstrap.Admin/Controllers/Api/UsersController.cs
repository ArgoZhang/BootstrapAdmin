using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
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
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<object> Get([FromQuery]QueryUserOption value)
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
                    ret = UserHelper.SaveDisplayName(value.UserName, value.DisplayName);
                else if (value.UserStatus == UserStates.ChangePassword)
                    ret = UserHelper.ChangePassword(value.UserName, value.Password, value.NewPassword);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<object> Post(string id, [FromQuery]string type)
        {
            IEnumerable<object> ret = null;
            switch (type)
            {
                case "role":
                    ret = UserHelper.RetrievesByRoleId(id).Select(p => new
                    {
                        p.Id,
                        p.DisplayName,
                        p.UserName,
                        p.Checked
                    });
                    break;
                case "group":
                    ret = UserHelper.RetrievesByGroupId(id);
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 前台User View调用，新建/更新用户
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]User value)
        {
            var ret = false;
            if (string.IsNullOrEmpty(value.Id))
            {
                value.Description = string.Format("管理员{0}创建用户", User.Identity.Name);
                value.ApprovedBy = User.Identity.Name;
                value.ApprovedTime = DateTime.Now;
                ret = UserHelper.Save(value);
            }
            else
            {
                ret = UserHelper.Update(value.Id, value.Password, value.DisplayName);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(string id, [FromBody]IEnumerable<string> userIds, [FromQuery]string type)
        {
            var ret = false;
            switch (type)
            {
                case "role":
                    ret = UserHelper.SaveByRoleId(id, userIds);
                    break;
                case "group":
                    ret = UserHelper.SaveByGroupId(id, userIds);
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return UserHelper.Delete(value);
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