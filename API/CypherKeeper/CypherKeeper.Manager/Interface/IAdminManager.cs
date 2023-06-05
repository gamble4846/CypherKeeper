using CypherKeeper.AuthLayer.Models;
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
    public interface IAdminManager
    {
        APIResponse Register(RegisterModel model);
        APIResponse Login(string Username, string Password);
    }
}
