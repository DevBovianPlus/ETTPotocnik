using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IInventoryRepository
    {
        InventoryDeliveries GetInventoryDeliveriesByID(int lId, Session currentSession = null);
        int SaveInventoryDeliveries(InventoryDeliveries model, int userID = 0);
        bool DeleteInventoryDeliveries(int lId);
        bool DeleteInventoryDeliveries(InventoryDeliveries model);
    }
}
