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
    public interface ITbKeysManager
    {
        APIResponse Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        APIResponse Add(tbKeysModel model);
        APIResponse Update(Guid Id, tbKeysModel model);
        APIResponse Delete(Guid Id);
        APIResponse Restore(Guid Id);
    }
}
