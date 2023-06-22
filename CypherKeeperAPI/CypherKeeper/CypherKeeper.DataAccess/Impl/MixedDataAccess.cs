using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.SQL.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary;
using EasyCrudLibrary.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.IO;
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
        private CypherKeeper.DataAccess.SQL.Interface.ITbGroupsDataAccess SQLTbGroupsDataAccess { get; set; }
        private CypherKeeper.DataAccess.SQL.Interface.ITbTwoFactorAuthDataAccess SQLTbTwoFactorAuthDataAccess { get; set; }
        public MixedDataAccess(string connectionString, CommonFunctions _cf)
        {
            try
            {
                ConnectionString = connectionString;
                _CF = _cf;
                SQLTbKeysDataAccess = new TbKeysDataAccess(ConnectionString, _CF);
                SQLTbStringKeyFieldsDataAccess = new TbStringKeyFieldsDataAccess(ConnectionString, _CF);
                SQLTbGroupsDataAccess = new TbGroupsDataAccess(ConnectionString, _CF);
            }
            catch (Exception) { }
        }

        public dynamic SaveKey(SavedKeyModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            tbKeysModel KeysModel_Save = new tbKeysModel();
            if (model.Key.Id != null)
            {
                var OldKeyData = SQLTbKeysDataAccess.GetById(model.Key.Id ?? Guid.NewGuid());
                var OldStringKeyFields = SQLTbStringKeyFieldsDataAccess.GetByKeyId(model.Key.Id ?? Guid.NewGuid());
                var OldTwoFactorAuths = SQLTbTwoFactorAuthDataAccess.GetByKeyId(model.Key.Id ?? Guid.NewGuid());

                OldKeyData = _CF.DecryptModel(OldKeyData);
                for (int i = 0; i < OldStringKeyFields.Count; i++)
                {
                    OldStringKeyFields[i] = _CF.DecryptModel(OldStringKeyFields[i]);
                }
                for (int i = 0; i < OldTwoFactorAuths.Count; i++)
                {
                    OldTwoFactorAuths[i] = _CF.DecryptModel(OldTwoFactorAuths[i]);
                }

                var _KeysJSONModel = new KeysJSONModel()
                {
                    Key = OldKeyData,
                    CustomFields = OldStringKeyFields,
                    TwoFactorAuths = OldTwoFactorAuths
                };

                var KeysJSON = Newtonsoft.Json.JsonConvert.SerializeObject(_KeysJSONModel);


                var toAddToHistory = new tbKeysHistoryModel()
                {
                    KeyId = model.Key.Id ?? Guid.NewGuid(),
                    KeysJSON = KeysJSON,
                    Type = "",
                    isDeleted = false,
                    Date = DateTime.UtcNow,
                };

                toAddToHistory = _CF.EncryptModel(toAddToHistory);

                toAddToHistory.Id = new Guid(_EC.Add(toAddToHistory, "Id", "Id", false));
            }
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
                    tbStringKeyFields = _CF.EncryptModel(tbStringKeyFields);

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

            foreach (var TwoFactorAuth in model.TwoFactorAuths)
            {
                var TwoFactorAuthModel = new tbTwoFactorAuthModel();
                if (TwoFactorAuth.Id != null)
                {
                    TwoFactorAuthModel = SQLTbTwoFactorAuthDataAccess.GetById(TwoFactorAuth.Id ?? Guid.NewGuid());
                    TwoFactorAuthModel = _CF.DecryptModel(TwoFactorAuthModel);
                    TwoFactorAuthModel.Name = TwoFactorAuth.Name;
                    TwoFactorAuthModel.SecretKey = TwoFactorAuth.SecretKey;
                    TwoFactorAuthModel.Mode = TwoFactorAuth.Mode;
                    TwoFactorAuthModel.CodeSize = TwoFactorAuth.CodeSize;
                    TwoFactorAuthModel.Type = TwoFactorAuth.Type;
                    TwoFactorAuthModel.KeyId = TwoFactorAuth.KeyId;
                    TwoFactorAuthModel.UpdatedDate = DateTime.UtcNow;
                    TwoFactorAuthModel = _CF.EncryptModel(TwoFactorAuthModel);

                    List<SqlParameter> Parameters = new List<SqlParameter>();
                    Parameters.Add(new SqlParameter("@Id", TwoFactorAuth.Id));
                    var WhereCondition = " WHERE Id = @Id ";
                    if (_EC.Update(TwoFactorAuthModel, WhereCondition, Parameters, "Id", false).ToUpper() == "TRUE")
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
                    TwoFactorAuthModel = new tbTwoFactorAuthModel()
                    {
                        Name = TwoFactorAuth.Name,
                        SecretKey = TwoFactorAuth.SecretKey,
                        Mode = TwoFactorAuth.Mode,
                        CodeSize = TwoFactorAuth.CodeSize,
                        Type = TwoFactorAuth.Type,
                        KeyId = TwoFactorAuth.KeyId,
                        isDeleted = false,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = null,
                        DeletedDate = null,
                        ArrangePosition = SQLTbTwoFactorAuthDataAccess.Total() + 1,
                    };

                    TwoFactorAuthModel = _CF.EncryptModel(TwoFactorAuthModel);
                    TwoFactorAuthModel.Id = new Guid(_EC.Add(TwoFactorAuthModel, "Id", "Id", false));
                }
            }

            _EC.SaveChanges();
            return KeysModel_Save.Id;
        }

        public List<tbKeysHistoryModel> GetKeyHistory(Guid KeyId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@KeyId", KeyId));
            var WhereCondition = " WHERE KeyId = @KeyId AND isDeleted = 0";
            return _EC.GetList<tbKeysHistoryModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public bool DublicateKey(Guid KeyId, Guid? NewGroupId = null)
        {
            var _EC = new EasyCrud(ConnectionString);
            var OldKeyData = SQLTbKeysDataAccess.GetById(KeyId);
            var OldStringKeyFields = SQLTbStringKeyFieldsDataAccess.GetByKeyId(KeyId);
            var OldKeyHistory = GetKeyHistory(KeyId);
            OldKeyHistory.Reverse();

            if(NewGroupId != null)
            {
                OldKeyData.ParentGroupId = NewGroupId ?? Guid.NewGuid();
            }

            var NewKey = SQLTbKeysDataAccess.Add(OldKeyData);
            foreach(var history in OldKeyHistory)
            {
                history.KeyId = NewKey.Id;
                history.Id = new Guid(_EC.Add(history, "Id", "Id", false));
            }

            foreach(var customKeyData in OldStringKeyFields)
            {
                customKeyData.ParentKeyId = NewKey.Id;
                customKeyData.Id = new Guid(_EC.Add(customKeyData, "Id", "Id", false));
            }

            _EC.SaveChanges();
            return true;
        }

        public bool DublicateGroup(Guid GroupId)
        {
            var OldGroupData = SQLTbGroupsDataAccess.GetById(GroupId);
            var OldKeys = SQLTbKeysDataAccess.GetByGroupId(GroupId);

            var NewGroup = SQLTbGroupsDataAccess.Add(OldGroupData);
            foreach(var key in OldKeys)
            {
                DublicateKey(key.Id, NewGroup.Id);
            }

            return true;
        }
    }
}
