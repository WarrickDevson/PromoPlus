using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using promoterplus.serverless.Models;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Lookups;
using static promoterplus.serverless.Helpers.GeneralHelper;

namespace promoterplus.serverless.Controllers.Lookups
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/Races")]
    public class RacesController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public RacesController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Races
        [HttpGet]
        public IQueryable Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            Response.Headers.Add("X-Total-Count", _context.Race.Count().ToString());
            return Sort(_context.Race.Include(a => a.ModifiedUser).Skip(start).Take(end - start), sort, order);
        }

        // GET: api/Races/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var race = await _context.Race.SingleOrDefaultAsync(m => m.Id == id);

            if (race == null)
            {
                return NotFound();
            }

            return Ok(race);
        }

        // PUT: api/Races/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Race race)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != race.Id)
            {
                return BadRequest();
            }

            race.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            race.ModifiedDate = DateTime.Now;

            _context.Entry(race).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(race);
        }

        // POST: api/Races
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Race race)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var x = HttpContext.User.Identity.Name;

                race.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
                race.ModifiedDate = DateTime.Now;

                _context.Race.Add(race);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                var x = e;
            }

            return CreatedAtAction("Get", new { id = race.Id }, race);
        }

        // DELETE: api/Races/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var race = await _context.Race.SingleOrDefaultAsync(m => m.Id == id);
            if (race == null)
            {
                return NotFound();
            }

            race.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            race.ModifiedDate = DateTime.Now;

            race.IsActive = false;

            _context.Entry(race).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(race);
        }

        private bool Exists(int id)
        {
            return _context.Race.Any(e => e.Id == id);
        }
    }
}