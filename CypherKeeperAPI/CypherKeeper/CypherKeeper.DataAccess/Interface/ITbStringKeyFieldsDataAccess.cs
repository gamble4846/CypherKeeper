using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.SQL.Interface
{
    public interface ITbStringKeyFieldsDataAccess
    {
        List<tbStringKeyFieldsModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        tbStringKeyFieldsModel GetById(Guid Id, bool onlyNonDeleted = true);
        tbStringKeyFieldsModel Add(tbStringKeyFieldsModel model);
        bool Update(Guid Id, tbStringKeyFieldsModel model);
        bool Delete(Guid Id);
        bool Restore(Guid Id);
        int Total(bool onlyNonDeleted = true);
    }
}
