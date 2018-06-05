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
    [Route("api/v1.0/Ages")]
    public class AgesController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public AgesController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Ages
        [HttpGet]
        public IQueryable Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            //    Response.Headers.Add("X-Total-Count", _context.Age.Count().ToString());
            //    return _context.Age.Select(a=> new Age {Id = a.Id, Description = a.Description,IsActive = a.IsActive, ModifiedUser = new User{Name = a.ModifiedUser.Name,Surname = a.ModifiedUser.Surname}}); 
            Response.Headers.Add("X-Total-Count", _context.Age.Count().ToString());
            return Sort(_context.Age.Include(a => a.ModifiedUser).Skip(start).Take(end - start),sort,order);
        }

        // GET: api/Ages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var age = await _context.Age.SingleOrDefaultAsync(m => m.Id == id);

            if (age == null)
            {
                return NotFound();
            }

            return Ok(age);
        }

        // PUT: api/Ages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Age age)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != age.Id)
            {
                return BadRequest();
            }

            age.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            age.ModifiedDate = DateTime.Now;

            _context.Entry(age).State = EntityState.Modified;

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

            return Ok(age);
        }

        // POST: api/Ages
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Age age)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            age.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            age.ModifiedDate = DateTime.Now;

            _context.Age.Add(age);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = age.Id }, age);
        }

        // DELETE: api/Ages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var age = await _context.Age.SingleOrDefaultAsync(m => m.Id == id);
            if (age == null)
            {
                return NotFound();
            }

            age.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            age.ModifiedDate = DateTime.Now;

            age.IsActive = false;

            _context.Entry(age).State = EntityState.Modified;

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

            return Ok(age);
        }

        private bool Exists(int id)
        {
            return _context.Age.Any(e => e.Id == id);
        }
    }
}