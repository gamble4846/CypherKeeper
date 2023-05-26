using System.Collections.Generic;
using System;
using AuthLayer.Models;

namespace AuthLayer.DataAccess.Interface
{
    public interface IRoleDataAccess
    {
        List<RoleaccessModel> GetRolesByAccessGUID(Guid AccessGUID);
        TbrolefeatureModel GetRoleFeatureFromFeatureName(string Feature);
        TbrolesubfeatureModel GetRoleSubFeatureFromFeatureIDAndSubFeatureName(int RoleFeatureId, string SubFeature);
        List<TbrolepermissionModel> GetRolePermissionsFromRolesGUIDAndSubfeatureId(List<Guid> CurrentUserRoles, int RoleSubFeatureId);
        string AddTbrole(string RoleName);
        bool UpdateTbrole(TbroleModel model);
        List<RoleaccessModel> GetListOfUsersFromRoleGUID(Guid RoleGUID);
        List<TbroleModel> GetListOfRoles();
        string AddUserToRole(Guid GUIDRole, Guid GUIDAccess);
        string RemoveUserFromRole(Guid GUIDRole, Guid GUIDAccess);
        string CreateFeature(string FeatureName);
        string DeleteFeature(int id);
        bool UpdateFeature(int id, string NewFeatureName);
        List<TbrolefeatureModel> GetRolesFeatures();
        List<TbrolesubfeatureModel> GetRolesSubFeatures();
        List<TbrolesubfeatureModel> GetRolesSubFeaturesByFeatureId(int featureId);
        string CreateSubFeature(string SubFeatureName, int featureId);
        string DeleteSubFeature(int id);
        bool UpdateSubFeature(int id, string NewSubFeatureName);
        List<TbrolepermissionModel> GetPermissionsByGUIDRoleAndSubFeatureID(Guid GUIDRole, int RoleSubFeatureId);
        bool UpdatePermission(int RolePermissionId, bool Allow);
        string CreateNewPermission(Guid GUIDRole, int RoleSubFeatureId, bool Allow);
        List<TbrolepermissionModel> GetPremissionByRoleAndFeatureId(Guid GUIDRole, int RoleFeatureId);
        List<UserData> GetOtherUsers(Guid GUIDRole);
        List<CtbRolePagePermissionModel> GetRolePagePermissionsByRoleGUID(Guid GUIDRole);
        List<CtbMenuPageModel> GetMenuPagesByPageTypeAndPageIds(string PageTypes, List<int> PageIds);
        CtbMenuPageModel GetMenuPageById(int id);
        CtbMenuPageModel GetMenuPageByRoute(string Route);
        string DeleteRole(Guid GUIDRole);
        ctbControllerActionModel GetControllerActionByName(string ControllerName, string ActionName);
        List<TbrolesubfeatureModel> GetSubFeaturesWithAccessToAction(Guid GUIDControllerAction);
        List<TbrolepermissionModel> GetRolePermissionsByRoleGUIDListAndSubFeatureIdList(List<Guid> CurrentUserRolesGUIDs, List<int> SubFeaturesGUIDs);
        List<ctbControllerActionModel> GetControllerActionBySubfeature(int RoleSubFeatureId);
        bool UpdateControllerAction(ctbControllerActionModel model);
        string DeleteControllerAction(Guid GUIDControllerAction);
        string CreateControllerAction(ctbControllerActionModel model);
        List<ctbControllerActionModel> GetOtherControllerActionBySubfeature(int RoleSubFeatureId);
        string AddControllerActionToSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction);
        string RemoveControllerActionFromSubFeature(int RoleSubFeatureId, Guid GUIDControllerAction);
        List<TbrolepermissionModel> PermissionsBySubFeatureId(int RoleSubFeatureId);
        List<string> GetUniqueMenuTypes();
        List<CtbMenuPageModel> GetAllMenuPages(string PageTypes);
        bool UpdateMenuPage(CtbMenuPageModel model);
        List<CtbRolePagePermissionModel> PagePermissionsByPageId(int MenuPageId);
        List<CtbRolePagePermissionModel> GetMenuPermissionsByGUIDRoleAndMenuPageId(Guid GUIDRole, int MenuPageId);
        bool UpdateMenuPermission(int MenuPageId, bool Allow);
        string CreateNewMenuPermission(Guid GUIDRole, int MenuPageId, bool Allow);
        string CreateMenuPage(CtbMenuPageModel model);
        string DeleteMenuPage(int MenuPageId);
        List<CtbsubfeaturelimitsModel> GetLimitsBySubfeatureId(int SubFeatureID);
        TbrolesubfeatureModel GetSubfeatureId(int SubFeatureID);
        TbroleModel GetRoleByGUID(Guid RoleGUID);
        CtbsubfeaturelimitsModel GetLimitByID(int LimitID);
        CtblimitdefaultsModel GetDefaultsByID(int DefaultsId);
        List<dynamic> GetDefualtDataFromTables(CtblimitdefaultsModel DefaultsObject);
        CtbrolelimitvalueModel GetRoleLimitsValueByGUIDRoleANDLimitsID(Guid GUIDRole, int SubfeatureLimitsId);
        string CreateRoleLimitsValue(CtbrolelimitvalueModel model);
        bool UpdateRoleLimitsValue(int Id, string Value);
        List<CtblimitscontrolleractionsModel> GetLimitsControllerActions(int LimitsId);
        string PostLimitsControllerActions(CtblimitscontrolleractionsModel model);
        string DeleteLimitsControllerActions(int LimitsId, Guid GUIDControllerAction);
        List<CtblimitscontrolleractionsModel> GetLimitsControllerActionsFromGUIDControllerAction(Guid GUIDControllerAction);
        List<CtbsubfeaturelimitsModel> GetSubFeatureLimitsFromLimitIds(string LimitIds);
        CtbsubfeaturelimitsModel GetSubFeatureLimitsFromLimitName(string limitName);
    }
}

