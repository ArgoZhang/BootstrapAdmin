using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Web.Http;
using System.Linq;

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
        public UserEntity Get([FromUri]TerminalsPageOption value)
        {
            var ret = new UserEntity();
            ret.RetrieveUsers(value);
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
            return UserHelper.SaveUser(value);
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