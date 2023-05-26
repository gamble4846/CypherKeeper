using AuthLayer.Utility;
using CypherKeeper.DataAccess.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.Impl
{
    public class TbGroupsDataAccess : ITbGroupsDataAccess
    {
        private CommonFunctions _cf { get; set; }
        private string ConnectionString { get; set; }
        public TbGroupsDataAccess(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            try
            {
                _cf = new CommonFunctions(configuration, env.ContentRootPath, httpContextAccessor);
                ConnectionString = _cf.GetNewConnectionString();
            }
            catch (Exception) { }
        }

        public List<tbGroupsModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null)
        {

        }
    }
}
