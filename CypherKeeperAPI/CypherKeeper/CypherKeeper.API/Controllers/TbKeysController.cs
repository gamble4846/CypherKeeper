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
    [LoginAuthorization]
    [ServerRequired]
    public class TbKeysController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public ITbKeysManager TbKeysManager { get; set; }

        public TbKeysController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITbKeysManager tbKeysManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            TbKeysManager = tbKeysManager;
        }

        [HttpGet]
        [Route("/api/TbKeys/Get")]
        public ActionResult Get(int page = 1, int itemsPerPage = 100, string orderBy = null, bool onlyNonDeleted = true)
        {
            try
            {
                List<OrderByModel> orderModelList = UtilityCommon.ConvertStringOrderToOrderModel(orderBy);
                return Ok(TbKeysManager.Get(page, itemsPerPage, orderModelList, onlyNonDeleted));
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpGet]
        [Route("/api/TbKeys/GetByGroupId/{GroupId}")]
        public ActionResult GetByGroupId(Guid GroupId)
        {
            try
            {
                return Ok(TbKeysManager.GetByGroupId(GroupId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPost]
        [Route("/api/TbKeys/Add")]
        public ActionResult Add(tbKeysModel model)
        {
            try
            {
                model.CreatedDate = DateTime.UtcNow;
                return Ok(TbKeysManager.Add(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPut]
        [Route("/api/TbKeys/Update/{Id}")]
        public ActionResult Update(Guid Id, tbKeysModel model)
        {
            try
            {
                model.UpdatedDate = DateTime.UtcNow;
                return Ok(TbKeysManager.Update(Id, model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpDelete]
        [Route("/api/TbKeys/Delete/{Id}")]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                return Ok(TbKeysManager.Delete(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPatch]
        [Route("/api/TbKeys/Restore/{Id}")]
        public ActionResult Restore(Guid Id)
        {
            try
            {
                return Ok(TbKeysManager.Restore(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }
    }
}
