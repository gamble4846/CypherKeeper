using System.Collections.Generic;
using System;
using AuthLayer.Models;

namespace AuthLayer.DataAccess.Interface
{
    public interface IUserDataAccess
    {
        TbaccessModel GetUserById(string UserId, string connectionString);
        TbaccessModel GetUserByAccessGUID(Guid GUIDAccess);
        List<TbaccessModel> GetUsersByAccessGUIDList(List<Guid> GUIDAccess);
    }
}

