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
    public class MixedDataAccess : IMixedDataAccess
    {
        private CommonFunctions _CF { get; set; }
        private string ConnectionString { get; set; }
        private CypherKeeper.DataAccess.SQL.Interface.ITbKeysDataAccess SQLTbKeysDataAccess { get; set; }
        private CypherKeeper.DataAccess.SQL.Interface.ITbStringKeyFieldsDataAccess SQLTbStringKeyFieldsDataAccess { get; set; }
        public MixedDataAccess(string connectionString, CommonFunctions _cf)
        {
            try
            {
                ConnectionString = connectionString;
                _CF = _cf;
                SQLTbKeysDataAccess = new TbKeysDataAccess(ConnectionString, _CF);
                SQLTbStringKeyFieldsDataAccess = new TbStringKeyFieldsDataAccess(ConnectionString, _CF);
            }
            catch (Exception) { }
        }

        public dynamic SaveKey(SavedKeyModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            tbKeysModel KeysModel_Save = new tbKeysModel();
            //Key
            //-------------------------------------------------------------
            if (model.Key.Id != null)
            {
                KeysModel_Save = SQLTbKeysDataAccess.GetById(model.Key.Id ?? Guid.NewGuid());
                KeysModel_Save = _CF.DecryptModel(KeysModel_Save);
                KeysModel_Save.ParentGroupId = model.Key.ParentGroupId;
                KeysModel_Save.Name = model.Key.Name;
                KeysModel_Save.UserName = model.Key.UserName;
                KeysModel_Save.Password = model.Key.Password;
                KeysModel_Save.WebsiteId = model.Key.WebsiteId;
                KeysModel_Save.Notes = model.Key.Notes;
                KeysModel_Save.UpdatedDate = DateTime.UtcNow;
                KeysModel_Save = _CF.EncryptModel(KeysModel_Save);

                List<SqlParameter> Parameters = new List<SqlParameter>();
                Parameters.Add(new SqlParameter("@Id", KeysModel_Save.Id));
                var WhereCondition = " WHERE Id = @Id ";
                if (_EC.Update(KeysModel_Save, WhereCondition, Parameters, "Id", false).ToUpper() == "TRUE")
                {
                    //Success
                }
                else
                {
                    _EC.RollBack();
                    throw new Exception("Error Updating Key");
                }
            }
            else
            {
                KeysModel_Save = new tbKeysModel()
                {
                    ParentGroupId = model.Key.ParentGroupId,
                    Name = model.Key.Name,
                    UserName = model.Key.UserName,
                    Password = model.Key.Password,
                    WebsiteId = model.Key.WebsiteId,
                    Notes = model.Key.Notes,
                    isDeleted = false,
                    DeletedDate = null,
                    UpdatedDate = null,
                    CreatedDate = DateTime.UtcNow,
                };
                KeysModel_Save = _CF.EncryptModel(KeysModel_Save);
                KeysModel_Save.Id = new Guid(_EC.Add(KeysModel_Save, "Id", "Id", false));
            }
            //-------------------------------------------------------------

            foreach (var StringKeyField in model.StringKeyFields)
            {
                var tbStringKeyFields = new tbStringKeyFieldsModel();
                if (StringKeyField.Id != null)
                {
                    tbStringKeyFields = SQLTbStringKeyFieldsDataAccess.GetById(StringKeyField.Id ?? Guid.NewGuid());
                    tbStringKeyFields = _CF.DecryptModel(tbStringKeyFields);
                    tbStringKeyFields.Name = StringKeyField.Name;
                    tbStringKeyFields.Value = StringKeyField.Value;
                    tbStringKeyFields.UpdatedDate = DateTime.UtcNow;

                    List<SqlParameter> Parameters = new List<SqlParameter>();
                    Parameters.Add(new SqlParameter("@Id", tbStringKeyFields.Id));
                    var WhereCondition = " WHERE Id = @Id ";
                    if (_EC.Update(tbStringKeyFields, WhereCondition, Parameters, "Id", false).ToUpper() == "TRUE")
                    {
                        //Success
                    }
                    else
                    {
                        _EC.RollBack();
                        throw new Exception("Error Updating Key");
                    }
                }
                else
                {
                    tbStringKeyFields = new tbStringKeyFieldsModel()
                    {
                        Name = StringKeyField.Name,
                        Value = StringKeyField.Value,
                        ParentKeyId = KeysModel_Save.Id,
                        isDeleted = false,
                        DeletedDate = null,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = null,
                    };

                    tbStringKeyFields = _CF.EncryptModel(tbStringKeyFields);
                    tbStringKeyFields.Id = new Guid(_EC.Add(tbStringKeyFields, "Id", "Id", false));
                }
            }

            _EC.SaveChanges();
            return KeysModel_Save.Id;
        }
    }
}
