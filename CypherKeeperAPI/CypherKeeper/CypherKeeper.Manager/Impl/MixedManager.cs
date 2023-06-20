using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.SQL.Impl;
using CypherKeeper.DataAccess.SQL.Interface;
using CypherKeeper.Manager.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Impl
{
    public class MixedManager : IMixedManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        CypherKeeper.DataAccess.SQL.Interface.IMixedDataAccess SQLMixedDataAccess { get; set; }
        public SelectedServerModel CurrentServer { get; set; }

        public MixedManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            CurrentServer = CommonFunctions.GetCurrentServer();
            if (CurrentServer == null)
            {
                throw new Exception("Server Not Found");
            }
        }

        public APIResponse SaveKey(SavedKeyModel model)
        {
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLMixedDataAccess = new MixedDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLMixedDataAccess.SaveKey(model);
                    if (result != null)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Saved", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Saved");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }

        public APIResponse GetKeyHistory(Guid KeyId)
        {
            List<tbKeysHistoryModel> GlobalResult = new List<tbKeysHistoryModel>();
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLMixedDataAccess = new MixedDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLMixedDataAccess.GetKeyHistory(KeyId);
                    if (result != null)
                    {
                        GlobalResult = result;
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Saved");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }

            for (var i = 0; i < GlobalResult.Count; i++)
            {
                GlobalResult[i] = CommonFunctions.DecryptModel(GlobalResult[i]);
            }

            var stringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(GlobalResult);
            var EncryptedStringsResponse = CommonFunctions.EncryptFinalResponseString(stringResponse);

            return new APIResponse(ResponseCode.SUCCESS, "Records Found", EncryptedStringsResponse, true);
        }
    }
}
