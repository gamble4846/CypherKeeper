using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using AuthLayer.DataAccess.Interface;
using AuthLayer.Models;
using EasyCrudLibrary;
using AuthLayer.Utility;

namespace AuthLayer.DataAccess.Impl
{
    public class RoleDataAccess : IRoleDataAccess
    {
        private CommonFunctions _cf { get; set; }
        private string ConnectionString { get; set; }

        public RoleDataAccess(IHostingEnvironment env, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                _cf = new CommonFunctions(configuration, env.ContentRootPath, httpContextAccessor);
                ConnectionString = _cf.GetNewConnectionString();
            }
            catch (Exception) { }
        }

        public List<RoleaccessModel> GetRolesByAccessGUID(Guid AccessGUID)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@AccessGUID", AccessGUID));
            return _EC.GetList<RoleaccessModel>(-1, -1, null,"WHERE [GUIDAccess] = @AccessGUID", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public TbrolefeatureModel GetRoleFeatureFromFeatureName(string Feature)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Feature", Feature));
            return _EC.GetFirstOrDefault<TbrolefeatureModel>("WHERE RoleFeatureName = @Feature", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public TbrolesubfeatureModel GetRoleSubFeatureFromFeatureIDAndSubFeatureName(int RoleFeatureId, string SubFeature)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleFeatureId", RoleFeatureId));
            Parameters.Add(new SqlParameter("@SubFeature", SubFeature));
            return _EC.GetFirstOrDefault<TbrolesubfeatureModel>("WHERE RoleFeatureId = @RoleFeatureId AND RoleSubFeatureName = @SubFeature", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<TbrolepermissionModel> GetRolePermissionsFromRolesGUIDAndSubfeatureId(List<Guid> CurrentUserRoles, int RoleSubFeatureId)
        {
            var _EC = new EasyCrud(ConnectionString);
            string GUIDRole = "('" + string.Join("','", CurrentUserRoles) + "')";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", RoleSubFeatureId));
            string WhereCondition = "WHERE GUIDRole IN " + GUIDRole + " AND RoleSubFeatureId = @RoleSubFeatureId";
            return _EC.GetList<TbrolepermissionModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public string AddTbrole(string RoleName)
        {
            var _EC = new EasyCrud(ConnectionString);
            var data = new TbroleModel()
            {
                RoleName = RoleName,
            };
            var recs = _EC.Add(data, "GUIDRole", "GUIDRole", true);
            return recs.ToString();
        }

        public bool UpdateTbrole(TbroleModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", model.GUIDRole));
            var recs = _EC.Update(model, " Where GUIDRole = @GUIDRole", Parameters, "GUIDRole", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public List<RoleaccessModel> GetListOfUsersFromRoleGUID(Guid RoleGUID)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleGUID", RoleGUID));
            string WhereCondition = "WHERE [GUIDRole] = @RoleGUID";
            return _EC.GetList<RoleaccessModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<TbroleModel> GetListOfRoles()
        {
            var _EC = new EasyCrud(ConnectionString);
            return _EC.GetList<TbroleModel>(-1, -1, null, "", null, GSEnums.WithInQuery.ReadPast);
        }

        public string AddUserToRole(Guid GUIDRole, Guid GUIDAccess)
        {
            var _EC = new EasyCrud(ConnectionString);
            var data = new ctbRoleAccessModel()
            {
                GUIDRole = GUIDRole,
                GUIDAccess = GUIDAccess
            };
            var recs = _EC.Add(data, "GUIDRoleAccess", "GUIDRoleAccess", true);
            return recs.ToString();
        }

        public string RemoveUserFromRole(Guid GUIDRole, Guid GUIDAccess)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            Parameters.Add(new SqlParameter("@GUIDAccess", GUIDAccess));
            var deleted = _EC.Remove<ctbRoleAccessModel>(" WHERE GUIDRole = @GUIDRole AND GUIDAccess = @GUIDAccess ", "GUIDRole", Parameters, true);
            if(deleted != null)
            {
                return deleted[0];
            }
            else
            {
                return null;
            }
        }

        public string CreateFeature(string FeatureName)
        {
            var _EC = new EasyCrud(ConnectionString);
            var data = new TbrolefeatureModel()
            {
                RoleFeatureName = FeatureName,
            };
            var recs = _EC.Add(data, "RoleFeatureId", "RoleFeatureId", true);
            return recs.ToString();
        }

        public string DeleteFeature(int id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleFeatureId", id));
            var deleted = _EC.Remove<TbrolefeatureModel>(" WHERE RoleFeatureId = @RoleFeatureId ", "RoleFeatureId", Parameters, true);
            if (deleted != null)
            {
                return deleted[0];
            }
            else
            {
                return null;
            }
        }

        public bool UpdateFeature(int id, string NewFeatureName)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleFeatureId", id));
            var model = new TbrolefeatureModel()
            {
                RoleFeatureName = NewFeatureName,
                RoleFeatureId = 0,
            };
            var recs = _EC.Update(model, " WHERE RoleFeatureId = @RoleFeatureId ", Parameters, "RoleFeatureId", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public List<TbrolefeatureModel> GetRolesFeatures()
        {
            var _EC = new EasyCrud(ConnectionString);
            return _EC.GetList<TbrolefeatureModel>(-1, -1, null, "", null, GSEnums.WithInQuery.ReadPast);
        }

        public List<TbrolesubfeatureModel> GetRolesSubFeatures()
        {
            var _EC = new EasyCrud(ConnectionString);
            return _EC.GetList<TbrolesubfeatureModel>(-1, -1, null, "", null, GSEnums.WithInQuery.ReadPast);
        }

        public List<TbrolesubfeatureModel> GetRolesSubFeaturesByFeatureId(int featureId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleFeatureId", featureId));
            return _EC.GetList<TbrolesubfeatureModel>(-1, -1, null, "WHERE RoleFeatureId = @RoleFeatureId", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public string CreateSubFeature(string SubFeatureName, int featureId)
        {
            var _EC = new EasyCrud(ConnectionString);
            var data = new TbrolesubfeatureModel()
            {
                RoleSubFeatureId = 0,
                RoleFeatureId = featureId,
                RoleSubFeatureName = SubFeatureName,
            };
            var recs = _EC.Add(data, "RoleSubFeatureId", "RoleSubFeatureId", true);
            return recs.ToString();
        }

        public string DeleteSubFeature(int id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", id));
            var deleted = _EC.Remove<TbrolesubfeatureModel>(" WHERE RoleSubFeatureId = @RoleSubFeatureId ", "RoleSubFeatureId", Parameters, true);
            if (deleted != null)
            {
                return deleted[0];
            }
            else
            {
                return null;
            }
        }

        public bool UpdateSubFeature(int id, string NewSubFeatureName)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", id));
            var model = new TbrolesubfeatureModel()
            {
                RoleSubFeatureId = id,
                RoleFeatureId = 0,
                RoleSubFeatureName = NewSubFeatureName
            };
            var recs = _EC.Update(model, " WHERE RoleSubFeatureId = @RoleSubFeatureId ", Parameters, "RoleSubFeatureId,RoleFeatureId", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public List<TbrolepermissionModel> GetPermissionsByGUIDRoleAndSubFeatureID(Guid GUIDRole, int RoleSubFeatureId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", RoleSubFeatureId));
            string WhereCondition = " WHERE GUIDRole = @GUIDRole AND RoleSubFeatureId = @RoleSubFeatureId ";
            return _EC.GetList<TbrolepermissionModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public bool UpdatePermission(int RolePermissionId, bool Allow)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RolePermissionId", RolePermissionId));
            var model = new TbrolepermissionModel()
            {
                RolePermissionId = RolePermissionId,
                RoleSubFeatureId = 0,
                Allow = Allow,
                GUIDRole = new Guid(),
            };
            var recs = _EC.Update(model, " WHERE RolePermissionId = @RolePermissionId ", Parameters, "GUIDRole,RolePermissionId,RoleSubFeatureId", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public string CreateNewPermission(Guid GUIDRole, int RoleSubFeatureId, bool Allow)
        {
            var _EC = new EasyCrud(ConnectionString);
            var data = new TbrolepermissionModel()
            {
                RolePermissionId = 0,
                RoleSubFeatureId = RoleSubFeatureId,
                Allow = Allow,
                GUIDRole = GUIDRole,
            };
            var recs = _EC.Add(data, "RolePermissionId", "RolePermissionId", true);
            return recs.ToString();
        }

        public List<TbrolepermissionModel> GetPremissionByRoleAndFeatureId(Guid GUIDRole, int RoleFeatureId)
        {
            var _EC = new EasyCrud(ConnectionString);
            string CommandText = @"Select RP.* From ctbRolePermission RP JOIN tbRoleSubFeature RSF On RP.RoleSubFeatureId = RSF.RoleSubFeatureId WITH(nolock) WHERE RSF.RoleFeatureId = @RoleFeatureId AND RP.GUIDRole = @GUIDRole";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            Parameters.Add(new SqlParameter("@RoleFeatureId", RoleFeatureId));
            var data = _EC.Query(CommandText, Parameters, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
                return (List<TbrolepermissionModel>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<TbrolepermissionModel>>(data);
            else
                return null;
        }

        public List<UserData> GetOtherUsers(Guid GUIDRole)
        {
            var _EC = new EasyCrud(ConnectionString);
            string CommandText = @"Select * from tbAccess WITH(nolock) where GUIDAccess NOT IN (Select GUIDAccess From ctbRoleAccess where GUIDRole = @GUIDRole)";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            var data = _EC.Query(CommandText, Parameters, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
                return (List<UserData>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<UserData>>(data);
            else
                return null;
        }

        public List<CtbRolePagePermissionModel> GetRolePagePermissionsByRoleGUID(Guid GUIDRole)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            string WhereCondition = " where GUIDRole = @GUIDRole ";
            return _EC.GetList<CtbRolePagePermissionModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<CtbMenuPageModel> GetMenuPagesByPageTypeAndPageIds(string PageTypes, List<int> PageIds)
        {
            var _EC = new EasyCrud(ConnectionString);
            var listPageTypes = PageTypes.Split(',');
            string CommandText = @"Select * from ctbMenuPages WITH(nolock) where Type IN ('" + string.Join("','", listPageTypes) + "') AND Id IN (" + string.Join(",", PageIds) + ") AND showPageInMenu = 1";
            var data = _EC.Query(CommandText, null, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
                return (List<CtbMenuPageModel>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<CtbMenuPageModel>>(data);
            else
                return null;
        }

        public CtbMenuPageModel GetMenuPageById(int id)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@id", id));
            return _EC.GetFirstOrDefault<CtbMenuPageModel>(" where Id = @id ", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public CtbMenuPageModel GetMenuPageByRoute(string Route)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Route", Route));
            return _EC.GetFirstOrDefault<CtbMenuPageModel>(" WHERE RelativeRoute = @Route ", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public string DeleteRole(Guid GUIDRole)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            var deleted = _EC.Remove<TbroleModel>(" WHERE GUIDRole = @GUIDRole ", "GUIDRole", Parameters, true);
            if (deleted != null)
            {
                return deleted[0];
            }
            else
            {
                return null;
            }
        }

        public ctbControllerActionModel GetControllerActionByName(string ControllerName, string ActionName)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@ControllerName", ControllerName));
            Parameters.Add(new SqlParameter("@ActionName", ActionName));
            return _EC.GetFirstOrDefault<ctbControllerActionModel>("WHERE ControllerName = @ControllerName AND ActionName = @ActionName", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<TbrolesubfeatureModel> GetSubFeaturesWithAccessToAction(Guid GUIDControllerAction)
        {
            var _EC = new EasyCrud(ConnectionString);
            string CommandText = @"Select * from ctbRoleSubFeature WITH(nolock) where RoleSubFeatureId IN (Select RoleSubFeatureId From ctbRoleSubFeatureControllerAction Where GUIDControllerAction = @GUIDControllerAction)";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDControllerAction", GUIDControllerAction));
            var data = _EC.Query(CommandText, Parameters, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
                return (List<TbrolesubfeatureModel>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<TbrolesubfeatureModel>>(data);
            else
                return null;
        }

        public List<TbrolepermissionModel> GetRolePermissionsByRoleGUIDListAndSubFeatureIdList(List<Guid> CurrentUserRolesGUIDs, List<int> SubFeaturesIDs)
        {
            var _EC = new EasyCrud(ConnectionString); //WITH(nolock)
            string CommandText = @"SELECT * FROM ctbRolePermission WHERE GUIDRole IN ('" + string.Join("','", CurrentUserRolesGUIDs) + @"')  AND RoleSubFeatureID IN ('" + string.Join("','", SubFeaturesIDs) + @"') AND Allow = 1";
            var data = _EC.Query(CommandText, null, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
                return (List<TbrolepermissionModel>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<TbrolepermissionModel>>(data);
            else
                return null;
        }

        public List<ctbControllerActionModel> GetControllerActionBySubfeature(int RoleSubFeatureId)
        {
            var _EC = new EasyCrud(ConnectionString); //WITH(nolock)
            string CommandText = @"SELECT * FROM [ctbControllerAction] WITH(nolock) WHERE GUIDControllerAction IN (Select GUIDControllerAction FROM [ctbRoleSubFeatureControllerAction] WHERE [RoleSubFeatureId] = @RoleSubFeatureId)";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", RoleSubFeatureId));
            var data = _EC.Query(CommandText, Parameters, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
                return (List<ctbControllerActionModel>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<ctbControllerActionModel>>(data);
            else
                return null;
        }

        public bool UpdateControllerAction(ctbControllerActionModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDControllerAction", model.GUIDControllerAction));
            var recs = _EC.Update(model, " WHERE [GUIDControllerAction] = @GUIDControllerAction ", Parameters, "GUIDControllerAction", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public string DeleteControllerAction(Guid GUIDControllerAction)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDControllerAction", GUIDControllerAction));
            var deleted = _EC.Remove<ctbControllerActionModel>(" WHERE GUIDControllerAction = @GUIDControllerAction ", "GUIDControllerAction", Parameters, true);
            if (deleted != null)
            {
                return deleted[0];
            }
            else
            {
                return null;
            }
        }

        public string CreateControllerAction(ctbControllerActionModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            var recs = _EC.Add(model, "GUIDControllerAction", "", true);
            return recs.ToString();
        }

        public List<ctbControllerActionModel> GetOtherControllerActionBySubfeature(int RoleSubFeatureId)
        {
            var _EC = new EasyCrud(ConnectionString); //WITH(nolock)
            string CommandText = @"SELECT * FROM [ctbControllerAction] WITH(nolock) WHERE GUIDControllerAction NOT IN (Select GUIDControllerAction FROM [ctbRoleSubFeatureControllerAction] WHERE [RoleSubFeatureId] = @RoleSubFeatureId)";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", RoleSubFeatureId));
            var data = _EC.Query(CommandText, Parameters, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
                return (List<ctbControllerActionModel>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<ctbControllerActionModel>>(data);
            else
                return null;
        }

        public string AddControllerActionToSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction)
        {
            var _EC = new EasyCrud(ConnectionString);
            var data = new ctbRoleSubFeatureControllerActionModel()
            {
                SubFeatureControllerActionID = 0,
                RoleSubFeatureId = RoleSubFeatureId,
                GUIDControllerAction = GUIDControllerAction,
            };
            var recs = _EC.Add(data, "SubFeatureControllerActionID", "SubFeatureControllerActionID", true);
            return recs.ToString();
        }

        public string RemoveControllerActionFromSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDControllerAction", GUIDControllerAction));
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", RoleSubFeatureId));
            var deleted = _EC.Remove<ctbRoleAccessModel>(" WHERE RoleSubFeatureId = @RoleSubFeatureId AND GUIDControllerAction = @GUIDControllerAction ", "GUIDControllerAction", Parameters, true);
            if (deleted != null)
                return deleted[0];
            else
                return null;
        }

        public List<TbrolepermissionModel> PermissionsBySubFeatureId(int RoleSubFeatureId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleSubFeatureId", RoleSubFeatureId));
            string WhereCondition = " WHERE [RoleSubFeatureId] = @RoleSubFeatureId ";
            return _EC.GetList<TbrolepermissionModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<string> GetUniqueMenuTypes()
        {
            var ret = new List<string>();
            var _EC = new EasyCrud(ConnectionString); //WITH(nolock)
            string CommandText = @"Select DISTINCT [Type] From [ctbMenuPages] WITH(nolock)";
            var data = _EC.Query(CommandText, null, true, GSEnums.ExecuteType.ExecuteReader);
            if (data != null)
            {
                var MenuTypes = (List<MenuPageType>)EasyCrudLibrary.Utility.ConvertDynamicToType<List<MenuPageType>>(data);
                foreach(var menuType in MenuTypes)
                {
                    ret.Add(menuType.Type);
                }
                return ret;
            }
            else
                return null;
        }

        public List<CtbMenuPageModel> GetAllMenuPages(string PageTypes)
        {
            var _EC = new EasyCrud(ConnectionString);
            var PageTypesList = PageTypes.Split(',');
            string WhereCondition = " WHERE Type IN ('" + string.Join("','", PageTypesList) + @"') ";
            return _EC.GetList<CtbMenuPageModel>(-1, -1, null, WhereCondition, null, GSEnums.WithInQuery.ReadPast);
        }

        public bool UpdateMenuPage(CtbMenuPageModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", model.Id));
            var recs = _EC.Update(model, " WHERE [Id] = @Id ", Parameters, "Id", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public List<CtbRolePagePermissionModel> PagePermissionsByPageId(int MenuPageId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@MenuPageId", MenuPageId));
            string WhereCondition = " WHERE [PageId] = @MenuPageId  ";
            return _EC.GetList<CtbRolePagePermissionModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<CtbRolePagePermissionModel> GetMenuPermissionsByGUIDRoleAndMenuPageId(Guid GUIDRole, int MenuPageId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@PageId", MenuPageId));
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            string WhereCondition = "  WHERE [PageId] = @PageId AND [GUIDRole] = @GUIDRole  ";
            return _EC.GetList<CtbRolePagePermissionModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public bool UpdateMenuPermission(int MenuPageId, bool Allow)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@MenuPageId", MenuPageId));
            var model = new CtbRolePagePermissionModel()
            {
                Id = MenuPageId,
                GUIDRole = new Guid(),
                PageId = 0,
                Allow = Allow,
            };
            var recs = _EC.Update(model, " WHERE Id = @MenuPageId ", Parameters, "Id, GUIDRole, PageId", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public string CreateNewMenuPermission(Guid GUIDRole, int MenuPageId, bool Allow)
        {
            var _EC = new EasyCrud(ConnectionString);
            var model = new CtbRolePagePermissionModel()
            {
                Id = MenuPageId,
                GUIDRole = GUIDRole,
                PageId = MenuPageId,
                Allow = Allow,
            };
            var recs = _EC.Add(model, "Id", "Id", true);
            return recs.ToString();
        }

        public string CreateMenuPage(CtbMenuPageModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            var recs = _EC.Add(model, "Id", "Id", true);
            return recs.ToString();
        }

        public string DeleteMenuPage(int MenuPageId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", MenuPageId));
            var deleted = _EC.Remove<CtbMenuPageModel>(" WHERE Id = @Id ", "Id", Parameters, true);
            if (deleted != null)
                return deleted[0];
            else
                return null;
        }

        public List<CtbsubfeaturelimitsModel> GetLimitsBySubfeatureId(int SubFeatureID)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@SubfeatureId", SubFeatureID));
            string WhereCondition = "WHERE SubfeatureId = @SubfeatureId";
            return _EC.GetList<CtbsubfeaturelimitsModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public TbrolesubfeatureModel GetSubfeatureId(int SubFeatureID)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@SubFeatureID", SubFeatureID));
            return _EC.GetFirstOrDefault<TbrolesubfeatureModel>(" Where RoleSubFeatureId = @SubFeatureID ", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public TbroleModel GetRoleByGUID(Guid RoleGUID)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@RoleGUID", RoleGUID));
            return _EC.GetFirstOrDefault<TbroleModel>("WHERE GUIDRole = @RoleGUID", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public CtbsubfeaturelimitsModel GetLimitByID(int LimitID)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@LimitID", LimitID));
            return _EC.GetFirstOrDefault<CtbsubfeaturelimitsModel>(" where Id = @LimitID ", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public CtblimitdefaultsModel GetDefaultsByID(int DefaultsId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@DefaultsId", DefaultsId));
            return _EC.GetFirstOrDefault<CtblimitdefaultsModel>("WHERE ID = @DefaultsId", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<dynamic> GetDefualtDataFromTables(CtblimitdefaultsModel DefaultsObject)
        {
            var listData = new List<dynamic>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string CommandText = @"SELECT * FROM " + DefaultsObject.TableName;
                SqlCommand cmd = new SqlCommand(CommandText, connection);

                if (!string.IsNullOrEmpty(DefaultsObject.Condition))
                {
                    cmd.CommandText += " WHERE " + DefaultsObject.Condition;
                }


                try
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var t = UtilityCustom.GetFullQueryRow(reader);
                            listData.Add(t);
                        }
                    }
                }
                catch (Exception)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    throw;
                }
                finally
                {
                    connection.Close();
                }

                return listData;
            }
        }

        public CtbrolelimitvalueModel GetRoleLimitsValueByGUIDRoleANDLimitsID(Guid GUIDRole, int SubfeatureLimitsId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDRole", GUIDRole));
            Parameters.Add(new SqlParameter("@SubfeatureLimitsId", SubfeatureLimitsId));
            return _EC.GetFirstOrDefault<CtbrolelimitvalueModel>(" WHERE GUIDRole = @GUIDRole AND SubfeatureLimitsId = @SubfeatureLimitsId ", Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public string CreateRoleLimitsValue(CtbrolelimitvalueModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            var recs = _EC.Add(model, "Id", "Id", true);
            return recs.ToString();
        }

        public bool UpdateRoleLimitsValue(int Id, string Value)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Id", Id));
            var model = new CtbrolelimitvalueModel()
            {
                Id = 0,
                Value = Value,
                SubfeatureLimitsId = 0,
                GUIDRole = new Guid()
            };
            var recs = _EC.Update(model, " WHERE Id = @Id ", Parameters, "Id, SubfeatureLimitsId, GUIDRole", true);
            if (recs == null)
                return false;
            else
                return true;
        }

        public List<CtblimitscontrolleractionsModel> GetLimitsControllerActions(int LimitsId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@LimitsId", LimitsId));
            string WhereCondition = "WHERE LimitsId = @LimitsId";
            return _EC.GetList<CtblimitscontrolleractionsModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public string PostLimitsControllerActions(CtblimitscontrolleractionsModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            var recs = _EC.Add(model, "Id", "Id", true);
            return recs.ToString();
        }

        public string DeleteLimitsControllerActions(int LimitsId, Guid GUIDControllerAction)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@LimitsId", LimitsId));
            Parameters.Add(new SqlParameter("@GUIDControllerAction", GUIDControllerAction));
            var deleted = _EC.Remove<CtblimitscontrolleractionsModel>(" WHERE LimitsId = @LimitsId AND GUIDControllerAction=@GUIDControllerAction ", "Id", Parameters, true);
            if (deleted != null)
                return deleted[0];
            else
                return null;
        }

        public List<CtblimitscontrolleractionsModel> GetLimitsControllerActionsFromGUIDControllerAction(Guid GUIDControllerAction)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@GUIDControllerAction", GUIDControllerAction));
            string WhereCondition = "WHERE GUIDControllerAction = @GUIDControllerAction";
            return _EC.GetList<CtblimitscontrolleractionsModel>(-1, -1, null, WhereCondition, Parameters, GSEnums.WithInQuery.ReadPast);
        }

        public List<CtbsubfeaturelimitsModel> GetSubFeatureLimitsFromLimitIds(string LimitIds)
        {
            var _EC = new EasyCrud(ConnectionString);
            string WhereCondition = "where Id IN (" + LimitIds + ")";
            return _EC.GetList<CtbsubfeaturelimitsModel>(-1, -1, null, WhereCondition, null, GSEnums.WithInQuery.ReadPast);
        }

        public CtbsubfeaturelimitsModel GetSubFeatureLimitsFromLimitName(string limitName)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@LimitName", limitName));
            return _EC.GetFirstOrDefault<CtbsubfeaturelimitsModel>("where LimitName = @LimitName", Parameters, GSEnums.WithInQuery.ReadPast);
        }
    }
}

