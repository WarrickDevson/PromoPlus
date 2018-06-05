using System;
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
using promoterplus.serverless.Models.Promotions;
using static promoterplus.serverless.Helpers.GeneralHelper;

namespace promoterplus.serverless.Controllers.Promotions
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/Traffic")]
    public class TrafficsController : Controller
    {
        private readonly PromoterPlusContext _context;

        private readonly IConfiguration _configuration;

        public TrafficsController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Traffic
        [HttpGet]
        public async Task<IQueryable> GetAsync([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            //    Response.Headers.Add("X-Total-Count", _context.Participant.Count().ToString());
            //    return _context.Participant.Select(a=> new Participant {Id = a.Id, Description = a.Description,IsActive = a.IsActive, ModifiedUser = new User{Name = a.ModifiedUser.Name,Surname = a.ModifiedUser.Surname}}); 
            Response.Headers.Add("X-Total-Count", _context.Traffic.Count().ToString());
            var traffic = await _context.Traffic.Select(a => new Traffic
            {
                Id = a.Id,
                StartTime = a.StartTime,
                Age = new Age
                {
                    Description = a.Age.Description
                },
                Gender = new Gender
                {
                    Description = a.Gender.Description
                },
                BuyingPower = new BuyingPower
                {
                    Description = a.BuyingPower.Description
                },
                Race = new Race
                {
                    Description = a.Race.Description
                },
                Promotion = new Promotion
                {
                    Client = new Client
                    {
                        Description = a.Promotion.Client.Description
                    },
                    Location = new Location
                    {
                        Label = a.Promotion.Location.Label
                    },
                },
                Promoter = new Promoter
                {
                    Name = a.Promoter.Name
                },
                ModifiedUser = new User
                {
                    Name = a.ModifiedUser.Name,
                    Surname = a.ModifiedUser.Surname
                }
            }).Skip(start).Take(end - start).ToListAsync();

            return Sort(traffic.AsQueryable(), sort, order);
        }
    

        // GET: api/Traffics/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var traffic = await _context.Traffic.SingleOrDefaultAsync(m => m.Id == id);

            if (traffic == null)
            {
                return NotFound();
            }

            return Ok(traffic);
        }

        // PUT: api/Traffics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Traffic traffic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != traffic.Id)
            {
                return BadRequest();
            }

            _context.Entry(traffic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrafficExists(id))
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

        // POST: api/Traffics
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Traffic traffic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var promotionPromoterId =
                    Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.GroupSid).Value);
                var promotionPromoter =
                    await _context.PromotionPromoter.SingleOrDefaultAsync(a => a.Id == promotionPromoterId);

                traffic.PromoterId = promotionPromoter.PromoterId;
                traffic.PromotionId = promotionPromoter.PromotionId;

                traffic.ModifiedUserId =
                    Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
                traffic.ModifiedDate = DateTime.Now;
                traffic.IsActive = true;

                _context.Traffic.Add(traffic);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new { id = traffic.Id }, traffic);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Traffics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var traffic = await _context.Traffic.SingleOrDefaultAsync(m => m.Id == id);
            if (traffic == null)
            {
                return NotFound();
            }

            _context.Traffic.Remove(traffic);
            await _context.SaveChangesAsync();

            return Ok(traffic);
        }

        private bool TrafficExists(int id)
        {
            return _context.Traffic.Any(e => e.Id == id);
        }
    }
}