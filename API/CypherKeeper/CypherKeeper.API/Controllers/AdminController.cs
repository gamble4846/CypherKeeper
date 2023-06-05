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
using EasyCrudLibrary.Model;
using System.Collections.Generic;
using CypherKeeper.Manager.Interface;
using CypherKeeper.Model;

namespace CypherKeeper.API.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public ITbGroupsManager TbGroupsManager { get; set; }

        public AdminController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITbGroupsManager tbGroupsManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            TbGroupsManager = tbGroupsManager;
        }

        [HttpGet]
        [Route("/api/Admin/Login")]
        public ActionResult Login(string UserName, string Password)
        {
            try
            {
                var model = new tbAccessModel()
                {
                    UserName = "rohan",
                    Password = "rohan",
                    Email = "rohan"
                };
                return Ok(MongoLayer.InsertMongo(model, "CypherKeeper", "tbAccess"));
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }
    }
}
