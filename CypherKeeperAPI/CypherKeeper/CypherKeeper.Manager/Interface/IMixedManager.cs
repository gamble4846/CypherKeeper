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
    public interface IMixedManager
    {
        APIResponse SaveKey(SavedKeyModel model);
        APIResponse GetKeyHistory(Guid KeyId);
        APIResponse DublicateKey(Guid KeyId);
        APIResponse DublicateGroup(Guid GroupId);
        APIResponse GetTwoFACodeData(Guid TwoFAId);
    }
}
