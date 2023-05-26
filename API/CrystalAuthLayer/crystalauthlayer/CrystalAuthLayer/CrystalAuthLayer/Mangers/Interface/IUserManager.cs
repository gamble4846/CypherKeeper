using System;
using System.Collections.Generic;
using System.Text;
using AuthLayer.Models;

namespace AuthLayer.Mangers.Interface
{
    public interface IUserManager
    {
        TbaccessModel GetUserByIdAndPassword(string userId, string userPassword, string connectionString);
        TbaccessModel GetCurrentUser();
    }
}

