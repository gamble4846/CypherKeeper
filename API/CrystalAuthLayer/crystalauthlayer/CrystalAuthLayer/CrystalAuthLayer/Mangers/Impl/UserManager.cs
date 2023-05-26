using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using AuthLayer.DataAccess.Interface;
using AuthLayer.Mangers.Interface;
using AuthLayer.Models;
using AuthLayer.Utility;

namespace AuthLayer.Mangers.Impl
{
    public class UserManager : IUserManager
    {
        private readonly IUserDataAccess DataAccess = null;
        private CommonFunctions _cf { get; set; }
        IHttpContextAccessor HttpContextAccessor;
        public UserManager(IUserDataAccess dataAccess, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            _cf = new CommonFunctions(configuration, env.ContentRootPath, httpContextAccessor);
            DataAccess = dataAccess;
            HttpContextAccessor = httpContextAccessor;
        }

        public TbaccessModel GetUserByIdAndPassword(string userId, string userPassword, string connectionString)
        {
            var currentUser = DataAccess.GetUserById(userId, connectionString);
            if (currentUser == null)
            {
                return null;
            }

            var ValidPassword = CommonFunctions.VerifyPassword(userPassword, currentUser.Password);
            if (!ValidPassword)
            {
                return null;
            }

            return currentUser;
        }

        public TbaccessModel GetCurrentUser()
        {
            var tokenData = _cf.GetTokenData(HttpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            var connectionString = _cf.GetNewConnectionString();
            return GetUserByIdAndPassword(tokenData.userId, tokenData.password, connectionString);
        }
    }
}

