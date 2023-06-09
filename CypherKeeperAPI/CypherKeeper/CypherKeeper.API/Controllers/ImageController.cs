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
using System.IO;
using SharpCompress.Compressors.Xz;
using System.Threading.Tasks;

namespace CypherKeeper.API.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public CommonFunctions CommonFunctions { get; set; }
        public IImageManager ImageManager { get; set; }
        public IAdminManager AdminManager { get; set; }

        public ImageController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IImageManager imageManager, IAdminManager adminManager)
        {
            Configuration = configuration;
            CommonFunctions = new CommonFunctions(Configuration, httpContextAccessor);
            ImageManager = imageManager;
            AdminManager = adminManager;
        }

        [HttpPost]
        [Route("/api/Image/UploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    var ImagurResponse = await ImageManager.UploadImageToImgur(bytes);
                    if(ImagurResponse == null)
                    {
                        return StatusCode(500, new APIResponse(ResponseCode.ERROR, "Image Not Uplaoded", null));
                    }

                    var ImageLink = ImagurResponse.data.link;
                    return Ok(AdminManager.AddImage(ImageLink));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

    }
}
