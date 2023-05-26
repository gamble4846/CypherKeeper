using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using AuthLayer.Mangers.Interface;
using AuthLayer.Models;
using AuthLayer.Utility;

namespace AuthLayer.ActionFilters
{
    public class FullAuthorization : IAuthorizationFilter
    {
        public IConfiguration Configuration;
        public string ContentRootPath;
        IHttpContextAccessor HttpContextAccessor;
        public IUserManager UserManager;
        public IRoleManager RoleManager;

        public FullAuthorization(IRoleManager roleManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment env, IUserManager manager)
        {
            Configuration = configuration;
            ContentRootPath = env.ContentRootPath;
            HttpContextAccessor = httpContextAccessor;
            UserManager = manager;
            RoleManager = roleManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _cs = new CommonFunctions(Configuration, ContentRootPath, HttpContextAccessor);
            var accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new ForbidResult();
                return;
            }

            var tokenData = _cs.GetTokenData(accessToken);

            if(tokenData == null) {
                context.Result = new ForbidResult();
                return;
            }

            var userId = tokenData.userId;
            var company = tokenData.CompanyData;
            var password = tokenData.password;
            var connectionString = _cs.GetNewConnectionString();
            TbaccessModel currentUser = UserManager.GetUserByIdAndPassword(userId, password, connectionString);

            if (currentUser == null)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }

    public class FullAuthorizationAttribute : TypeFilterAttribute
    {
        public FullAuthorizationAttribute() : base(typeof(FullAuthorization))
        {
            Arguments = new object[] { };
        }
    }
}
