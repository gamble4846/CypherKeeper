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
    public class TbWebsitesController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public ITbWebsitesManager TbWebsitesManager { get; set; }

        public TbWebsitesController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITbWebsitesManager tbWebsitesManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            TbWebsitesManager = tbWebsitesManager;
        }

        [HttpGet]
        [Route("/api/TbWebsites/Get")]
        public ActionResult Get(int page = 1, int itemsPerPage = 100, string orderBy = null, bool onlyNonDeleted = true)
        {
            try
            {
                List<OrderByModel> orderModelList = UtilityCommon.ConvertStringOrderToOrderModel(orderBy);
                return Ok(TbWebsitesManager.Get(page, itemsPerPage, orderModelList, onlyNonDeleted));
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPost]
        [Route("/api/TbWebsites/Add")]
        public ActionResult Add(tbWebsitesModel model)
        {
            try
            {
                model.CreatedDate = DateTime.UtcNow;
                return Ok(TbWebsitesManager.Add(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPut]
        [Route("/api/TbWebsites/Update/{Id}")]
        public ActionResult Update(Guid Id, tbWebsitesModel model)
        {
            try
            {
                model.UpdatedDate = DateTime.UtcNow;
                return Ok(TbWebsitesManager.Update(Id, model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpDelete]
        [Route("/api/TbWebsites/Delete/{Id}")]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                return Ok(TbWebsitesManager.Delete(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPatch]
        [Route("/api/TbWebsites/Restore/{Id}")]
        public ActionResult Restore(Guid Id)
        {
            try
            {
                return Ok(TbWebsitesManager.Restore(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }
    }
}
