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
    public class TbIconsManager : ITbIconsManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        CypherKeeper.DataAccess.SQL.Interface.ITbIconsDataAccess SQLTbIconsDataAccess { get; set; }
        CypherKeeper.DataAccess.GoogleSheets.Interface.ITbIconsDataAccess GoogleSheetTbIconsDataAccess { get; set; }
        public SelectedServerModel CurrentServer { get; set; }

        public TbIconsManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            CurrentServer = CommonFunctions.GetCurrentServer();
            if (CurrentServer == null)
            {
                throw new Exception("Server Not Found");
            }

            SQLTbIconsDataAccess = new TbIconsDataAccess(CurrentServer.ConnectionString, CommonFunctions);
            GoogleSheetTbIconsDataAccess = new CypherKeeper.DataAccess.GoogleSheets.Impl.TbIconsDataAccess(CurrentServer.ConnectionString, CommonFunctions);
        }

        public APIResponse Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true)
        {
            var GlobalResult = new List<tbIconsModel>();
            var GlobalTotal = 0;
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var resultSQL = SQLTbIconsDataAccess.Get(page, itemsPerPage, orderBy, onlyNonDeleted);
                    if (resultSQL != null && resultSQL.Count > 0)
                    {
                        GlobalResult = resultSQL;
                        GlobalTotal = SQLTbIconsDataAccess.Total();
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                case "GoogleSheets":
                    var resultGoogleSheets = GoogleSheetTbIconsDataAccess.Get(page, itemsPerPage, onlyNonDeleted);
                    if (resultGoogleSheets != null)
                    {
                        GlobalResult = resultGoogleSheets;
                        GlobalTotal = GoogleSheetTbIconsDataAccess.Total();
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Inserted");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }

            var response = new { records = GlobalResult, pageNumber = page, pageSize = itemsPerPage, totalRecords = GlobalTotal };
            return new APIResponse(ResponseCode.SUCCESS, "Records Found", response);
        }

        public APIResponse Add(tbIconsModel model)
        {
            tbIconsModel FinalResult = new tbIconsModel();
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var resultSQL = SQLTbIconsDataAccess.Add(model);
                    if (resultSQL != null)
                    {
                        FinalResult = resultSQL;
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Inserted");
                    }
                case "GoogleSheets":
                    var resultGoogleSheet = GoogleSheetTbIconsDataAccess.Add(model);
                    if (resultGoogleSheet != null)
                    {
                        FinalResult = resultGoogleSheet;
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Inserted");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }


            return new APIResponse(ResponseCode.SUCCESS, "Record Inserted", FinalResult);
        }

        public APIResponse Update(Guid Id, tbIconsModel model)
        {
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    var resultSQL = SQLTbIconsDataAccess.Update(Id,model);
                    if (resultSQL)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Updated", resultSQL);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                    }
                case "GoogleSheets":
                    var resultGoogleSheet = GoogleSheetTbIconsDataAccess.Update(Id, model);
                    if (resultGoogleSheet)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Updated", resultGoogleSheet);
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
                    var resultSQL = SQLTbIconsDataAccess.Delete(Id);
                    if (resultSQL)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", resultSQL);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");
                    }
                case "GoogleSheets":
                    var resultGoogleSheet = GoogleSheetTbIconsDataAccess.Delete(Id);
                    if (resultGoogleSheet)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", resultGoogleSheet);
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
                    var resultSQL = SQLTbIconsDataAccess.Restore(Id);
                    if (resultSQL)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Restored", resultSQL);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Restored");
                    }
                case "GoogleSheets":
                    var resultGoogleSheet = GoogleSheetTbIconsDataAccess.Restore(Id);
                    if (resultGoogleSheet)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Restored", resultGoogleSheet);
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
