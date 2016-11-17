using Bootstrap.DataAccess;
using Newtonsoft.Json.Linq;
using System.Web.Http;


namespace Bootstrap.Admin.Controllers
{
    public class ProfilesController : ApiController
    {
        // POST api/<controller>
        public bool Post([FromBody]JObject value)
        {
            //保存个性化设置
            dynamic json = value;
            return DictHelper.SaveProfiles((string)json.name, (string)json.code, (string)json.category);
        }
    }
}