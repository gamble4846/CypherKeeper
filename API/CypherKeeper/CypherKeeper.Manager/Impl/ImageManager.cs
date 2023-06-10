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
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using RestSharp;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Method = RestSharp.Method;

namespace CypherKeeper.Manager.Impl
{
    public class ImageManager : IImageManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        public MongoDBValues MongoValues { get; set; }
        public IAdminDataAccess DataAccess { get; set; }

        public ImageManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            MongoValues = CommonFunctions.GetMongoDBValues();
            DataAccess = new AdminDataAccess(configuration, httpContextAccessor);
        }

        public async Task<APIResponse> UploadImageToImgur(byte[] bytes)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", "263bbc738ab2de2");
            httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text/plain");
            string temp_inBase64 = Convert.ToBase64String(bytes);
            var response = await httpclient.PostAsync("https://api.imgur.com/3/Image", new StringContent(temp_inBase64));
            var stringcontent = await response.Content.ReadAsStringAsync();
            var ImgurResponseModel = JsonConvert.DeserializeObject<ImgurResponseModel>(stringcontent);

            if (ImgurResponseModel.success)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Image Uploaded", ImgurResponseModel);
            }
            else
            {
                return new APIResponse(ResponseCode.SUCCESS, "Image Not Uploaded", ImgurResponseModel);
            }
        }
    }
}
