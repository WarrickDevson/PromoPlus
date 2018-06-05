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
    [Route("api/v1.0/RepetitionTypes")]
    public class RepetitionTypesController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public RepetitionTypesController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/RepetitionTypes
        [HttpGet]
        public IQueryable Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            Response.Headers.Add("X-Total-Count", _context.RepetitionType.Count().ToString());
            return Sort(_context.RepetitionType.Include(a => a.ModifiedUser).Skip(start).Take(end - start), sort, order);
        }

        // GET: api/RepetitionTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var repetitionType = await _context.RepetitionType.SingleOrDefaultAsync(m => m.Id == id);

            if (repetitionType == null)
            {
                return NotFound();
            }

            return Ok(repetitionType);
        }

        // PUT: api/RepetitionTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] RepetitionType repetitionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != repetitionType.Id)
            {
                return BadRequest();
            }

            repetitionType.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            repetitionType.ModifiedDate = DateTime.Now;

            _context.Entry(repetitionType).State = EntityState.Modified;

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

            return Ok(repetitionType);
        }

        // POST: api/RepetitionTypes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RepetitionType repetitionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var x = HttpContext.User.Identity.Name;

            repetitionType.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            repetitionType.ModifiedDate = DateTime.Now;

            _context.RepetitionType.Add(repetitionType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = repetitionType.Id }, repetitionType);
        }

        // DELETE: api/RepetitionTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var repetitionType = await _context.RepetitionType.SingleOrDefaultAsync(m => m.Id == id);
            if (repetitionType == null)
            {
                return NotFound();
            }

            repetitionType.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            repetitionType.ModifiedDate = DateTime.Now;

            repetitionType.IsActive = false;

            _context.Entry(repetitionType).State = EntityState.Modified;

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

            return Ok(repetitionType);
        }

        private bool Exists(int id)
        {
            return _context.RepetitionType.Any(e => e.Id == id);
        }
    }
}