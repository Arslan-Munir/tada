using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tada.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public ValuesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetValues()
        {
            var values = await _dataContext.Values.ToListAsync();

            return Ok(values);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _dataContext.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        // POST: api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/values/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/values/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}