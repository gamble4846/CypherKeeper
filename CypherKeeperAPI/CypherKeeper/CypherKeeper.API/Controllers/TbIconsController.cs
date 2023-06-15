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
    public class TbIconsController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public ITbIconsManager TbIconsManager { get; set; }

        public TbIconsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITbIconsManager tbIconsManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            TbIconsManager = tbIconsManager;
        }

        [HttpGet]
        [Route("/api/TbIcons/Get")]
        public ActionResult Get(int page = 1, int itemsPerPage = 100, string orderBy = null, bool onlyNonDeleted = true)
        {
            try
            {
                List<OrderByModel> orderModelList = UtilityCommon.ConvertStringOrderToOrderModel(orderBy);
                return Ok(TbIconsManager.Get(page, itemsPerPage, orderModelList, onlyNonDeleted));
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPost]
        [Route("/api/TbIcons/Add")]
        public ActionResult Add(tbIconsModel model)
        {
            try
            {
                model.CreatedDate = DateTime.UtcNow;
                return Ok(TbIconsManager.Add(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPut]
        [Route("/api/TbIcons/Update/{Id}")]
        public ActionResult Update(Guid Id, tbIconsModel model)
        {
            try
            {
                model.UpdatedDate = DateTime.UtcNow;
                return Ok(TbIconsManager.Update(Id, model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpDelete]
        [Route("/api/TbIcons/Delete/{Id}")]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                return Ok(TbIconsManager.Delete(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }

        [HttpPatch]
        [Route("/api/TbIcons/Restore/{Id}")]
        public ActionResult Restore(Guid Id)
        {
            try
            {
                return Ok(TbIconsManager.Restore(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }
        }
    }
}
