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
    public class TbKeysManager : ITbKeysManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        CypherKeeper.DataAccess.SQL.Interface.ITbKeysDataAccess SQLTbKeysDataAccess { get; set; }
        public SelectedServerModel CurrentServer { get; set; }

        public TbKeysManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
            var GlobalResult = new List<tbKeysModel>();
            var GlobalTotal = 0;
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbKeysDataAccess = new TbKeysDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbKeysDataAccess.Get(page, itemsPerPage, orderBy, onlyNonDeleted);
                    if (result != null && result.Count > 0)
                    {
                        GlobalResult = result;
                        GlobalTotal = SQLTbKeysDataAccess.Total();
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
            return new APIResponse(ResponseCode.SUCCESS, "Records Found", response);
        }

        public APIResponse Add(tbKeysModel model)
        {
            model = CommonFunctions.EncryptModel(model);
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbKeysDataAccess = new TbKeysDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbKeysDataAccess.Add(model);
                    if (result != null)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Inserted", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Inserted");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }
        }

        public APIResponse Update(Guid Id, tbKeysModel model)
        {
            model = CommonFunctions.EncryptModel(model);
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbKeysDataAccess = new TbKeysDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbKeysDataAccess.Update(Id,model);
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
                    SQLTbKeysDataAccess = new TbKeysDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbKeysDataAccess.Delete(Id);
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
                    SQLTbKeysDataAccess = new TbKeysDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbKeysDataAccess.Restore(Id);
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
