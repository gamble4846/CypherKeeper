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
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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

        public SettingsModel GetSettings()
        {
            try
            {
                var currentUser = GetCurrentUser();
                return currentUser.Settings;
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

        public MongoDBValues GetMongoDBValues()
        {
            var mongoSection = Configuration.GetSection("MongoDB");

            var model = new MongoDBValues();
            model.ConnectionString = mongoSection.GetValue<string>("ConnectionString");
            model.Database = mongoSection.GetValue<string>("Database");

            return model;
        }

        public string CreateJWTToken(List<ClaimModel> Claims)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var Secret = jwtSection.GetValue<string>("Secret");
            var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
            var ValidAudience = jwtSection.GetValue<string>("ValidAudience");


            var claims = new List<Claim>();

            foreach (var claim in Claims)
            {
                claims.Add(new Claim(claim.ClaimName, JsonConvert.SerializeObject(claim.Data)));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: ValidIssuer, audience: ValidAudience, claims: claims, expires: DateTime.Now.AddYears(1), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public string CreateJWTToken(List<Claim> Claims)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var Secret = jwtSection.GetValue<string>("Secret");
            var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
            var ValidAudience = jwtSection.GetValue<string>("ValidAudience");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: ValidIssuer, audience: ValidAudience, claims: Claims, expires: DateTime.Now.AddYears(1), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public List<Claim> GetClaimsFromToken(string JWTToken)
        {
            try
            {
                JWTToken = JWTToken.Replace("Bearer ", "");
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
                var claims = handler.ValidateToken(JWTToken, validations, out var tokenSecure);
                var otherClaims = claims.Identities.ToList()[0].Claims.ToList();

                return otherClaims;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SelectedServerModel GetCurrentServer()
        {
            try
            {
                var _cryptography = new Cryptography(Configuration, HttpContextAccessor);

                var accessToken = GetTokenFromHeader();
                if (string.IsNullOrEmpty(accessToken)) { return null; }

                var claims = GetClaimsFromToken(accessToken);
                if (claims == null) { return null; }

                var ServerDataClaim = claims.Find(x => x.Type == "Server Data");
                if (ServerDataClaim == null) { return null; }

                var ServerData = JsonConvert.DeserializeObject<SelectedServerModel>(ServerDataClaim.Value);
                if (ServerData == null) { return null; }

                return ServerData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public tbAccessModel GetCurrentUser()
        {
            try
            {
                var _cryptography = new Cryptography(Configuration, HttpContextAccessor);

                var accessToken = GetTokenFromHeader();
                if (string.IsNullOrEmpty(accessToken)) { return null; }

                var claims = GetClaimsFromToken(accessToken);
                if (claims == null) { return null; }

                var LoginDataClaim = claims.Find(x => x.Type == "LoginData");
                if (LoginDataClaim == null) { return null; }

                var LoginData = JsonConvert.DeserializeObject<LoginModel>(LoginDataClaim.Value);
                if (LoginData == null) { return null; }

                var MongoValues = GetMongoDBValues();
                var Settings = MongoClientSettings.FromConnectionString(MongoValues.ConnectionString);
                Settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var Client = new MongoClient(Settings);
                var Database = Client.GetDatabase(MongoValues.Database);
                var Collection = "tbAccess";
                var collection = Database.GetCollection<tbAccessModel>(Collection);
                var filter = Builders<tbAccessModel>.Filter.Eq("Username", LoginData.Username);
                var documents = collection.Find(filter).ToList();
                if (documents.Count == 0) { return null; }

                var CurrentUserData = documents[0];
                var VerifyPassword = SecureHasher.Verify(LoginData.Password, CurrentUserData.Password);
                if (!VerifyPassword) { return null; }

                return CurrentUserData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T CreateDeepCopy<T>(T obj)
        {
            var stringObj = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var returnObect = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringObj);
            return returnObect;
        }
    }
}
