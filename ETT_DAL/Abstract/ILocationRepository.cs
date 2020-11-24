using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Models.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface ILocationRepository
    {
        Location GetLocationByID(int lId, Session currentSession = null);
        int SaveLocation(Location model, int userID = 0);
        bool DeleteLocation(int lId);
        bool DeleteLocation(Location model);
        Location GetLocationDefault(Session currentSession = null);
        List<LocationAPIModel> GetLocationsMobile();
        List<Location> GetWarehouses(Session currentSession = null);
        bool IsLocationWarehouse(int locationID, Session currentSession = null);
    }
}
