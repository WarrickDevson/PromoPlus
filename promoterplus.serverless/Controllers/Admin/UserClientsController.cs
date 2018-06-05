using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using promoterplus.serverless.Models;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Controllers.Admin
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/UserClients")]
    public class UserClientsController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public UserClientsController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/UserClients
        [HttpGet]
        public IEnumerable<UserClient> GetUserclient()
        {
            return _context.UserClient;
        }

        // GET: api/UserClients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserclient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userclient = await _context.UserClient.SingleOrDefaultAsync(m => m.Id == id);

            if (userclient == null)
            {
                return NotFound();
            }

            return Ok(userclient);
        }

        // PUT: api/UserClients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserclient([FromRoute] int id, [FromBody] UserClient userClient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userClient.Id)
            {
                return BadRequest();
            }

            _context.Entry(userClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserclientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserClients
        [HttpPost]
        public async Task<IActionResult> PostUserclient([FromBody] UserClient userClient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserClient.Add(userClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserclient", new { id = userClient.Id }, userClient);
        }

        // DELETE: api/UserClients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserclient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userclient = await _context.UserClient.SingleOrDefaultAsync(m => m.Id == id);
            if (userclient == null)
            {
                return NotFound();
            }

            _context.UserClient.Remove(userclient);
            await _context.SaveChangesAsync();

            return Ok(userclient);
        }

        private bool UserclientExists(int id)
        {
            return _context.UserClient.Any(e => e.Id == id);
        }
    }
}