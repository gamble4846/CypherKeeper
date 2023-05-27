using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CypherKeeper.AuthLayer.Models;

namespace CypherKeeper.AuthLayer.Utility
{
    public class CommonFunctions
    {
        public IConfiguration Configuration { get; }
        public string ContentRootPath { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public CommonFunctions(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        public CommonFunctions()
        {

        }

        public TokenModel GetTokenData()
        {
            try
            {
                string token = GetTokenFromHeader();

                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                token = token.Replace("Bearer ", "");
                var jwtSection = Configuration.GetSection("Jwt");
                var Secret = Encoding.ASCII.GetBytes(jwtSection.GetValue<string>("Secret"));
                var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
                var ValidAudience = jwtSection.GetValue<string>("ValidAudience");
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                var otherClaims = claims.Identities.ToList()[0].Claims.ToList();

                TokenModel TokenData = new TokenModel();
                TokenData = JsonConvert.DeserializeObject<TokenModel>(otherClaims.Find(x => x.Type == "TokenData").Value);
                return TokenData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetTokenFromHeader()
        {
            try
            {
                return HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
