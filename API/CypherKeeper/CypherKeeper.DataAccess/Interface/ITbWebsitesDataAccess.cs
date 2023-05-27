using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.SQL.Interface
{
    public interface ITbWebsitesDataAccess
    {
        List<tbWebsitesModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        tbWebsitesModel GetById(Guid Id, bool onlyNonDeleted = true);
        tbWebsitesModel Add(tbWebsitesModel model);
        bool Update(Guid Id, tbWebsitesModel model);
        bool Delete(Guid Id);
        bool Restore(Guid Id);
    }
}
