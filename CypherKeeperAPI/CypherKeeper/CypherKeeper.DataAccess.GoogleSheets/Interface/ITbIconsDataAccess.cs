using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.GoogleSheets.Interface
{
    public interface ITbIconsDataAccess
    {
        List<tbIconsModel> Get(int page = 1, int itemsPerPage = 100, bool onlyNonDeleted = true);
        tbIconsModel GetById(Guid Id, bool onlyNonDeleted = true);
        tbIconsModel Add(tbIconsModel model);
        bool Update(Guid Id, tbIconsModel model);
        bool Delete(Guid Id);
        bool Restore(Guid Id);
        int Total(bool onlyNonDeleted = true);
    }
}
