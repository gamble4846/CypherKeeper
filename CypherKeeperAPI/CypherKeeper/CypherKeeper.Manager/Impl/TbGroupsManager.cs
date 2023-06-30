using Azure;
using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.SQL.Impl;
using CypherKeeper.DataAccess.SQL.Interface;
using CypherKeeper.Manager.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Impl
{
    public class TbGroupsManager : ITbGroupsManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        CypherKeeper.DataAccess.SQL.Interface.ITbGroupsDataAccess SQLTbGroupsDataAccess { get; set; }
        CypherKeeper.DataAccess.GoogleSheets.Interface.ITbGroupsDataAccess GoogleSheetTbGroupsDataAccess { get; set; }
        public SelectedServerModel CurrentServer { get; set; }

        public TbGroupsManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            CurrentServer = CommonFunctions.GetCurrentServer();
            if (CurrentServer == null)
            {
                throw new Exception("Server Not Found");
            }
            SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);
            GoogleSheetTbGroupsDataAccess = new CypherKeeper.DataAccess.GoogleSheets.Impl.TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);
        }

        public APIResponse Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true)
        {
            var GlobalResult = new List<tbGroupsModel>();
            var GlobalTotal = 0;
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var resultSQLServer = SQLTbGroupsDataAccess.Get(page, itemsPerPage, orderBy, onlyNonDeleted);
                    if (resultSQLServer != null && resultSQLServer.Count > 0)
                    {
                        GlobalResult = resultSQLServer;
                        GlobalTotal = SQLTbGroupsDataAccess.Total();
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                case "GoogleSheets":
                    var resultGoogleSheets = GoogleSheetTbGroupsDataAccess.Get(page, itemsPerPage, onlyNonDeleted);
                    if (resultGoogleSheets != null && resultGoogleSheets.Count > 0)
                    {
                        GlobalResult = resultGoogleSheets;
                        GlobalTotal = GoogleSheetTbGroupsDataAccess.Total();
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }

            for (var i = 0; i < GlobalResult.Count; i++)
            {
                GlobalResult[i] = CommonFunctions.DecryptModel(GlobalResult[i]);
            }

            var response = new { records = GlobalResult, pageNumber = page, pageSize = itemsPerPage, totalRecords = GlobalTotal };
            var stringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            var EncryptedStringsResponse = CommonFunctions.EncryptFinalResponseString(stringResponse);

            return new APIResponse(ResponseCode.SUCCESS, "Records Found", EncryptedStringsResponse, true);
        }

        public APIResponse Add(tbGroupsAddModel AddModel)
        {
            var model = new tbGroupsModel()
            {
                Id = Guid.NewGuid(),
                Name = AddModel.Name,
                ParentGroupId = AddModel.ParentGroupId,
                IconId = AddModel.IconId,
                isDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = null,
                DeletedDate = null,
                ArrangePosition = 0,
            };
            model = CommonFunctions.EncryptModel(model);

            tbGroupsModel FinalResult = new tbGroupsModel();
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    model.ArrangePosition = SQLTbGroupsDataAccess.GetTotalByParentGroupId(model.ParentGroupId);
                    var resultSQLServer = SQLTbGroupsDataAccess.Add(model);
                    if (resultSQLServer != null)
                    {
                        FinalResult = resultSQLServer;
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Inserted");
                    }
                case "GoogleSheets":
                    model.ArrangePosition = GoogleSheetTbGroupsDataAccess.GetTotalByParentGroupId(model.ParentGroupId);
                    var resultGoogleSheets = GoogleSheetTbGroupsDataAccess.Add(model);
                    if (resultGoogleSheets != null)
                    {
                        FinalResult = resultGoogleSheets;
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Inserted");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }

            FinalResult = CommonFunctions.DecryptModel(FinalResult);
            var stringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(FinalResult);
            var EncryptedStringsResponse = CommonFunctions.EncryptFinalResponseString(stringResponse);
            return new APIResponse(ResponseCode.SUCCESS, "Record Inserted", EncryptedStringsResponse, true);
        }

        public APIResponse Update(Guid Id, tbGroupsModel model)
        {
            model = CommonFunctions.EncryptModel(model);
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var resultSQLServer = SQLTbGroupsDataAccess.Update(Id, model);
                    if (resultSQLServer)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Updated", resultSQLServer);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                    }
                case "GoogleSheets":
                    var resultGoogleSheets = GoogleSheetTbGroupsDataAccess.Update(Id, model);
                    if (resultGoogleSheets)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Updated", resultGoogleSheets);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }

        public APIResponse Delete(Guid Id)
        {
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var resultSQLServer = SQLTbGroupsDataAccess.Delete(Id);
                    if (resultSQLServer)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", resultSQLServer);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");
                    }
                case "GoogleSheets":
                    var resultGoogleSheets = GoogleSheetTbGroupsDataAccess.Delete(Id);
                    if (resultGoogleSheets)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", resultGoogleSheets);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }

        public APIResponse Restore(Guid Id)
        {
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var resultSQLServer = SQLTbGroupsDataAccess.Restore(Id);
                    if (resultSQLServer)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Restored", resultSQLServer);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Restored");
                    }
                case "GoogleSheets":
                    var resultGoogleSheets = GoogleSheetTbGroupsDataAccess.Restore(Id);
                    if (resultGoogleSheets)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Restored", resultGoogleSheets);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Restored");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }

        public APIResponse Rename(Guid Id, string NewName)
        {
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var OldGroupDataSQLServer = SQLTbGroupsDataAccess.GetById(Id);
                    OldGroupDataSQLServer = CommonFunctions.DecryptModel(OldGroupDataSQLServer);
                    if (OldGroupDataSQLServer == null)
                    {
                        return new APIResponse(ResponseCode.ERROR, "Group Not Found");
                    }
                    else
                    {
                        OldGroupDataSQLServer.Name = NewName;
                        OldGroupDataSQLServer.UpdatedDate = DateTime.UtcNow;
                        return Update(OldGroupDataSQLServer.Id, OldGroupDataSQLServer);
                    }
                case "GoogleSheets":
                    var OldGroupDataGoogleSheets = GoogleSheetTbGroupsDataAccess.GetById(Id);
                    OldGroupDataGoogleSheets = CommonFunctions.DecryptModel(OldGroupDataGoogleSheets);
                    if (OldGroupDataGoogleSheets == null)
                    {
                        return new APIResponse(ResponseCode.ERROR, "Group Not Found");
                    }
                    else
                    {
                        OldGroupDataGoogleSheets.Name = NewName;
                        OldGroupDataGoogleSheets.UpdatedDate = DateTime.UtcNow;
                        return Update(OldGroupDataGoogleSheets.Id, OldGroupDataGoogleSheets);
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }

        public APIResponse ChangeIcon(Guid Id, Guid? IconId)
        {
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var OldGroupDataSQLServer = SQLTbGroupsDataAccess.GetById(Id);
                    OldGroupDataSQLServer = CommonFunctions.DecryptModel(OldGroupDataSQLServer);
                    if (OldGroupDataSQLServer == null)
                    {
                        return new APIResponse(ResponseCode.ERROR, "Group Not Found");
                    }
                    else
                    {
                        OldGroupDataSQLServer.IconId = IconId;
                        OldGroupDataSQLServer.UpdatedDate = DateTime.UtcNow;
                        return Update(OldGroupDataSQLServer.Id, OldGroupDataSQLServer);
                    }
                case "GoogleSheets":
                    var OldGroupDataGoogleSheets = GoogleSheetTbGroupsDataAccess.GetById(Id);
                    OldGroupDataGoogleSheets = CommonFunctions.DecryptModel(OldGroupDataGoogleSheets);
                    if (OldGroupDataGoogleSheets == null)
                    {
                        return new APIResponse(ResponseCode.ERROR, "Group Not Found");
                    }
                    else
                    {
                        OldGroupDataGoogleSheets.IconId = IconId;
                        OldGroupDataGoogleSheets.UpdatedDate = DateTime.UtcNow;
                        return Update(OldGroupDataGoogleSheets.Id, OldGroupDataGoogleSheets);
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }
    }
}
