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
    [Route("api/v1.0/BuyingPowers")]
    public class BuyingPowersController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public BuyingPowersController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/BuyingPowers
        [HttpGet]
        public IQueryable Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            //    Response.Headers.Add("X-Total-Count", _context.Age.Count().ToString());
            //    return _context.Age.Select(a=> new Age {Id = a.Id, Description = a.Description,IsActive = a.IsActive, ModifiedUser = new User{Name = a.ModifiedUser.Name,Surname = a.ModifiedUser.Surname}}); 
            Response.Headers.Add("X-Total-Count", _context.BuyingPower.Count().ToString());
            return Sort(_context.BuyingPower.Include(a => a.ModifiedUser).Skip(start).Take(end - start), sort, order);
        }

        // GET: api/BuyingPowers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buyingPower = await _context.BuyingPower.SingleOrDefaultAsync(m => m.Id == id);

            if (buyingPower == null)
            {
                return NotFound();
            }

            return Ok(buyingPower);
        }

        // PUT: api/BuyingPowers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] BuyingPower buyingPower)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buyingPower.Id)
            {
                return BadRequest();
            }

            buyingPower.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            buyingPower.ModifiedDate = DateTime.Now;

            _context.Entry(buyingPower).State = EntityState.Modified;

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

            return Ok(buyingPower);
        }

        // POST: api/BuyingPowers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BuyingPower buyingPower)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var x = HttpContext.User.Identity.Name;

            buyingPower.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            buyingPower.ModifiedDate = DateTime.Now;

            _context.BuyingPower.Add(buyingPower);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = buyingPower.Id }, buyingPower);
        }

        // DELETE: api/BuyingPowers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buyingPower = await _context.BuyingPower.SingleOrDefaultAsync(m => m.Id == id);
            if (buyingPower == null)
            {
                return NotFound();
            }

            buyingPower.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            buyingPower.ModifiedDate = DateTime.Now;

            buyingPower.IsActive = false;

            _context.Entry(buyingPower).State = EntityState.Modified;

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

            return Ok(buyingPower);
        }

        private bool Exists(int id)
        {
            return _context.BuyingPower.Any(e => e.Id == id);
        }
    }
}