﻿using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Manager.Interface
{
    public interface ITbIconsManager
    {
        APIResponse Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        APIResponse Add(tbIconsModel model);
        APIResponse Update(Guid Id, tbIconsModel model);
        APIResponse Delete(Guid Id);
        APIResponse Restore(Guid Id);
    }
}
