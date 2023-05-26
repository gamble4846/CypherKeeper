using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;
using AuthLayer.DataAccess.Interface;
using AuthLayer.Mangers.Interface;
using AuthLayer.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using AuthLayer.Utility;

namespace AuthLayer.Mangers.Impl
{
    public class RoleManager : IRoleManager
    {
        private readonly IRoleDataAccess DataAccess = null;
        private readonly IUserDataAccess UserDataAccess = null;
        public IConfiguration Configuration;
        public string ContentRootPath;
        IHttpContextAccessor HttpContextAccessor;
        private CommonFunctions _cf;
        public IUserManager UserManager;

        public RoleManager(IRoleDataAccess dataAccess, IUserDataAccess userDataAccess, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHostingEnvironment env, IUserManager userManager)
        {
            DataAccess = dataAccess;
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            ContentRootPath = env.ContentRootPath;
            UserDataAccess = userDataAccess;
            UserManager = userManager;
            _cf = new CommonFunctions(configuration, env.ContentRootPath, httpContextAccessor);
        }

        public List<Guid> GetListOfRolesGUIDByAccessGUID(Guid AccessGUID)
        {
            try
            {
                var RoleAccessList = DataAccess.GetRolesByAccessGUID(AccessGUID);
                List<Guid> RoleGUIDList = RoleAccessList.Select(x => x.GUIDRole).ToList();
                return RoleGUIDList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TbrolefeatureModel GetRoleFeatureFromFeatureName(string Feature)
        {
            try
            {
                return DataAccess.GetRoleFeatureFromFeatureName(Feature);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TbrolesubfeatureModel GetRoleSubFeatureFromFeatureIDAndSubFeatureName(int RoleFeatureId, string SubFeature)
        {
            try
            {
                return DataAccess.GetRoleSubFeatureFromFeatureIDAndSubFeatureName(RoleFeatureId, SubFeature);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<TbrolepermissionModel> GetRolePermissionsFromRolesGUIDAndSubfeatureId(List<Guid> CurrentUserRoles, int RoleSubFeatureId)
        {
            try
            {
                return DataAccess.GetRolePermissionsFromRolesGUIDAndSubfeatureId(CurrentUserRoles, RoleSubFeatureId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public APIResponse AddTbrole(string RoleName)
        {
            var result = DataAccess.AddTbrole(RoleName);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse UpdateTbrole(string RoleName, Guid GUIDRole)
        {
            var model = new TbroleModel();
            model.RoleName = RoleName;
            model.GUIDRole = GUIDRole;

            var result = DataAccess.UpdateTbrole(model);
            if (result)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
            }
        }

        public List<UserData> GetListOfUserFromRoleGUID(Guid RoleGUID)
        {
            try
            {
                var RoleAccessList = DataAccess.GetListOfUsersFromRoleGUID(RoleGUID);
                var AccessGuidsList = RoleAccessList.Select(x => x.GUIDAccess).ToList();
                if (AccessGuidsList.Count == 0)
                {
                    return new List<UserData>();
                }
                else
                {
                    var Users = UserDataAccess.GetUsersByAccessGUIDList(AccessGuidsList);
                    if (Users.Count == 0)
                    {
                        return new List<UserData>();
                    }
                    return Users.Select(x => new UserData((Guid)x.GUIDAccess, x.ID, x.Name, x.EMail)).ToList();
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<TbroleModel> GetListOfRoles()
        {
            try
            {
                return DataAccess.GetListOfRoles();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public APIResponse AddUserToRole(Guid GUIDRole, Guid GUIDAccess)
        {
            var result = DataAccess.AddUserToRole(GUIDRole, GUIDAccess);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse RemoveUserFromRole(Guid GUIDRole, Guid GUIDAccess)
        {
            var result = DataAccess.RemoveUserFromRole(GUIDRole, GUIDAccess);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");
            }
        }

        public APIResponse CreateFeature(string FeatureName)
        {
            var result = DataAccess.CreateFeature(FeatureName);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse DeleteFeature(int id)
        {
            var result = DataAccess.DeleteFeature(id);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");
            }
        }

        public APIResponse UpdateFeature(int id, string NewFeatureName)
        {
            var result = DataAccess.UpdateFeature(id, NewFeatureName);
            if (result)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
            }
        }

        public APIResponse GetRolesFeatures()
        {
            var result = DataAccess.GetRolesFeatures();
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetRolesSubFeatures()
        {
            var result = DataAccess.GetRolesSubFeatures();
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetRolesSubFeaturesByFeatureId(int featureId)
        {
            var result = DataAccess.GetRolesSubFeaturesByFeatureId(featureId);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse CreateSubFeature(string SubFeatureName, int featureId)
        {
            var result = DataAccess.CreateSubFeature(SubFeatureName, featureId);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse DeleteSubFeature(int id)
        {
            var result = DataAccess.DeleteSubFeature(id);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");
            }
        }

        public APIResponse UpdateSubFeature(int id, string NewSubFeatureName)
        {
            var result = DataAccess.UpdateSubFeature(id, NewSubFeatureName);
            if (result)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
            }
        }

        public APIResponse UpdatePermission(Guid GUIDRole, int RoleSubFeatureId, bool Allow)
        {
            var CurrentPermission = DataAccess.GetPermissionsByGUIDRoleAndSubFeatureID(GUIDRole, RoleSubFeatureId).FirstOrDefault();

            if (CurrentPermission != null)
            {
                var result = DataAccess.UpdatePermission(CurrentPermission.RolePermissionId, Allow);
                if (result)
                {
                    return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
                }
                else
                {
                    return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                }
            }
            else
            {
                var result = DataAccess.CreateNewPermission(GUIDRole, RoleSubFeatureId, Allow);
                if (!string.IsNullOrEmpty(result))
                {
                    return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
                }
                else
                {
                    return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                }
            }
        }

        public APIResponse GetPremissionByRoleAndFeatureId(Guid GUIDRole, int RoleFeatureId)
        {
            var result = DataAccess.GetPremissionByRoleAndFeatureId(GUIDRole, RoleFeatureId);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetOtherUsers(Guid GUIDRole)
        {
            var result = DataAccess.GetOtherUsers(GUIDRole);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetMenuPages(string PageTypes)
        {
            var accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenData = _cf.GetTokenData(accessToken);
            var userId = tokenData.userId;
            var company = tokenData.CompanyData;
            var password = tokenData.password;
            var connectionString = _cf.GetNewConnectionString();

            TbaccessModel currentUser = UserManager.GetUserByIdAndPassword(userId, password, connectionString);

            if (currentUser != null && currentUser.GUIDAccess != null)
            {
                var RoleAccessList = DataAccess.GetRolesByAccessGUID(currentUser.GUIDAccess ?? new Guid());
                var RolePagePermissions = new List<CtbRolePagePermissionModel>();
                foreach (var roleAccess in RoleAccessList)
                {
                    var RolePagePermissionsIn = DataAccess.GetRolePagePermissionsByRoleGUID(roleAccess.GUIDRole);
                    RolePagePermissionsIn = RolePagePermissionsIn.Where(x => x.Allow).ToList();
                    foreach (var RolePagePermission in RolePagePermissionsIn)
                    {
                        RolePagePermissions.Add(RolePagePermission);
                    }
                }

                RolePagePermissions = RolePagePermissions.GroupBy(o => new { o.PageId }).Select(o => o.FirstOrDefault()).ToList();
                var pageIds = RolePagePermissions.Select(o => o.PageId).ToList();

                if (pageIds.Count == 0)
                {
                    return new APIResponse(ResponseCode.ERROR, "Record Not Found");
                }

                var MenuPages = DataAccess.GetMenuPagesByPageTypeAndPageIds(PageTypes, pageIds);

                foreach (var menuPage in MenuPages.ToList())
                {
                    if (menuPage.ParentId != null)
                    {
                        if (!MenuPages.Select(o => o.Id).ToList().Contains(menuPage.ParentId ?? 0))
                        {
                            MenuPages.Add(DataAccess.GetMenuPageById(menuPage.ParentId ?? 0));
                        }
                    }
                }

                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + MenuPages.Count + ")", MenuPages);
            }

            return new APIResponse(ResponseCode.ERROR, "Record Not Found");
        }

        public APIResponse GetAllMenuPages(string PageTypes)
        {
            var result = DataAccess.GetAllMenuPages(PageTypes);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetIfUserHasAccessToRoute(string Route)
        {
            Route = Route.Replace('|', '/');
            var accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenData = _cf.GetTokenData(accessToken);
            var userId = tokenData.userId;
            var company = tokenData.CompanyData;
            var password = tokenData.password;
            var connectionString = _cf.GetNewConnectionString();

            TbaccessModel currentUser = UserManager.GetUserByIdAndPassword(userId, password, connectionString);

            if (currentUser != null && currentUser.GUIDAccess != null)
            {
                var RoleAccessList = DataAccess.GetRolesByAccessGUID(currentUser.GUIDAccess ?? new Guid());
                var RolePagePermissions = new List<CtbRolePagePermissionModel>();
                foreach (var roleAccess in RoleAccessList)
                {
                    var RolePagePermissionsIn = DataAccess.GetRolePagePermissionsByRoleGUID(roleAccess.GUIDRole);
                    RolePagePermissionsIn = RolePagePermissionsIn.Where(x => x.Allow).ToList();
                    foreach (var RolePagePermission in RolePagePermissionsIn)
                    {
                        RolePagePermissions.Add(RolePagePermission);
                    }
                }

                RolePagePermissions = RolePagePermissions.GroupBy(o => new { o.PageId }).Select(o => o.FirstOrDefault()).ToList();
                var pageIds = RolePagePermissions.Select(o => o.PageId).ToList();

                var MenuPage = DataAccess.GetMenuPageByRoute(Route);
                if (MenuPage == null)
                {
                    return new APIResponse(ResponseCode.ERROR, "MenuPage Not Found");
                }

                var CanAccess = pageIds.Contains(MenuPage.Id);

                if (CanAccess)
                {
                    return new APIResponse(ResponseCode.SUCCESS, "User Can Access Route", CanAccess);
                }
                else
                {
                    return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Route", CanAccess);
                }
            }

            return new APIResponse(ResponseCode.ERROR, "Error Occured");
        }

        public APIResponse DeleteRole(Guid GUIDRole)
        {
            var result = DataAccess.DeleteRole(GUIDRole);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");

            }
        }

        public APIResponse GetIfUserHasAccessToSubFeature(int RoleSubFeatureId)
        {
            var _cs = new CommonFunctions(Configuration, ContentRootPath, HttpContextAccessor);
            var accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenData = _cs.GetTokenData(accessToken);
            var userId = tokenData.userId;
            var password = tokenData.password;
            var connectionString = _cs.GetNewConnectionString();

            TbaccessModel currentUser = UserManager.GetUserByIdAndPassword(userId, password, connectionString);
            if (currentUser == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access SubFeature (User Not Found)", false);
            }

            var CurrentUserRolesGUIDs = GetListOfRolesGUIDByAccessGUID((Guid)currentUser.GUIDAccess);
            if (CurrentUserRolesGUIDs.Count == 0 || CurrentUserRolesGUIDs == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access SubFeature (User Has No Role)", false);
            }

            var SubFeatureIdList = new List<int>();
            SubFeatureIdList.Add(RoleSubFeatureId);

            var RolePermissions = DataAccess.GetRolePermissionsByRoleGUIDListAndSubFeatureIdList(CurrentUserRolesGUIDs, SubFeatureIdList);
            if (RolePermissions.Count == 0 || RolePermissions == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access SubFeature (RolePermissions Not Found)", false);
            }

            return new APIResponse(ResponseCode.SUCCESS, "User Can Access SubFeature", true);
        }

        public APIResponse GetIfUserHasAccessToControllerAction(string ControllerName, string ActionName)
        {
            var _cs = new CommonFunctions(Configuration, ContentRootPath, HttpContextAccessor);
            var accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenData = _cs.GetTokenData(accessToken);
            var userId = tokenData.userId;
            var password = tokenData.password;
            var connectionString = _cs.GetNewConnectionString();

            TbaccessModel currentUser = UserManager.GetUserByIdAndPassword(userId, password, connectionString);
            if (currentUser == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (User Not Found)", false);
            }

            var CurrentUserRolesGUIDs = GetListOfRolesGUIDByAccessGUID((Guid)currentUser.GUIDAccess);
            if (CurrentUserRolesGUIDs.Count == 0 || CurrentUserRolesGUIDs == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (User Has No Role)", false);
            }

            var ControllerAction = DataAccess.GetControllerActionByName(ControllerName, ActionName);
            if (ControllerAction == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (Controller / Action Not Found)", false);
            }

            var subFeaturesWithAccessToAction = DataAccess.GetSubFeaturesWithAccessToAction(ControllerAction.GUIDControllerAction);
            if (subFeaturesWithAccessToAction.Count == 0 || subFeaturesWithAccessToAction == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (SubFeatures Not Found)", false);
            }

            var RolePermissions = DataAccess.GetRolePermissionsByRoleGUIDListAndSubFeatureIdList(CurrentUserRolesGUIDs, subFeaturesWithAccessToAction.Select(x => x.RoleSubFeatureId).ToList());
            if (RolePermissions.Count == 0 || RolePermissions == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (RolePermissions Not Found)", false);
            }


            return new APIResponse(ResponseCode.SUCCESS, "User Can Access Action", true);
        }

        public APIResponse GetControllerActionBySubfeature(int RoleSubFeatureId)
        {
            var result = DataAccess.GetControllerActionBySubfeature(RoleSubFeatureId);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse UpdateControllerAction(ctbControllerActionModel model)
        {
            var result = DataAccess.UpdateControllerAction(model);
            if (result)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
            }
        }

        public APIResponse DeleteControllerAction(Guid GUIDControllerAction)
        {
            var result = DataAccess.DeleteControllerAction(GUIDControllerAction);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");

            }
        }

        public APIResponse CreateControllerAction(ctbControllerActionModel model)
        {
            model.GUIDControllerAction = Guid.NewGuid();
            var result = DataAccess.CreateControllerAction(model);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse GetOtherControllerActionBySubfeature(int RoleSubFeatureId)
        {
            var result = DataAccess.GetOtherControllerActionBySubfeature(RoleSubFeatureId);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse AddControllerActionToSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction)
        {
            var result = DataAccess.AddControllerActionToSubFeature(RoleSubFeatureId, GUIDControllerAction);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse RemoveControllerActionFromSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction)
        {
            var result = DataAccess.RemoveControllerActionFromSubFeature(RoleSubFeatureId, GUIDControllerAction);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");

            }
        }

        public APIResponse GetRolesWithAllow(int RoleSubFeatureId)
        {
            var RolesWithAllow = new List<RoleWithAllow>();
            var Roles = DataAccess.GetListOfRoles();
            var PermissionsBySubFeatureId = DataAccess.PermissionsBySubFeatureId(RoleSubFeatureId);

            foreach (var role in Roles)
            {
                var roleWithAllow = new RoleWithAllow();
                roleWithAllow.GUIDRole = role.GUIDRole;
                roleWithAllow.RoleName = role.RoleName;

                var currentPermission = PermissionsBySubFeatureId.FirstOrDefault(x => x.GUIDRole == role.GUIDRole);
                if (currentPermission != null)
                {
                    roleWithAllow.Allow = currentPermission.Allow ?? false;
                }
                else
                {
                    roleWithAllow.Allow = false;
                }

                RolesWithAllow.Add(roleWithAllow);
            }

            if (RolesWithAllow != null && RolesWithAllow.Count > 0)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Found(" + RolesWithAllow.Count + ")", RolesWithAllow);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");

            }
        }

        public APIResponse GetUniqueMenuTypes()
        {
            var result = DataAccess.GetUniqueMenuTypes();
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse UpdateMenuPage(CtbMenuPageModel model)
        {
            var result = DataAccess.UpdateMenuPage(model);
            if (result)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
            }
        }

        public APIResponse GetRolesWithAllowPages(int MenuPageId)
        {
            var RolesWithAllow = new List<RoleWithAllow>();
            var Roles = DataAccess.GetListOfRoles();
            var Permissions = DataAccess.PagePermissionsByPageId(MenuPageId);

            foreach (var role in Roles)
            {
                var roleWithAllow = new RoleWithAllow();
                roleWithAllow.GUIDRole = role.GUIDRole;
                roleWithAllow.RoleName = role.RoleName;

                var currentPermission = Permissions.FirstOrDefault(x => x.GUIDRole == role.GUIDRole);
                if (currentPermission != null)
                {
                    roleWithAllow.Allow = currentPermission.Allow;
                }
                else
                {
                    roleWithAllow.Allow = false;
                }

                RolesWithAllow.Add(roleWithAllow);
            }

            if (RolesWithAllow != null && RolesWithAllow.Count > 0)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Found(" + RolesWithAllow.Count + ")", RolesWithAllow);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");

            }
        }

        public APIResponse UpdateMenuPagesPermission(Guid GUIDRole, int MenuPageId, bool Allow)
        {
            var CurrentPermission = DataAccess.GetMenuPermissionsByGUIDRoleAndMenuPageId(GUIDRole, MenuPageId).FirstOrDefault();

            if (CurrentPermission != null)
            {
                var result = DataAccess.UpdateMenuPermission(CurrentPermission.Id, Allow);
                if (result)
                {
                    return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
                }
                else
                {
                    return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                }
            }
            else
            {
                var result = DataAccess.CreateNewMenuPermission(GUIDRole, MenuPageId, Allow);
                if (!string.IsNullOrEmpty(result))
                {
                    return new APIResponse(ResponseCode.SUCCESS, "Record Updated", result);
                }
                else
                {
                    return new APIResponse(ResponseCode.ERROR, "Record Not Updated");
                }
            }
        }

        public APIResponse CreateMenuPage(CtbMenuPageModel model)
        {
            var result = DataAccess.CreateMenuPage(model);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse DeleteMenuPage(int MenuPageId)
        {
            var result = DataAccess.DeleteMenuPage(MenuPageId);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");

            }
        }

        public APIResponse GetLimitsBySubfeatureId(int SubFeatureID)
        {
            var result = DataAccess.GetLimitsBySubfeatureId(SubFeatureID);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Records Found (" + result.Count + ")", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetSubfeatureId(int SubFeatureID)
        {
            var result = DataAccess.GetSubfeatureId(SubFeatureID);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Found", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetRoleByGUID(Guid RoleGUID)
        {
            var result = DataAccess.GetRoleByGUID(RoleGUID);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Found", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Found");
            }
        }

        public APIResponse GetDefaultsList(int LimitID)
        {
            var LimitObject = DataAccess.GetLimitByID(LimitID);

            if (LimitObject == null)
            {
                return new APIResponse(ResponseCode.ERROR, "Limit Not Found");
            }

            if (LimitObject.DefaultsId == null)
            {
                return new APIResponse(ResponseCode.ERROR, "DefaultsId Not Found");
            }

            var DefaultsObject = DataAccess.GetDefaultsByID(LimitObject.DefaultsId ?? 0);

            if (DefaultsObject == null)
            {
                return new APIResponse(ResponseCode.ERROR, "Defaults Not Found");
            }

            switch (DefaultsObject.Type)
            {
                case "List":
                    {
                        var ListData = DefaultsObject.Values.Split(',').ToList();
                        dynamic toReturn = new System.Dynamic.ExpandoObject();
                        toReturn.ListData = ListData;
                        toReturn.DefaultsObject = DefaultsObject;
                        return new APIResponse(ResponseCode.SUCCESS, "Defaults Found", toReturn);
                    }


                case "Table":
                    {
                        var ListData = DataAccess.GetDefualtDataFromTables(DefaultsObject);
                        dynamic toReturn = new System.Dynamic.ExpandoObject();
                        toReturn.ListData = ListData;
                        toReturn.DefaultsObject = DefaultsObject;
                        return new APIResponse(ResponseCode.SUCCESS, "Defaults Found", toReturn);
                    }

                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Default Type");
            }
        }

        public APIResponse AddUpdateLimitsValue(CtbrolelimitvalueModel model)
        {
            var LimitsData = DataAccess.GetRoleLimitsValueByGUIDRoleANDLimitsID(model.GUIDRole, model.SubfeatureLimitsId);

            if (LimitsData == null)
            {
                var result = DataAccess.CreateRoleLimitsValue(model);
                if (!string.IsNullOrEmpty(result))
                {
                    return new APIResponse(ResponseCode.SUCCESS, "Limit Created", result);
                }
                else
                {
                    return new APIResponse(ResponseCode.ERROR, "Limit Not Created");
                }
            }
            else
            {
                LimitsData.Value = model.Value;
                var result = DataAccess.UpdateRoleLimitsValue(LimitsData.Id, LimitsData.Value);
                if (result)
                {
                    return new APIResponse(ResponseCode.SUCCESS, "Limit Updated", result);
                }
                else
                {
                    return new APIResponse(ResponseCode.ERROR, "Limit Not Updated");
                }
            }
        }

        public APIResponse GetRoleLimitsValueByIDs(Guid GUIDRole, int SubfeatureLimitsId)
        {
            var result = DataAccess.GetRoleLimitsValueByGUIDRoleANDLimitsID(GUIDRole, SubfeatureLimitsId);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Found", result);
            }
            else
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Not Found");
            }
        }

        public APIResponse GetLimitsControllerActions(int LimitsId)
        {
            var result = DataAccess.GetLimitsControllerActions(LimitsId);
            if (result != null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Found", result);
            }
            else
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Not Found");
            }
        }

        public APIResponse PostLimitsControllerActions(CtblimitscontrolleractionsModel model)
        {
            var result = DataAccess.PostLimitsControllerActions(model);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Created", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Created");
            }
        }

        public APIResponse DeleteLimitsControllerActions(int LimitsId, Guid GUIDControllerAction)
        {
            var result = DataAccess.DeleteLimitsControllerActions(LimitsId, GUIDControllerAction);
            if (!string.IsNullOrEmpty(result))
            {
                return new APIResponse(ResponseCode.SUCCESS, "Record Deleted", result);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Record Not Deleted");

            }
        }

        public APIResponse GetIfUserHasAccessToControllerActionLimit(string ControllerName, string ActionName, AuthorizationFilterContext authorizationFilterContext)
        {
            var _cs = new CommonFunctions(Configuration, ContentRootPath, HttpContextAccessor);
            var accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenData = _cs.GetTokenData(accessToken);
            var userId = tokenData.userId;
            var password = tokenData.password;
            var connectionString = _cs.GetNewConnectionString();

            TbaccessModel currentUser = UserManager.GetUserByIdAndPassword(userId, password, connectionString);
            if (currentUser == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (User Not Found)", false);
            }

            var CurrentUserRolesGUIDs = GetListOfRolesGUIDByAccessGUID((Guid)currentUser.GUIDAccess);
            if (CurrentUserRolesGUIDs.Count == 0 || CurrentUserRolesGUIDs == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (User Has No Role)", false);
            }

            var ControllerAction = DataAccess.GetControllerActionByName(ControllerName, ActionName);
            if (ControllerAction == null)
            {
                return new APIResponse(ResponseCode.SUCCESS, "User Can't Access Action (Controller / Action Not Found)", false);
            }

            var LimitsControllerActions = DataAccess.GetLimitsControllerActionsFromGUIDControllerAction(ControllerAction.GUIDControllerAction);
            if (LimitsControllerActions == null || LimitsControllerActions.Count == 0)
            {
                return new APIResponse(ResponseCode.SUCCESS, "There are no Limits", true);
            }

            var limitIdsString = string.Join(",", LimitsControllerActions.Select(x => x.LimitsId).ToList());
            var SubfeatureLimits = DataAccess.GetSubFeatureLimitsFromLimitIds(limitIdsString);

            var limitsWithValues = new List<LimitsWithValues>();

            foreach (var subfeatureLimit in SubfeatureLimits)
            {
                var Values = new List<CtbrolelimitvalueModel>();
                foreach (var roleGUID in CurrentUserRolesGUIDs)
                {
                    var value = DataAccess.GetRoleLimitsValueByGUIDRoleANDLimitsID(roleGUID, subfeatureLimit.Id);
                    if (value != null)
                    {
                        Values.Add(value);
                    }
                }
                var limitsWithValue = new LimitsWithValues(subfeatureLimit, Values);
                limitsWithValues.Add(limitsWithValue);
            }

            var SubfeatureLimitsGrouped = limitsWithValues.GroupBy(x => x.Limit.VaraibleName).ToList();

            foreach (var limitWithValue in limitsWithValues)
            {
                var isValid = false;
                var currentValue = UtilityCustom.GetValueFromAuthContextAndVariableName(authorizationFilterContext, limitWithValue.Limit.VaraibleName);
                foreach (var values in limitWithValue.Values)
                {
                    switch (limitWithValue.Limit.Operator)
                    {
                        case "=":
                            {

                            }
                            break;
                        default: continue;
                    }
                }
            }

            var x = 0;

            return new APIResponse(ResponseCode.SUCCESS, "Granted", true);
        }

        public APIResponse GetFullLimitByName(string limitName)
        {
            var _cs = new CommonFunctions(Configuration, ContentRootPath, HttpContextAccessor);
            var accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenData = _cs.GetTokenData(accessToken);
            var userId = tokenData.userId;
            var password = tokenData.password;
            var connectionString = _cs.GetNewConnectionString();

            TbaccessModel currentUser = UserManager.GetUserByIdAndPassword(userId, password, connectionString);
            if (currentUser == null)
            {
                return new APIResponse(ResponseCode.ERROR, "User Not Found", null);
            }

            var CurrentUserRolesGUIDs = GetListOfRolesGUIDByAccessGUID((Guid)currentUser.GUIDAccess);
            if (CurrentUserRolesGUIDs.Count == 0 || CurrentUserRolesGUIDs == null)
            {
                return new APIResponse(ResponseCode.ERROR, "User Has No Role", null);
            }

            var SubFeatureLimit = DataAccess.GetSubFeatureLimitsFromLimitName(limitName);
            if (SubFeatureLimit == null)
            {
                return new APIResponse(ResponseCode.ERROR, "SubFeatureLimit Not Found", null);
            }

            var DefaultsManagerCall = GetDefaultsList(SubFeatureLimit.Id);
            dynamic DefaultsData = null;
            if (DefaultsManagerCall.Code == ResponseCode.SUCCESS)
            {
                DefaultsData = DefaultsManagerCall.Document;
            }

            var Values = new List<CtbrolelimitvalueModel>();
            foreach (var roleGUID in CurrentUserRolesGUIDs)
            {
                var value = DataAccess.GetRoleLimitsValueByGUIDRoleANDLimitsID(roleGUID, SubFeatureLimit.Id);
                if (value != null)
                {
                    Values.Add(value);
                }
            }

            var FullLimit = new FullLimitModel(SubFeatureLimit, DefaultsData, Values);


            return new APIResponse(ResponseCode.SUCCESS, "FullLimit Found", FullLimit);
        }
    }
}

