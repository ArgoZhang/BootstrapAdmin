using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class TerminalsController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public TerminalsModel Get([FromUri]TerminalsPageOption value)
        {
            var ret = new TerminalsModel();
            ret.RetrieveTerminals(value);
            return ret;
        }

        // GET api/<controller>/5
        [HttpGet]
        public Terminal Get(int id)
        {
            return TerminalHelper.RetrieveTerminals(string.Empty).FirstOrDefault(t => t.ID == id);
        }

        // POST api/<controller>
        [HttpPost]
        public bool Post([FromBody]Terminal value)
        {
            return TerminalHelper.SaveTerminal(value);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public bool Put(int id, [FromBody]Terminal value)
        {
            return TerminalHelper.SaveTerminal(value);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            TerminalHelper.DeleteTerminal(value);
            return true;
        }
    }
}