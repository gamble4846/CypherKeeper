﻿using CypherKeeper.AuthLayer.Models;
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
    public class TbStringKeyFieldsDataAccess : ITbStringKeyFieldsDataAccess
    {
        private CommonFunctions _CF { get; set; }
        private string ConnectionString { get; set; }
        public TbStringKeyFieldsDataAccess(string connectionString, CommonFunctions _cf)
        {
            try
            {
                ConnectionString = connectionString;
                _CF = _cf;
            }
            catch (Exception) { }
        }

        public List<tbStringKeyFieldsModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            var WhereCondition = " ";
            if (onlyNonDeleted)
            {
                WhereCondition = " WHERE isDeleted = 0 ";
            }
            return _EC.GetList<tbStringKeyFieldsModel>(page, itemsPerPage, orderBy, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public tbStringKeyFieldsModel GetById(Guid Id, bool onlyNonDeleted = true)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var WhereCondition = " WHERE Id = @Id ";
            if (onlyNonDeleted)
            {
                WhereCondition += " AND isDeleted = 0 ";
            }
            return _EC.GetFirstOrDefault<tbStringKeyFieldsModel>(WhereCondition, Parameters, GSEnums.WithInQuery.NoLock);
        }

        public tbStringKeyFieldsModel Add(tbStringKeyFieldsModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            var newId = _EC.Add(model, "Id", "Id", true);
            return GetById(Guid.Parse(newId), false);
        }

        public bool Update(Guid Id, tbStringKeyFieldsModel model)
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
            Parameters.Add(new SqlParameter("@DeletedDate", DateTime.UtcNow.ToString()));
            var Query = "UPDATE tbStringKeyFields SET isDeleted = 1, DeletedDate = @DeletedDate WHERE Id = @Id";
            return _EC.Query(Query, Parameters, true, GSEnums.ExecuteType.ExecuteNonQuery) > 0;
        }

        public bool Restore(Guid Id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var Query = "UPDATE tbStringKeyFields SET isDeleted = 0 WHERE Id = @Id";
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
            return _EC.Count<tbStringKeyFieldsModel>(WhereCondition, null, GSEnums.WithInQuery.ReadPast);
        }
    }
}
