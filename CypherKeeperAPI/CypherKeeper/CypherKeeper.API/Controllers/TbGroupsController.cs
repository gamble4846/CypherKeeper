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
    public class TbGroupsController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public ITbGroupsManager TbGroupsManager { get; set; }

        public TbGroupsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITbGroupsManager tbGroupsManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            TbGroupsManager = tbGroupsManager;
        }

        [HttpGet]
        [Route("/api/TbGroups/Get")]
        public ActionResult Get(int page = 1, int itemsPerPage = 100, string orderBy = null, bool onlyNonDeleted = true)
        {
            try
            {
                List<OrderByModel> orderModelList = UtilityCommon.ConvertStringOrderToOrderModel(orderBy);
                return Ok(TbGroupsManager.Get(page, itemsPerPage, orderModelList, onlyNonDeleted));
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPost]
        [Route("/api/TbGroups/Add")]
        public ActionResult Add(tbGroupsAddModel AddModel)
        {
            try
            {
                return Ok(TbGroupsManager.Add(AddModel));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPut]
        [Route("/api/TbGroups/Update/{Id}")]
        public ActionResult Update(Guid Id, tbGroupsModel model)
        {
            try
            {
                model.UpdatedDate = DateTime.UtcNow;
                return Ok(TbGroupsManager.Update(Id, model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpDelete]
        [Route("/api/TbGroups/Delete/{Id}")]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                return Ok(TbGroupsManager.Delete(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPatch]
        [Route("/api/TbGroups/Restore/{Id}")]
        public ActionResult Restore(Guid Id)
        {
            try
            {
                return Ok(TbGroupsManager.Restore(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPatch]
        [Route("/api/TbGroups/Rename/{Id}/{NewName}")]
        public ActionResult Rename(Guid Id, string NewName)
        {
            try
            {
                return Ok(TbGroupsManager.Rename(Id, NewName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPatch]
        [Route("/api/TbGroups/ChangeIcon/{Id}")]
        public ActionResult ChangeIcon(Guid Id, Guid? IconId)
        {
            try
            {
                return Ok(TbGroupsManager.ChangeIcon(Id, IconId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }
    }
}
