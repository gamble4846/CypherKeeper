using CypherKeeper.Model;
using EasyCrudLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.Interface
{
    public interface ITbGroupsDataAccess
    {
        public List<tbGroupsModel> Get(int page = 1, int itemsPerPage = 100, List<OrderByModel> orderBy = null);
    }
}
