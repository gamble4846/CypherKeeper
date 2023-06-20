using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.SQL.Interface
{
    public interface ITbKeysDataAccess
    {
        List<tbKeysModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        List<tbKeysModel> GetByGroupId(Guid GroupId);
        tbKeysModel GetById(Guid Id, bool onlyNonDeleted = true);
        tbKeysModel Add(tbKeysModel model);
        bool Update(Guid Id, tbKeysModel model);
        bool Delete(Guid Id);
        bool Restore(Guid Id);
        int Total(bool onlyNonDeleted = true);
    }
}
