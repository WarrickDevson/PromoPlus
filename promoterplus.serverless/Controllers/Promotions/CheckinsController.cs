using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using promoterplus.serverless.Models;
using promoterplus.serverless.Models.Promotions;

namespace promoterplus.serverless.Controllers.Promotions
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/Checkins")]
    public class CheckinsController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public CheckinsController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Checkins
        [HttpGet]
        public IEnumerable<Checkin> Get()
        {
            return _context.Checkin;
        }

        // GET: api/Checkins/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkin = await _context.Checkin.SingleOrDefaultAsync(m => m.Id == id);

            if (checkin == null)
            {
                return NotFound();
            }

            return Ok(checkin);
        }

        // PUT: api/Checkins/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Checkin checkin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != checkin.Id)
            {
                return BadRequest();
            }

            _context.Entry(checkin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckinExists(id))
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

        // POST: api/Checkins
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Checkin checkin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Checkin.Add(checkin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = checkin.Id }, checkin);
        }

        // DELETE: api/Checkins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkin = await _context.Checkin.SingleOrDefaultAsync(m => m.Id == id);
            if (checkin == null)
            {
                return NotFound();
            }

            _context.Checkin.Remove(checkin);
            await _context.SaveChangesAsync();

            return Ok(checkin);
        }

        private bool CheckinExists(int id)
        {
            return _context.Checkin.Any(e => e.Id == id);
        }
    }
}