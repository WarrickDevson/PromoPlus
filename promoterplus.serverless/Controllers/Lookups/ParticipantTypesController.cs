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
    [Route("api/v1.0/ParticipantTypes")]
    public class ParticipantTypesController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public ParticipantTypesController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/ParticipantTypes
        [HttpGet]
        public IQueryable Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            Response.Headers.Add("X-Total-Count", _context.ParticipantType.Count().ToString());
            return Sort(_context.ParticipantType.Include(a => a.ModifiedUser).Skip(start).Take(end - start), sort, order);
        }

        // GET: api/ParticipantTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participantType = await _context.ParticipantType.SingleOrDefaultAsync(m => m.Id == id);

            if (participantType == null)
            {
                return NotFound();
            }

            return Ok(participantType);
        }

        // PUT: api/ParticipantTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ParticipantType participantType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participantType.Id)
            {
                return BadRequest();
            }

            participantType.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            participantType.ModifiedDate = DateTime.Now;

            _context.Entry(participantType).State = EntityState.Modified;

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

            return Ok(participantType);
        }

        // POST: api/ParticipantTypes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ParticipantType participantType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var x = HttpContext.User.Identity.Name;

            participantType.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            participantType.ModifiedDate = DateTime.Now;

            _context.ParticipantType.Add(participantType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = participantType.Id }, participantType);
        }

        // DELETE: api/ParticipantTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participantType = await _context.ParticipantType.SingleOrDefaultAsync(m => m.Id == id);
            if (participantType == null)
            {
                return NotFound();
            }

            participantType.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            participantType.ModifiedDate = DateTime.Now;

            participantType.IsActive = false;

            _context.Entry(participantType).State = EntityState.Modified;

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

            return Ok(participantType);
        }

        private bool Exists(int id)
        {
            return _context.ParticipantType.Any(e => e.Id == id);
        }
    }
}