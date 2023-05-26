using AuthLayer.Models;
using AuthLayer.Utility;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthLayer.Mangers.Interface
{
    public interface IRoleManager
    {
        List<Guid> GetListOfRolesGUIDByAccessGUID(Guid AccessGUID);
        TbrolefeatureModel GetRoleFeatureFromFeatureName(string Feature);
        TbrolesubfeatureModel GetRoleSubFeatureFromFeatureIDAndSubFeatureName(int RoleFeatureId, string SubFeature);
        List<TbrolepermissionModel> GetRolePermissionsFromRolesGUIDAndSubfeatureId(List<Guid> CurrentUserRoles, int RoleSubFeatureId);
        APIResponse AddTbrole(string RoleName);
        APIResponse UpdateTbrole(string RoleName, Guid GUIDRole);
        APIResponse DeleteRole(Guid GUIDRole);
        List<UserData> GetListOfUserFromRoleGUID(Guid RoleGUID);
        List<TbroleModel> GetListOfRoles();
        APIResponse AddUserToRole(Guid GUIDRole, Guid GUIDAccess);
        APIResponse RemoveUserFromRole(Guid GUIDRole, Guid GUIDAccess);
        APIResponse CreateFeature(string FeatureName);
        APIResponse DeleteFeature(int id);
        APIResponse UpdateFeature(int id, string NewFeatureName);
        APIResponse GetRolesFeatures();
        APIResponse GetRolesSubFeatures();
        APIResponse GetRolesSubFeaturesByFeatureId(int featureId);
        APIResponse CreateSubFeature(string SubFeatureName, int featureId);
        APIResponse DeleteSubFeature(int id);
        APIResponse UpdateSubFeature(int id, string NewSubFeatureName);
        APIResponse UpdatePermission(Guid GUIDRole, int RoleSubFeatureId, bool Allow);
        APIResponse GetPremissionByRoleAndFeatureId(Guid GUIDRole, int RoleFeatureId);
        APIResponse GetOtherUsers(Guid GUIDRole);
        APIResponse GetMenuPages(string PageTypes);
        APIResponse GetAllMenuPages(string PageTypes);
        APIResponse GetIfUserHasAccessToRoute(string Route);
        APIResponse GetIfUserHasAccessToControllerAction(string ControllerName, string ActionName);
        APIResponse GetControllerActionBySubfeature(int RoleSubFeatureId);
        APIResponse UpdateControllerAction(ctbControllerActionModel model);
        APIResponse DeleteControllerAction(Guid GUIDControllerAction);
        APIResponse CreateControllerAction(ctbControllerActionModel model);
        APIResponse GetOtherControllerActionBySubfeature(int RoleSubFeatureId);
        APIResponse AddControllerActionToSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction);
        APIResponse RemoveControllerActionFromSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction);
        APIResponse GetRolesWithAllow(int RoleSubFeatureId);
        APIResponse GetUniqueMenuTypes();
        APIResponse UpdateMenuPage(CtbMenuPageModel model);
        APIResponse GetRolesWithAllowPages(int MenuPageId);
        APIResponse UpdateMenuPagesPermission(Guid GUIDRole, int MenuPageId, bool Allow);
        APIResponse CreateMenuPage(CtbMenuPageModel model);
        APIResponse DeleteMenuPage(int MenuPageId);
        APIResponse GetIfUserHasAccessToSubFeature(int RoleSubFeatureId);
        APIResponse GetLimitsBySubfeatureId(int SubFeatureID);
        APIResponse GetSubfeatureId(int SubFeatureID);
        APIResponse GetRoleByGUID(Guid RoleGUID);
        APIResponse GetDefaultsList(int LimitID);
        APIResponse AddUpdateLimitsValue(CtbrolelimitvalueModel model);
        APIResponse GetRoleLimitsValueByIDs(Guid GUIDRole, int SubfeatureLimitsId);
        APIResponse GetLimitsControllerActions(int LimitsId);
        APIResponse PostLimitsControllerActions(CtblimitscontrolleractionsModel model);
        APIResponse DeleteLimitsControllerActions(int LimitsId, Guid GUIDControllerAction);
        APIResponse GetIfUserHasAccessToControllerActionLimit(string ControllerName, string ActionName, AuthorizationFilterContext authorizationFilterContext);
        APIResponse GetFullLimitByName(string limitName);
    }
}

