using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using promoterplus.serverless.Models;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Lookups;
using promoterplus.serverless.Models.Promotions;
using static promoterplus.serverless.Helpers.GeneralHelper;

namespace promoterplus.serverless.Controllers.Promotions
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/Participants")]
    public class ParticipantsController : Controller
    {
        private readonly PromoterPlusContext _context;

        private readonly IConfiguration _configuration;

        public ParticipantsController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Participants
        [HttpGet]
        public async Task<IQueryable> GetAsync([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            //    Response.Headers.Add("X-Total-Count", _context.Participant.Count().ToString());
            //    return _context.Participant.Select(a=> new Participant {Id = a.Id, Description = a.Description,IsActive = a.IsActive, ModifiedUser = new User{Name = a.ModifiedUser.Name,Surname = a.ModifiedUser.Surname}}); 
            Response.Headers.Add("X-Total-Count", _context.Participant.Count().ToString());

            var participants =await _context.Participant.Select(a => new Participant
            {
                Id = a.Id,
                StartTime = a.StartTime,
                ParticipantType = new ParticipantType
                {
                    Description = a.ParticipantType.Description
                },
                Age = new Age
                {
                    Description = a.Age.Description
                },
                BuyingPower = new BuyingPower
                {
                    Description = a.BuyingPower.Description
                },
                Feedback = new Feedback
                {
                    Description = a.Feedback.Description
                },
                Gender = new Gender
                {
                    Description = a.Gender.Description
                },
                Product = new Product
                {
                    Label = a.Product.Label
                },
                Race = new Race
                {
                    Description = a.Race.Description
                },
                RepetitionType = new RepetitionType
                {
                    Description = a.RepetitionType.Description
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

            return Sort(participants.AsQueryable(), sort, order);
        }

        // GET: api/Participants/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participant = await _context.Participant.SingleOrDefaultAsync(m => m.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            return Ok(participant);
        }

        // PUT: api/Participants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participant.Id)
            {
                return BadRequest();
            }

            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
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

        // POST: api/Participants
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Participant participant)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var promotionPromoterId =
                    Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity).FindFirst(ClaimTypes.GroupSid).Value);
                var promotionPromoter =
                    await _context.PromotionPromoter.SingleOrDefaultAsync(a => a.Id == promotionPromoterId);

                participant.PromoterId = promotionPromoter.PromoterId;
                participant.PromotionId = promotionPromoter.PromotionId;

                participant.ModifiedUserId =
                    Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
                participant.ModifiedDate = DateTime.Now;
                participant.IsActive = true;

                _context.Participant.Add(participant);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new {id = participant.Id}, participant);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Participants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participant = await _context.Participant.SingleOrDefaultAsync(m => m.Id == id);
            if (participant == null)
            {
                return NotFound();
            }

            _context.Participant.Remove(participant);
            await _context.SaveChangesAsync();

            return Ok(participant);
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participant.Any(e => e.Id == id);
        }
    }
}