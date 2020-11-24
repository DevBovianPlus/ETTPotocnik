using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class InventoryStockProvider : ServerMasterPage
    {
        public void SetInventoryStockModel(InventoryStock model)
        {
            AddValueToSession(Enums.InventoryStockSession.InventoryStockModel, model);
        }

        public InventoryStock GetInventoryStockModel()
        {
            if (SessionHasValue(Enums.InventoryStockSession.InventoryStockModel))
                return (InventoryStock)GetValueFromSession(Enums.InventoryStockSession.InventoryStockModel);

            return null;
        }
    }
}