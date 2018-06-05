using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using promoterplus.serverless.Models;

namespace promoterplus.serverless.Controllers
{
    [Route("api/v1.0/[controller]")]
    public class TestController : Controller
    {
        private readonly PromoterPlusContext _context;
        private readonly IConfiguration _configuration;

        public TestController(PromoterPlusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Index()
        {
            try
            {
                return "Api Service Online " + _context.User?.FirstOrDefault()?.ModifiedDate;
            }
            catch (Exception e)
            {
                return "Error " + e.Message;
            }



        }

        // POST api/values
        [HttpPost]
        public void Post()
        {
            //The ".FirstOrDefault()" method will return either the first matched
        }
    }
}
