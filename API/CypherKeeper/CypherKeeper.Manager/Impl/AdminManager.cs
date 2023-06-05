using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.MongoDB.Impl;
using CypherKeeper.DataAccess.MongoDB.Interface;
using CypherKeeper.DataAccess.SQL.Impl;
using CypherKeeper.DataAccess.SQL.Interface;
using CypherKeeper.Manager.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Impl
{
    public class AdminManager : IAdminManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        public Cryptography _Cryptography { get; set; }
        public MongoDBValues MongoValues { get; set; }
        public IAdminDataAccess DataAccess { get; set; }

        public AdminManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            MongoValues = CommonFunctions.GetMongoDBValues();
            DataAccess = new AdminDataAccess(configuration, httpContextAccessor);
            _Cryptography = new Cryptography(configuration);
        }

        public APIResponse Register(RegisterModel model)
        {
            var EmailCheck = DataAccess.GetByEmail(model.Email);
            if (EmailCheck != null)
            {
                return new APIResponse(ResponseCode.ERROR, "Email Already Exists");
            }

            var UsernameCheck = DataAccess.GetByUsername(model.Username);
            if (UsernameCheck != null)
            {
                return new APIResponse(ResponseCode.ERROR, "Username Already Exists");
            }

            var ToInsertModel = new tbAccessModel()
            {
                _id = Guid.NewGuid().ToString(),
                Username = model.Username,
                Password = SecureHasher.Hash(model.Password),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Settings = new SettingsModel(),
            };

            var data = DataAccess.Register(ToInsertModel);
            return new APIResponse(ResponseCode.SUCCESS, "Register Success", data);
        }

        public APIResponse Login(string Username, string Password)
        {
            var UsernameCheck = DataAccess.GetByUsername(Username);
            if (UsernameCheck == null)
            {
                return new APIResponse(ResponseCode.ERROR, "Username And Password did not match");
            }

            var VerifyPassword = SecureHasher.Verify(Password, UsernameCheck.Password);

            if (VerifyPassword)
            {
                var model = new LoginModel()
                {
                    Username = Username,
                    Password = _Cryptography.Encrypt(Password),
                };

                var claims = new List<ClaimModel>();
                claims.Add(new ClaimModel () { ClaimName = "LoginData", Data = model });

                var token = CommonFunctions.CreateJWTToken(claims);

                return new APIResponse(ResponseCode.SUCCESS, "Login Success", token);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Username And Password did not match");
            }
        }
    }
}
