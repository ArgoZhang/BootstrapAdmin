using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class MenusController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Menu> Get([FromUri]QueryMenuOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Menu Get(int id)
        {
            return MenuHelper.RetrieveMenus().FirstOrDefault(t => t.ID == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]Menu value)
        {
            return MenuHelper.SaveMenu(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            return MenuHelper.DeleteMenu(value);
        }
    }
}