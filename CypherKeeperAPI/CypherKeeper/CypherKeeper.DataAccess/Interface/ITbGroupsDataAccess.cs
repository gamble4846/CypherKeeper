using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.SQL.Interface
{
    public interface ITbGroupsDataAccess
    {
        List<tbGroupsModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        tbGroupsModel GetById(Guid Id, bool onlyNonDeleted = true);
        tbGroupsModel Add(tbGroupsModel model);
        bool Update(Guid Id, tbGroupsModel model);
        bool Delete(Guid Id);
        bool Restore(Guid Id);
        int Total(bool onlyNonDeleted = true);
        int GetTotalByParentGroupId(Guid? ParentGroupId, bool onlyNonDeleted = true);
    }
}
