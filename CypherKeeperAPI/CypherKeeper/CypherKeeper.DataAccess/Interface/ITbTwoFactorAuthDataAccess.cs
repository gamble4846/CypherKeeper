using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.SQL.Interface
{
    public interface ITbTwoFactorAuthDataAccess
    {
        List<tbTwoFactorAuthModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null, bool onlyNonDeleted = true);
        List<tbTwoFactorAuthModel> GetByKeyId(Guid KeyId, bool onlyNonDeleted = true);
        tbTwoFactorAuthModel GetById(Guid Id, bool onlyNonDeleted = true);
        tbTwoFactorAuthModel Add(tbTwoFactorAuthModel model);
        bool Update(Guid Id, tbTwoFactorAuthModel model);
        bool Delete(Guid Id);
        bool Restore(Guid Id);
        int Total(bool onlyNonDeleted = true);
    }
}
