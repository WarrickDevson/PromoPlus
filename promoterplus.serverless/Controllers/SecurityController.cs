using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using promoterplus.serverless.Helpers;
using promoterplus.serverless.Models;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Promotions;

namespace promoterplus.serverless.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1.0/Security")]
    public class SecurityController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public SecurityController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            int? promotionPromoterId = null;

            if (user.Email == null || user.Password == null) return Unauthorized();
            var myUser = await  _context.User.FirstOrDefaultAsync(u =>u.Email == user.Email);
            if (!PasswordHelper.ValidatePassword(user.Password, myUser.Password))
            {
                return Unauthorized();
            }

            if (user.Username == null) return Ok(new {token = GenerateToken(myUser.Email, myUser.Id, null)});
            {
                var promoter = await _context.Promoter.Include(a=>a.PromotionPromoter).ThenInclude(b=>b.Promotion).FirstOrDefaultAsync(u => u.Username == user.Username);

                if (promoter == null)
                {
                    return Unauthorized();
                }

                var promotionPromoters = promoter?.PromotionPromoter;

                if (promotionPromoters != null)
                    foreach (var p in promotionPromoters)
                    {
                        if (p.Promotion.Date != DateTime.Today) continue;
                        promotionPromoterId = p.Id;
                        _context.Checkin.Add(new Checkin
                        {
                            PromotionPromoterId = p.Id,
                            Latitude = user.Latitude,
                            Longitude = user.Longitude,
                            ModifiedDate = DateTime.Now,
                            ModifiedUserId = myUser.Id,
                            IsActive = true
                        });
                        await _context.SaveChangesAsync();
                        break;
                    }

                if (promotionPromoterId == null)
                {
                    return BadRequest("No promotions for this user today");
                }
            }

            return Ok(new {token = GenerateToken(myUser.Email,myUser.Id,promotionPromoterId)});
        }

        private string GenerateToken(string username,int userId,int? promotionPromoterId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Sid, userId.ToString()),
                new Claim(ClaimTypes.GroupSid,promotionPromoterId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["SiteUrl"],
                _configuration["SiteUrl"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["SessionDuration"])),
                signingCredentials: creds);


            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}