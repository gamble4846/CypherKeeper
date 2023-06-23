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
using CypherKeeper.AuthLayer.ActionFilters;

namespace CypherKeeper.API.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public IAdminManager AdminManager { get; set; }

        public AdminController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IAdminManager adminManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            AdminManager = adminManager;
        }

        [HttpPost]
        [Route("/api/Admin/Register")]
        public ActionResult Register(RegisterModel model)
        {
            try
            {
                return Ok(AdminManager.Register(model));
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPost]
        [Route("/api/Admin/Login")]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                return Ok(AdminManager.Login(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPost]
        [Route("/api/Server/Add")]
        [LoginAuthorization]
        public ActionResult AddServer(ServerViewModel model)
        {
            try
            {
                return Ok(AdminManager.AddServer(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpGet]
        [Route("/api/Servers/Get")]
        [LoginAuthorization]
        public ActionResult GetServers()
        {
            try
            {
                return Ok(AdminManager.GetServers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPost]
        [Route("/api/Server/Select")]
        [LoginAuthorization]
        public ActionResult SelectServer(SelectServerModel model)
        {
            try
            {
                return Ok(AdminManager.SelectServer(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpGet]
        [Route("/api/Images/Get")]
        [LoginAuthorization]
        public ActionResult GetImages()
        {
            try
            {
                return Ok(AdminManager.GetImages());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }
    }
}
 