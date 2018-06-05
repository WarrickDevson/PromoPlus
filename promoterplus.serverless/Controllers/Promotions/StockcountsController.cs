using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using promoterplus.serverless.Models;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Promotions;
using static promoterplus.serverless.Helpers.GeneralHelper;

namespace promoterplus.serverless.Controllers.Promotions
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/StockCounts")]
    public class StockCountsController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public StockCountsController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/StockCounts
        [HttpGet]
        public async Task<IQueryable> GetAsync([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")] int end,
            [FromQuery(Name = "_sort")] string sort, [FromQuery(Name = "_order")] string order)
        {
            Response.Headers.Add("X-Total-Count", _context.StockCount.Count().ToString());
            var stockCounts = await _context.StockCount.Select(a => new StockCount{
                    Id = a.Id,
                    Count = a.Count,
                    IsActive = a.IsActive,
                    ModifiedUser = new User
                    {
                        Name = a.ModifiedUser.Name,
                        Surname = a.ModifiedUser.Surname
                    },
                    Promoter =  new Promoter
                    {
                        Name = a.Promoter.Name
                    },
                    PromotionProduct = new PromotionProduct
                    {
                        Product = new Product
                        {
                            Label = a.PromotionProduct.Product.Label
                        },
                        Promotion = new Promotion
                        {
                            Client = new Client
                            {
                                Description = a.PromotionProduct.Promotion.Client.Description
                            },
                            Location = new Location
                            {
                                Label = a.PromotionProduct.Promotion.Location.Label
                            },
                        }
                    }
            }).Skip(start).Take(end - start).ToListAsync();

            return Sort(stockCounts.AsQueryable(), sort, order);
        }

        // GET: api/StockCounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockCount = await _context.StockCount.SingleOrDefaultAsync(m => m.Id == id);

            if (stockCount == null)
            {
                return NotFound();
            }

            return Ok(stockCount);
        }

        // PUT: api/StockCounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] StockCount stockCount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stockCount.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockCount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockCountExists(id))
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

        // POST: api/StockCounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StockCount stockCount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotionPromoterId =
                Convert.ToInt32(((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.GroupSid).Value);

            var promotionPromoter =
                await _context.PromotionPromoter.SingleOrDefaultAsync(a => a.Id == promotionPromoterId);

            var promotionProduct = await _context.PromotionProduct.SingleOrDefaultAsync(a =>
                a.ProductId == stockCount.ProductId && a.PromotionId == promotionPromoter.PromotionId);

            stockCount.PromotionProductId = promotionProduct.Id;
            stockCount.PromoterId = promotionPromoter.PromoterId;
            stockCount.ModifiedDate=DateTime.Now;
            stockCount.IsActive = true;
            stockCount.ModifiedUserId =
                Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);

            _context.StockCount.Add(stockCount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = stockCount.Id }, stockCount);
        }

        // DELETE: api/StockCounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockCount = await _context.StockCount.SingleOrDefaultAsync(m => m.Id == id);
            if (stockCount == null)
            {
                return NotFound();
            }

            _context.StockCount.Remove(stockCount);
            await _context.SaveChangesAsync();

            return Ok(stockCount);
        }

        private bool StockCountExists(int id)
        {
            return _context.StockCount.Any(e => e.Id == id);
        }
    }
}