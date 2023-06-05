﻿using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MongoDB.Driver;
using NuGet.Configuration;

namespace CypherKeeper.AuthLayer.ActionFilters
{
    public class FullAuthorization : IAuthorizationFilter
    {
        public IConfiguration Configuration;
        IHttpContextAccessor HttpContextAccessor;

        public FullAuthorization(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _CF = new CommonFunctions(Configuration, HttpContextAccessor);
            var CurrentUser = _CF.GetCurrentUser();
            if (CurrentUser == null)
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
