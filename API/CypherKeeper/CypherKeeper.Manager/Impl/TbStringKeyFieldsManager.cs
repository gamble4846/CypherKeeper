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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Impl
{
    public class TbStringKeyFieldsManager : ITbStringKeyFieldsManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        CypherKeeper.DataAccess.SQL.Interface.ITbStringKeyFieldsDataAccess SQLTbStringKeyFieldsDataAccess { get; set; }
        public SelectedServerModel CurrentServer { get; set; }

        public TbStringKeyFieldsManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
            var GlobalResult = new List<tbStringKeyFieldsModel>();
            var GlobalTotal = 0;
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbStringKeyFieldsDataAccess = new TbStringKeyFieldsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbStringKeyFieldsDataAccess.Get(page, itemsPerPage, orderBy, onlyNonDeleted);
                    if (result != null && result.Count > 0)
                    {
                        GlobalResult = result;
                        GlobalTotal = SQLTbStringKeyFieldsDataAccess.Total();
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

        public APIResponse Add(tbStringKeyFieldsModel model)
        {
            model = CommonFunctions.EncryptModel(model);
            tbStringKeyFieldsModel FinalResult = new tbStringKeyFieldsModel();
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbStringKeyFieldsDataAccess = new TbStringKeyFieldsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbStringKeyFieldsDataAccess.Add(model);
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

        public APIResponse Update(Guid Id, tbStringKeyFieldsModel model)
        {
            model = CommonFunctions.EncryptModel(model);
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbStringKeyFieldsDataAccess = new TbStringKeyFieldsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbStringKeyFieldsDataAccess.Update(Id,model);
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
                    SQLTbStringKeyFieldsDataAccess = new TbStringKeyFieldsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbStringKeyFieldsDataAccess.Delete(Id);
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
                    SQLTbStringKeyFieldsDataAccess = new TbStringKeyFieldsDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbStringKeyFieldsDataAccess.Restore(Id);
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
    }
}
