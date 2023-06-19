using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Interface
{
    public interface ITbGroupsManager
    {
        APIResponse Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        APIResponse Add(tbGroupsAddModel AddModel);
        APIResponse Update(Guid Id, tbGroupsModel model);
        APIResponse Delete(Guid Id);
        APIResponse Restore(Guid Id);
        APIResponse Rename(Guid Id, string NewName);
        APIResponse ChangeIcon(Guid Id, Guid? IconId);
    }
}
