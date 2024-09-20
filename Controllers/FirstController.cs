using Microsoft.AspNetCore.Mvc;
using swp_be.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstController : ControllerBase
    {
        private readonly ApplicationDBContext _DBcontext;
        public FirstController(ApplicationDBContext applicationDBContext)
        {
            _DBcontext = applicationDBContext;
        }
        // GET: api/<FirstController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FirstController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FirstController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FirstController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FirstController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
