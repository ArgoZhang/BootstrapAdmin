﻿using Bootstrap.Admin.Models;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public UserModel Get()
        {
            return new UserModel()
            {
            };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}