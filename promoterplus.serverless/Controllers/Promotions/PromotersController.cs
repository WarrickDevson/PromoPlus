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
using promoterplus.serverless.Models.Promotions;
using static promoterplus.serverless.Helpers.GeneralHelper;

namespace promoterplus.serverless.Controllers.Promotions
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/Promoters")]
    public class PromotersController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public PromotersController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Promoters
        [HttpGet]
        public async Task<IQueryable> Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            Response.Headers.Add("X-Total-Count", _context.Promoter.Count().ToString());
            return Sort((await _context.Promoter.Include(a => a.ModifiedUser).Skip(start).Take(end - start).ToListAsync()).AsQueryable(), sort, order);
        }


        // GET: api/Promoters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promoter = await _context.Promoter.SingleOrDefaultAsync(m => m.Id == id);

            if (promoter == null)
            {
                return NotFound();
            }

            return Ok(promoter);
        }

        // PUT: api/Promoters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Promoter promoter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != promoter.Id)
            {
                return BadRequest();
            }

            promoter.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            promoter.ModifiedDate = DateTime.Now;

            _context.Entry(promoter).State = EntityState.Modified;

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

            return Ok(promoter);
        }

        // POST: api/Promoters
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Promoter promoter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var x = HttpContext.User.Identity.Name;

            promoter.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            promoter.ModifiedDate = DateTime.Now;

            _context.Promoter.Add(promoter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = promoter.Id }, promoter);
        }

        // DELETE: api/Promoters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promoter = await _context.Promoter.SingleOrDefaultAsync(m => m.Id == id);
            if (promoter == null)
            {
                return NotFound();
            }

            promoter.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            promoter.ModifiedDate = DateTime.Now;

            promoter.IsActive = false;

            _context.Entry(promoter).State = EntityState.Modified;

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

            return Ok(promoter);
        }

        private bool Exists(int id)
        {
            return _context.Promoter.Any(e => e.Id == id);
        }
    }
}