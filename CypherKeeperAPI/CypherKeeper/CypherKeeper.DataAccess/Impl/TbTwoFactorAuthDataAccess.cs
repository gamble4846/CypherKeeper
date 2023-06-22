using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.SQL.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CypherKeeper.DataAccess.SQL.Impl
{
    public class TbTwoFactorAuthDataAccess : ITbTwoFactorAuthDataAccess
    {
        private CommonFunctions _CF { get; set; }
        private string ConnectionString { get; set; }
        public TbTwoFactorAuthDataAccess(string connectionString, CommonFunctions _cf)
        {
            try
            {
                ConnectionString = connectionString;
                _CF = _cf;
            }
            catch (Exception) { }
        }

        public List<tbTwoFactorAuthModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            var WhereCondition = " ";
            if (onlyNonDeleted)
            {
                WhereCondition = " WHERE isDeleted = 0 ";
            }
            return _EC.GetList<tbTwoFactorAuthModel>(page, itemsPerPage, orderBy, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<tbTwoFactorAuthModel> GetByKeyId(Guid KeyId, bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            var WhereCondition = " WHERE KeyId = @KeyId";
            Parameters.Add(new SqlParameter("@KeyId", KeyId));
            if (onlyNonDeleted)
            {
                WhereCondition += " AND isDeleted = 0 ";
            }
            return _EC.GetList<tbTwoFactorAuthModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public tbTwoFactorAuthModel GetById(Guid Id, bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var WhereCondition = " WHERE Id = @Id ";
            if (onlyNonDeleted)
            {
                WhereCondition += " AND isDeleted = 0 ";
            }
            return _EC.GetFirstOrDefault<tbTwoFactorAuthModel>(WhereCondition, Parameters, GSEnums.WithInQuery.NoLock);
        }

        public tbTwoFactorAuthModel Add(tbTwoFactorAuthModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            var newId = _EC.Add(model, "Id", "Id", true);
            return GetById(Guid.Parse(newId), false);
        }

        public bool Update(Guid Id, tbTwoFactorAuthModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var WhereCondition = " WHERE Id = @Id ";
            return _EC.Update(model, WhereCondition, Parameters, "Id", true).ToUpper() == "TRUE";
        }

        public bool Delete(Guid Id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));    
            Parameters.Add(new SqlParameter("@DeletedDate", _CF.GetSQLCurrentDateTime()));
            var Query = "UPDATE tbTwoFactorAuth SET isDeleted = 1, DeletedDate = @DeletedDate WHERE Id = @Id";
            return _EC.Query(Query, Parameters, true, GSEnums.ExecuteType.ExecuteNonQuery) > 0;
        }

        public bool Restore(Guid Id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var Query = "UPDATE tbTwoFactorAuth SET isDeleted = 0 WHERE Id = @Id";
            return _EC.Query(Query, Parameters, true, GSEnums.ExecuteType.ExecuteNonQuery) > 0;
        }

        public int Total(bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            var WhereCondition = "  ";
            if (onlyNonDeleted)
            {
                WhereCondition = " WHERE isDeleted = 0 ";
            }
            return _EC.Count<tbTwoFactorAuthModel>(WhereCondition, null, GSEnums.WithInQuery.ReadPast);
        }
    }
}
