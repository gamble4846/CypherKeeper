using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.SQL.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary;
using EasyCrudLibrary.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.SQL.Impl
{
    public class TbIconsDataAccess : ITbIconsDataAccess
    {
        private CommonFunctions _CF { get; set; }
        private string ConnectionString { get; set; }
        public TbIconsDataAccess(string connectionString, CommonFunctions _cf)
        {
            try
            {
                ConnectionString = connectionString;
                _CF = _cf;
            }
            catch (Exception) { }
        }

        public List<tbIconsModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            var WhereCondition = " ";
            if (onlyNonDeleted)
            {
                WhereCondition = " WHERE isDeleted = 0 ";
            }
            return _EC.GetList<tbIconsModel>(page, itemsPerPage, orderBy, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public tbIconsModel GetById(Guid Id, bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var WhereCondition = " WHERE Id = @Id ";
            if (onlyNonDeleted)
            {
                WhereCondition += " AND isDeleted = 0 ";
            }
            return _EC.GetFirstOrDefault<tbIconsModel>(WhereCondition, Parameters, GSEnums.WithInQuery.NoLock);
        }

        public tbIconsModel Add(tbIconsModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            var newId = _EC.Add(model, "Id", "Id", true);
            return GetById(Guid.Parse(newId), false);
        }

        public bool Update(Guid Id, tbIconsModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var WhereCondition = " WHERE Id = @Id ";
            return _EC.Update(model, WhereCondition, Parameters, "Id", true) == "true";
        }

        public bool Delete(Guid Id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var Query = "UPDATE tbIcons SET isDeleted = 1 WHERE Id = @Id";
            return _EC.Query(Query, Parameters, true, GSEnums.ExecuteType.ExecuteNonQuery) > 0;
        }

        public bool Restore(Guid Id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var Query = "UPDATE tbIcons SET isDeleted = 0 WHERE Id = @Id";
            return _EC.Query(Query, Parameters, true, GSEnums.ExecuteType.ExecuteNonQuery) > 0;
        }

        public int Total()
        {
            var _EC = new EasyCrud(ConnectionString);
            return _EC.Count<tbIconsModel>(null, null, GSEnums.WithInQuery.ReadPast);
        }
    }
}
