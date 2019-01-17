using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<object> Get([FromQuery]QueryGroupOption value)
        {
            return value.RetrieveData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Group Get(string id)
        {
            return GroupHelper.Retrieves().FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]Group value)
        {
            return GroupHelper.Save(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return GroupHelper.Delete(value);
        }

        /// <summary>
        /// 获取部门授权
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<Group> Post(string id, [FromQuery]string type)
        {
            IEnumerable<Group> ret = new List<Group>();
            switch (type)
            {
                case "user":
                    ret = GroupHelper.RetrievesByUserId(id);
                    break;
                case "role":
                    ret = GroupHelper.RetrievesByRoleId(id);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 保存部门授权
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(string id, [FromBody]IEnumerable<string> groupIds, [FromQuery]string type)
        {
            var ret = false;
            switch (type)
            {
                case "user":
                    ret = GroupHelper.SaveByUserId(id, groupIds);
                    break;
                case "role":
                    ret = GroupHelper.SaveByRoleId(id, groupIds);
                    break;
            }
            return ret;
        }
    }
}
