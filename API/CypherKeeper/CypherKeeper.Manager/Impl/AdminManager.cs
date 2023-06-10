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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Impl
{
    public class AdminManager : IAdminManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        public MongoDBValues MongoValues { get; set; }
        public IAdminDataAccess DataAccess { get; set; }

        public AdminManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            MongoValues = CommonFunctions.GetMongoDBValues();
            DataAccess = new AdminDataAccess(configuration, httpContextAccessor);
        }

        public APIResponse Register(RegisterModel model)
        {
            model.Password = CommonFunctions.DecryptRSAEncryptedString(model.Password);
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
                Images = new List<ImagesModel>(),
            };

            var data = DataAccess.Register(ToInsertModel);
            return new APIResponse(ResponseCode.SUCCESS, "Register Success", data);
        }

        public APIResponse Login(LoginModel model)
        {
            model.Password = CommonFunctions.DecryptRSAEncryptedString(model.Password);
            var UsernameCheck = DataAccess.GetByUsername(model.Username);
            if (UsernameCheck == null)
            {
                return new APIResponse(ResponseCode.ERROR, "Username And Password did not match");
            }

            var VerifyPassword = SecureHasher.Verify(model.Password, UsernameCheck.Password);

            if (VerifyPassword)
            {
                var claims = new List<ClaimModel>();
                model.Password = CommonFunctions.Encrypt(model.Password);
                claims.Add(new ClaimModel() { ClaimName = "LoginData", Data = model });

                var token = CommonFunctions.CreateJWTToken(claims);
                return new APIResponse(ResponseCode.SUCCESS, "Login Success", token);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Username And Password did not match");
            }
        }

        public APIResponse AddServer(ServerViewModel model)
        {
            var ToAddModel = new Server()
            {
                GUIDServer = Guid.NewGuid(),
                ServerName = model.ServerName,
                DatabaseType = model.DatabaseType,
                ConnectionString = CommonFunctions.Encrypt(CommonFunctions.DecryptRSAEncryptedString(model.ConnectionString), CommonFunctions.DecryptRSAEncryptedString(model.Key)),
                KeyVerify = CommonFunctions.Encrypt("Verify", CommonFunctions.DecryptRSAEncryptedString(model.Key)),
                ImageLink = model.ImageLink,
            };

            var CurrentUser = CommonFunctions.GetCurrentUser();
            if (CurrentUser == null)
            {
                return new APIResponse(ResponseCode.ERROR, "User Not Found");
            }

            var OldSettingsData = CommonFunctions.CreateDeepCopy(CurrentUser.Settings);
            if (OldSettingsData == null)
            {
                OldSettingsData = new SettingsModel();
            }
            if (OldSettingsData.Servers == null)
            {
                OldSettingsData.Servers = new List<Server>();
            }

            OldSettingsData.Servers.Add(ToAddModel);


            var result = DataAccess.UpdateUserSettings(OldSettingsData, CurrentUser);

            if (result > 0)
            {
                var toReturn = new ServerDisplayModel()
                {
                    GUIDServer = ToAddModel.GUIDServer,
                    ServerName = ToAddModel.ServerName,
                    DatabaseType = ToAddModel.DatabaseType,
                    ImageLink = ToAddModel.ImageLink,
                };
                return new APIResponse(ResponseCode.SUCCESS, "UpdateSuccess", toReturn);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Update Failed", null);
            }
        }

        public APIResponse GetServers()
        {
            var CurrentUser = CommonFunctions.GetCurrentUser();
            if (CurrentUser == null)
            {
                return new APIResponse(ResponseCode.ERROR, "User Not Found");
            }

            var SettingsData = CommonFunctions.CreateDeepCopy(CurrentUser.Settings);
            if (SettingsData == null)
            {
                SettingsData = new SettingsModel();
            }
            if (SettingsData.Servers == null)
            {
                SettingsData.Servers = new List<Server>();
            }

            return new APIResponse(ResponseCode.SUCCESS, "UpdateSuccess", SettingsData.Servers);
        }

        public APIResponse SelectServer(SelectServerModel model)
        {
            model.Key = CommonFunctions.DecryptRSAEncryptedString(model.Key);

            var CurrentUser = CommonFunctions.GetCurrentUser();
            if (CurrentUser == null)
            {
                return new APIResponse(ResponseCode.ERROR, "User Not Found");
            }

            var SettingsData = CommonFunctions.CreateDeepCopy(CurrentUser.Settings);
            if (SettingsData == null || SettingsData.Servers == null || SettingsData.Servers.Count == 0)
            {
                return new APIResponse(ResponseCode.ERROR, "Server Not Found");
            }

            var CurrentServer = SettingsData.Servers.Find(x => x.GUIDServer == model.GUIDServer);
            if (CurrentServer == null)
            {
                return new APIResponse(ResponseCode.ERROR, "Server Not Found");
            }

            var _KeyVerify = CommonFunctions.Decrypt(CurrentServer.KeyVerify, model.Key);
            if (_KeyVerify != "Verify")
            {
                return new APIResponse(ResponseCode.ERROR, "Incorrect Key");
            }

            var SelectedServer = new SelectedServerModel()
            {
                GUIDServer = CurrentServer.GUIDServer,
                ServerName = CurrentServer.ServerName,
                DatabaseType = CurrentServer.DatabaseType,
                ConnectionString = CommonFunctions.Decrypt(CurrentServer.ConnectionString, model.Key),
                Key = model.Key,
                ImageLink = CurrentServer.ImageLink,
            };

            var OldClaims = CommonFunctions.GetClaimsFromToken(CommonFunctions.GetTokenFromHeader());
            OldClaims.Add(new Claim("ServerData", CommonFunctions.Encrypt(JsonConvert.SerializeObject(SelectedServer))));

            var NewToken = CommonFunctions.CreateJWTToken(OldClaims);

            return new APIResponse(ResponseCode.SUCCESS, "Server Selected", NewToken);
        }

        public APIResponse AddImage(string ImageLink)
        {
            var CurrentUser = CommonFunctions.GetCurrentUser();
            if (CurrentUser == null)
            {
                return new APIResponse(ResponseCode.ERROR, "User Not Found");
            }

            var OldImages = CommonFunctions.CreateDeepCopy(CurrentUser.Images);
            if (OldImages == null)
            {
                OldImages = new List<ImagesModel>();
            }

            OldImages.Add(new ImagesModel() { ImageLink = ImageLink });

            var result = DataAccess.UpdateUserImages(OldImages, CurrentUser);

            if (result > 0)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Image Added", OldImages);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Image Not Added to Mongo", null);
            }
        }

        public APIResponse GetImages()
        {
            var CurrentUser = CommonFunctions.GetCurrentUser();
            if (CurrentUser == null)
            {
                return new APIResponse(ResponseCode.ERROR, "User Not Found");
            }

            var ImageData = CommonFunctions.CreateDeepCopy(CurrentUser.Images);
            if (ImageData == null)
            {
                ImageData = new List<ImagesModel>();
            }

            return new APIResponse(ResponseCode.SUCCESS, "UpdateSuccess", ImageData);
        }
    }
}
