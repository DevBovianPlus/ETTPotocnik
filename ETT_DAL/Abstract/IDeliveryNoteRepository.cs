using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Models;
using ETT_DAL.Models.XML;
using ETT_Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Abstract
{
    public interface IDeliveryNoteRepository
    {
        DeliveryNote GetDeliveryNoteByID(int dnId, Session currentSession = null);
        int SaveDeliveryNote(DeliveryNote model, int userID = 0);
        bool DeleteDeliveryNote(int dnId);
        bool DeleteDeliveryNote(DeliveryNote model);

        DeliveryNoteItem GetDeliveryNoteItemByID(int dniId, Session currentSession = null);
        int SaveDeliveryNoteItem(DeliveryNoteItem model, int userID = 0);
        bool DeleteDeliveryNoteItem(int dniId);
        bool DeleteDeliveryNoteItem(DeliveryNoteItem model);

        void SaveSummaryToDeliveryNoteItem(List<SummaryItemModel> model, int deliveryNoteID, int locationID, int supplierID, List<Item> atomes, int userID = 0);
        List<DeliveryNoteItem> GetDeliveryNoteItemsByDeliveryNoteID(int dnId, Session currentSession = null);

        //InventoryDeliveries
        void SaveInventoryDeliveries(List<Item> model, int deliveryNoteID, int locationID, int userID = 0);
        List<PackageItem> GroupByPackagesUIDs(int deliveryNoteItemID, Session currentSession = null);

        DeliveryNoteStatus GetDeliveryNoteStatusByCode(Enums.DeliveryNoteStatus status, Session currentSession = null);
    }
}