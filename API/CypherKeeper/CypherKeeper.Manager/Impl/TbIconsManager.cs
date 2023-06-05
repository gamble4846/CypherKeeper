﻿using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.SQL.Impl;
using CypherKeeper.DataAccess.SQL.Interface;
using CypherKeeper.Manager.Interface;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using Microsoft.AspNetCore.Http;
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
        public SettingsModel TokenData { get; set; }
        public string ConnectionString { get; set; }
        public string ServerType { get; set; }

        public TbIconsManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            TokenData = CommonFunctions.GetTokenData();
            ConnectionString = TokenData.Servers.Find(x => x.IsSelected).ConnectionString;
            ServerType = TokenData.Servers.Find(x => x.IsSelected).DatabaseType;
        }

        public APIResponse Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true)
        {
            switch (ServerType)
            {
                case "SQLServer":
                    SQLTbIconsDataAccess = new TbIconsDataAccess(ConnectionString, CommonFunctions);

                    var result = SQLTbIconsDataAccess.Get(page, itemsPerPage, orderBy, onlyNonDeleted);
                    if (result != null && result.Count > 0)
                    {
                        var total = SQLTbIconsDataAccess.Total();
                        var response = new { records = result, pageNumber = page, pageSize = itemsPerPage, totalRecords = total };
                        return new APIResponse(ResponseCode.SUCCESS, "Records Found", response);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }
        }

        public APIResponse Add(tbIconsModel model)
        {
            switch (ServerType)
            {
                case "SQLServer":
                    SQLTbIconsDataAccess = new TbIconsDataAccess(ConnectionString, CommonFunctions);

                    var result = SQLTbIconsDataAccess.Add(model);
                    if (result != null)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Inserted", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Inserted");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }
        }

        public APIResponse Update(Guid Id, tbIconsModel model)
        {
            switch (ServerType)
            {
                case "SQLServer":
                    SQLTbIconsDataAccess = new TbIconsDataAccess(ConnectionString, CommonFunctions);

                    var result = SQLTbIconsDataAccess.Update(Id,model);
                    if (result)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }
        }

        public APIResponse Delete(Guid Id)
        {
            switch (ServerType)
            {
                case "SQLServer":
                    SQLTbIconsDataAccess = new TbIconsDataAccess(ConnectionString, CommonFunctions);

                    var result = SQLTbIconsDataAccess.Delete(Id);
                    if (result)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }
        }

        public APIResponse Restore(Guid Id)
        {
            switch (ServerType)
            {
                case "SQLServer":
                    SQLTbIconsDataAccess = new TbIconsDataAccess(ConnectionString, CommonFunctions);

                    var result = SQLTbIconsDataAccess.Restore(Id);
                    if (result)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Record Restored", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "Record Not Restored");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }
        }
    }
}
