using CypherKeeper.AuthLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.DataAccess.MongoDB.Interface
{
    public interface IAdminDataAccess
    {
        tbAccessModel Register(tbAccessModel model);
        tbAccessModel GetByEmail(string email);
        tbAccessModel GetByUsername(string username);
        long UpdateUserSettings(SettingsModel model, tbAccessModel CurrentUser);
        long UpdateUserImages(List<ImagesModel> model, tbAccessModel CurrentUser);
    }
}
