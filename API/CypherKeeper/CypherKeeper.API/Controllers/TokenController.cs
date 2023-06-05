using System;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using log4net;
using Newtonsoft.Json;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.Utility;
using CypherKeeper.AuthLayer.Models;
using System.Configuration;

namespace CypherKeeper.API.Controllers
{
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }

        public TokenController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
        }

        [HttpPost]
        [Route("/api/CreateToken")]
        public ActionResult CreateToken(SettingsModel model)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var Secret = jwtSection.GetValue<string>("Secret");
            var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
            var ValidAudience = jwtSection.GetValue<string>("ValidAudience");


            var claims = new[]
            {
                new Claim("TokenData", JsonConvert.SerializeObject( model )),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: ValidIssuer, audience: ValidAudience, claims: claims, expires: DateTime.Now.AddYears(1), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new APIResponse(ResponseCode.SUCCESS, "Token Generated", tokenString));
        }

        [Authorize]
        [HttpGet]
        [Route("/api/GetToken")]
        public ActionResult GetToken()
        {
            try
            {
                var tokenData = CommonFunctions.GetSettings();
                if (tokenData == null)
                {
                    return StatusCode(500, new APIResponse(ResponseCode.ERROR, "Token Not Found", null));
                }
                else
                {
                    return Ok(new APIResponse(ResponseCode.SUCCESS, "Token Recieved", CommonFunctions.GetSettings()));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }

        }
    }
}
