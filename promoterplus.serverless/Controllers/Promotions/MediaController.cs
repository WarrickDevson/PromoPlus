using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("api/v1.0/Medias")]
    public class MediasController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public MediasController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Medias
        [HttpGet]
        public IQueryable Get([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")]int end, [FromQuery(Name = "_sort")]string sort, [FromQuery(Name = "_order")]string order)
        {
            Response.Headers.Add("X-Total-Count", _context.Media.Count().ToString());
            return Sort(_context.Media.Include(a => a.ModifiedUser).Skip(start).Take(end - start), sort, order);
        }

        // GET: api/Medias/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var media = await _context.Media.SingleOrDefaultAsync(m => m.Id == id);

            if (media == null)
            {
                return NotFound();
            }

            return Ok(media);
        }

        // PUT: api/Medias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Media media)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != media.Id)
            {
                return BadRequest();
            }

            media.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            media.ModifiedDate = DateTime.Now;

            _context.Entry(media).State = EntityState.Modified;

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

            return Ok(media);
        }

        // POST: api/Medias
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file,int mediaTypeId)
        {
            Media media = new Media();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var context = _context)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        media.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                            .FindFirst(ClaimTypes.Sid).Value);
                        media.ModifiedDate = DateTime.Now;
                        media.MediaTypeId = mediaTypeId;
                        media.IsActive = true;
                        MemoryStream ms = new MemoryStream();
                        file.OpenReadStream().CopyTo(ms);
                        media.MediaContent = ms.ToArray();

                        _context.Media.Add(media);
                        await context.SaveChangesAsync();

                        _context.PromotionPromoterMedia.Add(new PromotionPromoterMedia
                        {
                            MediaId = media.Id,
                            PromotionPromoterId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                                .FindFirst(ClaimTypes.GroupSid).Value),
                            ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                                .FindFirst(ClaimTypes.Sid).Value),
                            ModifiedDate = DateTime.Now,
                            IsActive = true
                        });
                        await context.SaveChangesAsync();

                        // Commit transaction if all commands succeed, transaction will auto-rollback
                        // when disposed if either commands fails
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return BadRequest(e.Message);
                    }
                }

                return Ok(media.Id);
            }
        }

        // DELETE: api/Medias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var media = await _context.Media.SingleOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }

            media.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            media.ModifiedDate = DateTime.Now;

            media.IsActive = false;

            _context.Entry(media).State = EntityState.Modified;

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

            return Ok(media);
        }

        private bool Exists(int id)
        {
            return _context.Media.Any(e => e.Id == id);
        }
    }
}