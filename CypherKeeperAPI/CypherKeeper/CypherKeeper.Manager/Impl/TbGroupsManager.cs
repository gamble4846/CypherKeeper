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
        public SelectedServerModel CurrentServer { get; set; }

        public TbGroupsManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            CurrentServer = CommonFunctions.GetCurrentServer();
            if (CurrentServer == null)
            {
                throw new Exception("Server Not Found");
            }
        }

        public APIResponse Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true)
        {
            var GlobalResult = new List<tbGroupsModel>();
            var GlobalTotal = 0;
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);
                    var result = SQLTbGroupsDataAccess.Get(page, itemsPerPage, orderBy, onlyNonDeleted);
                    if (result != null && result.Count > 0)
                    {
                        GlobalResult = result;
                        GlobalTotal = SQLTbGroupsDataAccess.Total();
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
                    SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    model.ArrangePosition = SQLTbGroupsDataAccess.GetTotalByParentGroupId(model.ParentGroupId);
                    var result = SQLTbGroupsDataAccess.Add(model);
                    if (result != null)
                    {
                        FinalResult = result;
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
                    SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbGroupsDataAccess.Update(Id, model);
                    if (result)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
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
                    SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbGroupsDataAccess.Delete(Id);
                    if (result)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
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
                    SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbGroupsDataAccess.Restore(Id);
                    if (result)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Restored", result);
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
                    SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var OldGroupData = SQLTbGroupsDataAccess.GetById(Id);
                    OldGroupData = CommonFunctions.DecryptModel(OldGroupData);
                    if (OldGroupData == null)
                    {
                        return new APIResponse(ResponseCode.ERROR, "Group Not Found");
                    }
                    else
                    {
                        OldGroupData.Name = NewName;
                        OldGroupData.UpdatedDate = DateTime.UtcNow;
                        return Update(OldGroupData.Id, OldGroupData);
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
                    SQLTbGroupsDataAccess = new TbGroupsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var OldGroupData = SQLTbGroupsDataAccess.GetById(Id);
                    OldGroupData = CommonFunctions.DecryptModel(OldGroupData);
                    if (OldGroupData == null)
                    {
                        return new APIResponse(ResponseCode.ERROR, "Group Not Found");
                    }
                    else
                    {
                        OldGroupData.IconId = IconId;
                        OldGroupData.UpdatedDate = DateTime.UtcNow;
                        return Update(OldGroupData.Id, OldGroupData);
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }
    }
}
