using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class LocationProvider : ServerMasterPage
    {
        public void SetLocationModel(Location model)
        {
            AddValueToSession(Enums.LocationSession.LocationModel, model);
        }

        public Location GetLocationModel()
        {
            if (SessionHasValue(Enums.LocationSession.LocationModel))
                return (Location)GetValueFromSession(Enums.LocationSession.LocationModel);

            return null;
        }
    }
}