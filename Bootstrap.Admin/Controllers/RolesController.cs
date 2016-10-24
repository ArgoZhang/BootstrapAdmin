using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
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
        /// <returns></returns>
        [HttpGet]
        public Role Get(int id)
        {
            return RoleHelper.RetrieveRole().FirstOrDefault(t => t.ID == id);
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
