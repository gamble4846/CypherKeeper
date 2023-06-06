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
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
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
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpGet]
        [Route("/api/Settings/Get")]
        [FullAuthorization]
        public ActionResult GetSettings()
        {
            try
            {
                return Ok(AdminManager.GetSettings());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPost]
        [Route("/api/Settings/Update")]
        [FullAuthorization]
        public ActionResult UpdateSettings(SettingsModel model)
        {
            try
            {
                return Ok(AdminManager.UpdateSettings(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

    }
}