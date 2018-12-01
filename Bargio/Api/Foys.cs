using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bargio.Api
{
    [Route("api/[controller]")]
    public class Foys : Controller
    {
        private ApplicationDbContext _context;

        public Foys(ApplicationDbContext context) {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get() {
            dynamic userData = _context.UserData.Select(o => new {o.UserName, o.HorsFoys, o.Solde}).ToList();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(DateTime id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
    }
}
