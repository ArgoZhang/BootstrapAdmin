using Bootstrap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Bootstrap.Controllers
{
    public class TestController : ApiController
    {
        // GET api/<controller>
        public Dummy Get(int limit, int offset, string name, string price, string sort, string order)
        {
            var p = new TerminalsModel();
            var ret = new List<TerminalsModel>();
            Random rnd = new Random();
            for (int index = 1; index < 1000; index++)
            {
                ret.Add(new TerminalsModel() { Id = index.ToString(), Name = string.Format("Argo-{0:D4}", index), Price = index.ToString() });
            }
            if (!string.IsNullOrEmpty(name)) ret = ret.Where(n => n.Name.Contains(name)).ToList();
            if (!string.IsNullOrEmpty(price)) ret = ret.Where(n => n.Price.Contains(price)).ToList();
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("Name")) { ret = order == "desc" ? ret.OrderByDescending(n => n.Name).ToList() : ret.OrderBy(n => n.Name).ToList(); }
                if (sort.Equals("Price")) { ret = order == "desc" ? ret.OrderByDescending(n => n.Price).ToList() : ret.OrderBy(n => n.Price).ToList(); }
            }
            var total = ret.Count;
            var rows = ret.Skip(offset).Take(limit).ToList();
            return new Dummy() { total = total, rows = rows };
        }

        public class Dummy
        {
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public string Post([FromBody]string value)
        {
            return "value";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        [System.Web.Mvc.HttpDelete]
        // DELETE api/<controller>/5
        public void Delete(int id)
        {

        }
    }
}