﻿using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.SQL.Interface
{
    public interface IMixedDataAccess
    {
        dynamic SaveKey(SavedKeyModel model);
        List<tbKeysHistoryModel> GetKeyHistory(Guid KeyId);
        bool DublicateKey(Guid KeyId, Guid? NewGroupId = null);
        bool DublicateGroup(Guid GroupId);
    }
}
