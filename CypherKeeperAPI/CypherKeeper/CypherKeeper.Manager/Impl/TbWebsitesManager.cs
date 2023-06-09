﻿using CypherKeeper.AuthLayer.Models;
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Impl
{
    public class TbWebsitesManager : ITbWebsitesManager
    {
        public CommonFunctions CommonFunctions { get; set; }
        CypherKeeper.DataAccess.SQL.Interface.ITbWebsitesDataAccess SQLTbWebsitesDataAccess { get; set; }
        public SelectedServerModel CurrentServer { get; set; }

        public TbWebsitesManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
            var GlobalResult = new List<tbWebsitesModel>();
            var GlobalTotal = 0;
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbWebsitesDataAccess = new TbWebsitesDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbWebsitesDataAccess.Get(page, itemsPerPage, orderBy, onlyNonDeleted);
                    if (result != null && result.Count > 0)
                    {
                        GlobalResult = result;
                        GlobalTotal = SQLTbWebsitesDataAccess.Total();
                        break;
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", CurrentServer.DatabaseType);
            }


            var response = new { records = GlobalResult, pageNumber = page, pageSize = itemsPerPage, totalRecords = GlobalTotal };
            return new APIResponse(ResponseCode.SUCCESS, "Records Found", response);
        }

        public APIResponse Add(tbWebsitesModel_ToAdd model)
        {
            tbWebsitesModel ToAddModel = new tbWebsitesModel()
            {
                Name = model.Name,
                Link = model.Link,
                IconId = model.IconId,
                isDeleted = false,
                DeletedDate = null,
                UpdatedDate = null,
                CreatedDate = DateTime.UtcNow,
            };

            tbWebsitesModel FinalResult = new tbWebsitesModel();
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbWebsitesDataAccess = new TbWebsitesDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbWebsitesDataAccess.Add(ToAddModel);
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
            return new APIResponse(ResponseCode.SUCCESS, "Record Inserted", FinalResult);
        }

        public APIResponse Update(Guid Id, tbWebsitesModel model)
        {
            switch (CurrentServer.DatabaseType)
            {
                case "SQLServer":
                    SQLTbWebsitesDataAccess = new TbWebsitesDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbWebsitesDataAccess.Update(Id,model);
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
                    SQLTbWebsitesDataAccess = new TbWebsitesDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbWebsitesDataAccess.Delete(Id);
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
                    SQLTbWebsitesDataAccess = new TbWebsitesDataAccess(CurrentServer.ConnectionString, CommonFunctions);

                    var result = SQLTbWebsitesDataAccess.Restore(Id);
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
