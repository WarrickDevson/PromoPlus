using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/v1.0/Promotions")]
    public class PromotionsController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public PromotionsController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Promotions
        [HttpGet]
        public async Task<IQueryable> GetAsync([FromQuery(Name = "_start")] int start, [FromQuery(Name = "_end")] int end,
            [FromQuery(Name = "_sort")] string sort, [FromQuery(Name = "_order")] string order)
        {

            Response.Headers.Add("X-Total-Count", _context.Promotion.Count().ToString());

            var promotions = await _context.Promotion.Select(a=> new Promotion
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Date = a.Date,
                ModifiedUser = new User
                {
                    Name = a.ModifiedUser.Name,
                    Surname = a.ModifiedUser.Surname
                },
                Client = new Client
                {
                    Description = a.Client.Description
                },
                Location = new Location
                {
                    Label = a.Location.Label
                },
                PromotionPromoter = new List<PromotionPromoter>(),
                PromotionProduct = new List<PromotionProduct>()
            }).Skip(start).Take(end - start).ToListAsync();

            foreach (var promotion in promotions)
            {

                promotion.PromotionProduct = await _context.PromotionProduct.Select(a => new PromotionProduct
                {
                    PromotionId = a.PromotionId,
                    Product = new Product
                    {
                        Label = a.Product.Label
                    }
                }).Where(a => a.PromotionId == promotion.Id).ToListAsync();

                promotion.PromotionPromoter = await _context.PromotionPromoter.Select(a => new PromotionPromoter
                {
                    PromotionId = a.PromotionId,
                    Promoter = new Promoter
                    {
                        Name = a.Promoter.Name
                    }
                }).Where(a => a.PromotionId == promotion.Id).ToListAsync();
            }

            return Sort(promotions.AsQueryable(),sort,order);
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotion.Select(a => new Promotion
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Date = a.Date,
                ClientId = a.ClientId,
                LocationId = a.LocationId,
                Client = new Client
                {
                    Description = a.Client.Description
                },
                Location = new Location
                {
                    Label = a.Location.Label
                },
                ProductIds = new List<int>(),
                PromoterIds = new List<int>()
            }).SingleOrDefaultAsync(m => m.Id == id);

            if (promotion == null)
            {
                return NotFound();
            }

            var promotionProduct =  _context.PromotionProduct.Select(a => new PromotionProduct
            {
                PromotionId = a.PromotionId,
                ProductId = a.ProductId
            }).Where(a => a.PromotionId == promotion.Id);

            var promotionPromoter = _context.PromotionPromoter.Select(a => new PromotionPromoter
            {
                PromotionId = a.PromotionId,
                PromoterId = a.PromoterId
            }).Where(a => a.PromotionId == promotion.Id);

            foreach (var p in promotionProduct)
            {
                promotion.ProductIds.Add(p.ProductId);
            }

            foreach (var p in promotionPromoter)
            {
                promotion.PromoterIds.Add(p.PromoterId);
            }

            return Ok(promotion);
        }

        // PUT: api/Promotions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != promotion.Id)
            {
                return BadRequest();
            }

            promotion.ModifiedUserId =
                Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            promotion.ModifiedDate = DateTime.Now;



            try
            {
                using (var context = _context)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            promotion.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                                .FindFirst(ClaimTypes.Sid).Value);
                            promotion.ModifiedDate = DateTime.Now;

                            _context.Entry(promotion).State = EntityState.Modified;

                            var promotionsProducts = _context.PromotionProduct.Where(a => a.PromotionId == id);

                            foreach (var promotionsProduct in promotionsProducts)
                            {
                                if (promotion.ProductIds.Contains(promotionsProduct.ProductId)) continue;
                                _context.PromotionProduct.Remove(promotionsProduct);
                            }

                            foreach (var productId in promotion.ProductIds)
                            {
                                if (!promotionsProducts.Any(a => a.ProductId == productId))
                                    context.PromotionProduct.Add(new PromotionProduct
                                    {
                                        ProductId = productId,
                                        PromotionId = id,
                                        ModifiedDate = DateTime.Now,
                                        ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                                            .FindFirst(ClaimTypes.Sid).Value),
                                        IsActive = true
                                    });
                            }

                            var promotionsPromoters = _context.PromotionPromoter.Where(a => a.PromotionId == id);

                            foreach (var promotionsPromoter in promotionsPromoters)
                            {
                                if (promotion.PromoterIds.Contains(promotionsPromoter.PromoterId)) continue;
                                _context.PromotionPromoter.Remove(promotionsPromoter);
                            }

                            foreach (var promoterId in promotion.PromoterIds)
                            {
                                if (!promotionsPromoters.Any(a => a.PromoterId == promoterId))
                                    context.PromotionPromoter.Add(new PromotionPromoter
                                    {
                                        PromoterId = promoterId,
                                        PromotionId = id,
                                        ModifiedDate = DateTime.Now,
                                        ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                                            .FindFirst(ClaimTypes.Sid).Value),
                                        IsActive = true
                                    });
                            }

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
                }
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

            return Ok(promotion);
        }

        // POST: api/Promotions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Promotion promotion)
        {
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
                        promotion.ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                            .FindFirst(ClaimTypes.Sid).Value);
                        promotion.ModifiedDate = DateTime.Now;

                        context.Promotion.Add(promotion);

                        await _context.SaveChangesAsync();

                        var promotionId = promotion.Id;

                        foreach (var id in promotion.ProductIds)
                        {
                            context.PromotionProduct.Add(new PromotionProduct
                            {
                                ProductId = id,
                                PromotionId = promotionId,
                                ModifiedDate = DateTime.Now,
                                ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                                    .FindFirst(ClaimTypes.Sid).Value),
                                IsActive = true
                            });
                        }

                        foreach (var id in promotion.PromoterIds)
                        {
                            context.PromotionPromoter.Add(new PromotionPromoter
                            {
                                PromoterId = id,
                                PromotionId = promotionId,
                                ModifiedDate = DateTime.Now,
                                ModifiedUserId = Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity)
                                    .FindFirst(ClaimTypes.Sid).Value),
                                IsActive = true
                            });
                        }

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
            }
            return CreatedAtAction("Get", new {id = promotion.Id}, promotion);
        }

        // DELETE: api/Promotions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotion.SingleOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            promotion.ModifiedUserId =
                Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity).FindFirst(ClaimTypes.Sid).Value);
            promotion.ModifiedDate = DateTime.Now;

            promotion.IsActive = false;

            _context.Entry(promotion).State = EntityState.Modified;

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

            return Ok(promotion);
        }

        private bool Exists(int id)
        {
            return _context.Promotion.Any(e => e.Id == id);
        }


        // GET: api/Promotions/AppPromotion
        [HttpGet("App")]
        public async Task<IActionResult> GetApp()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotionPromoterId =
                Convert.ToInt32(((ClaimsIdentity) HttpContext.User.Identity).FindFirst(ClaimTypes.GroupSid).Value);

            var promotionId = await _context.PromotionPromoter.FirstOrDefaultAsync(a => a.Id == promotionPromoterId);

            var promotion = await _context.Promotion.Include(a => a.PromotionProduct).ThenInclude(y=>y.Product).Include(a => a.Client).FirstOrDefaultAsync(m => m.Id == promotionId.Id); 

            var retVal = new Custom.ClientProductsReturn
            {
                Client = promotion.Client.Description,
                Products = new List<Custom.Product>()
            };

            foreach (var promotionProduct in promotion.PromotionProduct)
            {
                retVal.Products.Add(new Custom.Product
                {
                    Id = promotionProduct.ProductId,
                    Label = promotionProduct.Product.Label
                });
            }

            return Ok(retVal);

        }

    }
}