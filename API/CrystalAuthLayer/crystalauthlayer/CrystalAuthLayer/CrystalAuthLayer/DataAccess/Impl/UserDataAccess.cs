using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using AuthLayer.DataAccess.Interface;
using AuthLayer.Models;
using EasyCrudLibrary;
using AuthLayer.Utility;

namespace AuthLayer.DataAccess.Impl
{
    public class UserDataAccess : IUserDataAccess
    {
        private CommonFunctions _cf { get; set; }
        private string ConnectionString { get; set; }

        public UserDataAccess(IHostingEnvironment env, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                _cf = new CommonFunctions(configuration, env.ContentRootPath, httpContextAccessor);
                ConnectionString = _cf.GetNewConnectionString();
            }
            catch (Exception) { }
        }

        public TbaccessModel GetUserById(string UserId, string connectionString)
        {
            var _EC = new EasyCrud(connectionString);

            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@UserId", UserId));

            return _EC.GetFirstOrDefault<TbaccessModel>("WHERE ID = @UserId", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public TbaccessModel GetUserByAccessGUID(Guid GUIDAccess)
        {
            var _EC = new EasyCrud(ConnectionString);

            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDAccess", GUIDAccess));

            return _EC.GetFirstOrDefault<TbaccessModel>("WHERE GUIDAccess = @GUIDAccess", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<TbaccessModel> GetUsersByAccessGUIDList(List<Guid> GUIDAccess)
        {
            var _EC = new EasyCrud(ConnectionString);
            string GUIDsAccess = "('" + string.Join("','", GUIDAccess) + "')";
            string WhereCondition = " WHERE GUIDAccess IN " + GUIDsAccess;
            return _EC.GetList<TbaccessModel>(-1,-1,null, WhereCondition, null, GSEnums.WithInQuery.ReadPast);
        }
    }
}

