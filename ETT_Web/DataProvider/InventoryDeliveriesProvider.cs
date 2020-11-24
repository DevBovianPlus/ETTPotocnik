using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class InventoryDeliveriesProvider : ServerMasterPage
    {
        public void SetInventoryDeliveriesModel(InventoryDeliveries model)
        {
            AddValueToSession(Enums.InventoryDeliveriesSession.InventoryDeliveriesModel, model);
        }

        public InventoryDeliveries GetInventoryDeliveriesModel()
        {
            if (SessionHasValue(Enums.InventoryDeliveriesSession.InventoryDeliveriesModel))
                return (InventoryDeliveries)GetValueFromSession(Enums.InventoryDeliveriesSession.InventoryDeliveriesModel);

            return null;
        }
    }
}