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
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace CypherKeeper.API.Controllers
{
    [ApiController]
    [LoginAuthorization]
    [ServerRequired]
    public class MixedController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public IMixedManager MixedManager { get; set; }

        public MixedController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMixedManager mixedManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            MixedManager = mixedManager;
        }

        [HttpPost]
        [Route("/api/Mixed/SaveKey")]
        public ActionResult SaveKey(SavedKeyModel model)
        {
            try
            {
                return Ok(MixedManager.SaveKey(model));
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpGet]
        [Route("/api/Mixed/KeyHistory/{KeyId}")]
        public ActionResult GetKeyHistory(Guid KeyId)
        {
            try
            {
                return Ok(MixedManager.GetKeyHistory(KeyId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpGet]
        [Route("/api/Mixed/DublicateKey/{KeyId}")]
        public ActionResult DublicateKey(Guid KeyId)
        {
            try
            {
                return Ok(MixedManager.DublicateKey(KeyId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpGet]
        [Route("/api/Mixed/DublicateGroup/{GroupId}")]
        public ActionResult DublicateGroup(Guid GroupId)
        {
            try
            {
                return Ok(MixedManager.DublicateGroup(GroupId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpGet]
        [Route("/api/Mixed/GetTwoFACodeData/{TwoFAId}")]
        public ActionResult GetTwoFACodeData(Guid TwoFAId)
        {
            try
            {
                return Ok(MixedManager.GetTwoFACodeData(TwoFAId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }
    }
}
