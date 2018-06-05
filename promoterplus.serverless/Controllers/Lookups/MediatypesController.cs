using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using promoterplus.serverless.Helpers;
using promoterplus.serverless.Models;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Lookups;
using static promoterplus.serverless.Helpers.GeneralHelper;

namespace promoterplus.serverless.Controllers.Lookups
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/Mediatypes")]
    public class MediatypesController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public MediatypesController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Mediatypes
        [HttpGet]
        public IQueryable Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            //    Response.Headers.Add("X-Total-Count", _context.Mediatype.Count().ToString());
            //    return _context.Mediatype.Select(a=> new Mediatype {Id = a.Id, Description = a.Description,IsActive = a.IsActive, ModifiedUser = new User{Name = a.ModifiedUser.Name,Surname = a.ModifiedUser.Surname}}); 
            Response.Headers.Add("X-Total-Count", _context.Mediatype.Count().ToString());
            return Sort(_context.Mediatype.Include(a => a.ModifiedUser).Skip(start).Take(end - start), sort, order);
        }

        // GET: api/Mediatypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mediatype = await _context.Mediatype.SingleOrDefaultAsync(m => m.Id == id);

            if (mediatype == null)
            {
                return NotFound();
            }

            return Ok(mediatype);
        }

        // PUT: api/Mediatypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Mediatype mediatype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mediatype.Id)
            {
                return BadRequest();
            }

            mediatype.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            mediatype.ModifiedDate = DateTime.Now;

            _context.Entry(mediatype).State = EntityState.Modified;

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

            return Ok(mediatype);
        }

        // POST: api/Mediatypes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Mediatype mediatype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mediatype.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            mediatype.ModifiedDate = DateTime.Now;

            _context.Mediatype.Add(mediatype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = mediatype.Id }, mediatype);
        }

        // DELETE: api/Mediatypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mediatype = await _context.Mediatype.SingleOrDefaultAsync(m => m.Id == id);
            if (mediatype == null)
            {
                return NotFound();
            }

            mediatype.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            mediatype.ModifiedDate = DateTime.Now;

            mediatype.IsActive = false;

            _context.Entry(mediatype).State = EntityState.Modified;

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

            return Ok(mediatype);
        }

        private bool Exists(int id)
        {
            return _context.Mediatype.Any(e => e.Id == id);
        }
    }
}