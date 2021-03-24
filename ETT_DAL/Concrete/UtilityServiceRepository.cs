using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class UtilityServiceRepository : IUtilityServiceRepository
    {
        Session session;

        IMeasuringUnitRepository measureRepo;
        IProductRepository productRepo;
        ILocationRepository locationRepo;
        ICategorieRepository categoryRepo;
        IClientRepository clientRepo;
        IUserRepository userRepo;

        public UtilityServiceRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;

            measureRepo = new MeasuringUnitRepository(session);
            productRepo = new ProductRepository(session);
            locationRepo = new LocationRepository(session);
            categoryRepo = new CategorieRepository(session);
            clientRepo = new ClientRepository(session);
            userRepo = new UserRepository(session);
        }

        private List<InventoryDeliveriesLocation> GetInventoryDeliveryLocationsThatNeedsMatching(Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryDeliveriesLocation> idl = null;

                if (currentSession != null)
                    idl = currentSession.Query<InventoryDeliveriesLocation>();
                else
                    idl = session.Query<InventoryDeliveriesLocation>();

                return idl.Where(i => i.NeedsMatching && !i.LocationToID.IsBuyer).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_41, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        /// <summary>
        /// Razknjiževanje mobilnih transakcij
        /// </summary>
        /// <param name="issueDocumentTransactions">Seznam InventoryDeliveriesLocation zapisov, ki smo jih potegnali iz MobileTransaction objektov. 
        /// Ti objekti so bili prenešeni na izdajnico in je potrebno razknjižiti njihovo količino.
        /// </param>
        public void MatchMobileTransWithInventoryDeliveries(List<InventoryDeliveriesLocation> issueDocumentTransactions = null)
        {
            try
            {
                UnitOfWork uow = XpoHelper.GetNewUnitOfWork();
                XPQuery<InventoryDeliveries> id = uow.Query<InventoryDeliveries>();
                int iCnt = 0;
                var transactionForMatching = issueDocumentTransactions ?? GetInventoryDeliveryLocationsThatNeedsMatching(uow);
                foreach (var item in transactionForMatching)
                {
                    iCnt++;
                    var uid = item.MobileTransactions.FirstOrDefault().UIDCode;
                    var deliveries = id.Where(inv => inv.PackagesUIDs.Contains(uid)).ToList();

                    if (deliveries != null && deliveries.Count > 0)
                    {
                        MatchDeliveriesAndTransactions(item, deliveries, uow);
                        item.NeedsMatching = false;
                    }
                    uow.CommitChanges();
                }

                uow.CommitChanges();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_41, error, CommonMethods.GetCurrentMethodName()));
            }
        }
        // zakaj si tole kloniral?
        private InventoryDeliveriesLocation CreateNewInstance(InventoryDeliveriesLocation itemToClone, InventoryDeliveries delivery, UnitOfWork unitOfWork)
        {
            CloneHelper clone = new CloneHelper(unitOfWork);
            var clonedInventoryDeliveriesLocation = new InventoryDeliveriesLocation(unitOfWork);//clone.Clone(itemToClone);

            clonedInventoryDeliveriesLocation.ParentID = itemToClone.InventoryDeliveriesLocationID;
            clonedInventoryDeliveriesLocation.InventoryDeliveriesLocationID = 0;
            clonedInventoryDeliveriesLocation.InventoryDeliveriesID = delivery;
            clonedInventoryDeliveriesLocation.NeedsMatching = false;
            clonedInventoryDeliveriesLocation.LocationFromID = itemToClone.LocationFromID;
            clonedInventoryDeliveriesLocation.LocationToID = itemToClone.LocationToID;
            clonedInventoryDeliveriesLocation.Notes = itemToClone.Notes;
            clonedInventoryDeliveriesLocation.UserID = itemToClone.UserID;
            clonedInventoryDeliveriesLocation.tsInsert = DateTime.Now;
            clonedInventoryDeliveriesLocation.tsUpdate = DateTime.Now;
            clonedInventoryDeliveriesLocation.IsMobileTransaction = itemToClone.IsMobileTransaction;
            //clonedInventoryDeliveriesLocation.MobileTransactions = new XPCollection<MobileTransaction>(unitOfWork, itemToClone.MobileTransactions);

            delivery.LastLocationID = clonedInventoryDeliveriesLocation.LocationToID;
            //delivery.Save();

            //clonedInventoryDeliveriesLocation.Save();

            CalculateInventoryStock(itemToClone, delivery, unitOfWork);

            return clonedInventoryDeliveriesLocation;
        }

        private void MatchDeliveriesAndTransactions(InventoryDeliveriesLocation invLoc, List<InventoryDeliveries> deliveries, UnitOfWork unitOfWork)
        {
            if (deliveries.Count > 1)
            {
                foreach (var item in deliveries)
                    CreateNewInstance(invLoc, item, unitOfWork);
            }
            else
            {
                var delivery = deliveries.FirstOrDefault();
                invLoc.InventoryDeliveriesID = delivery;
                invLoc.NeedsMatching = false;
                invLoc.tsUpdate = DateTime.Now;

                delivery.LastLocationID = invLoc.LocationToID;

                CalculateInventoryStock(invLoc, delivery, unitOfWork);

                //invLoc.Save();
            }
        }

        private void CreateNewRecordForStock(bool bAddItems, Location cLocation, InventoryDeliveriesLocation item, InventoryDeliveries delivery, UnitOfWork unitOfWork)
        {
            // naredimo zapis za to skladišče in ta produkt
            // gremo v minus zalogo, da bomo pozneje odkrivali napake
            InventoryStock InvStock = new InventoryStock(unitOfWork);
            InvStock.ProductID = delivery.DeliveryNoteItemID.ProductID;
            InvStock.LocationID = cLocation;
            InvStock.Quantity = (bAddItems) ? InvStock.Quantity + delivery.Quantity : InvStock.Quantity - delivery.Quantity;
            InvStock.QuantityPcs = (bAddItems) ? InvStock.QuantityPcs + 1 : InvStock.QuantityPcs - 1;
            InvStock.Notes = "";

            if (InvStock.InventoryStockID == 0)
            {
                InvStock.tsInsert = DateTime.Now;
                InvStock.tsInsertUserID = item.UserID.UserID;
            }

            InvStock.Save();

            unitOfWork.CommitChanges();
        }

        private void CalculateInventoryStock(InventoryDeliveriesLocation item, InventoryDeliveries delivery, UnitOfWork unitOfWork)
        {
            bool locationFromWarehouse = false, locationToWarehouse = false;
            XPQuery<InventoryStock> inventoryStock = unitOfWork.Query<InventoryStock>();

            locationFromWarehouse = locationRepo.IsLocationWarehouse(item.LocationFromID.LocationID);
            locationToWarehouse = locationRepo.IsLocationWarehouse(item.LocationToID.LocationID);


            //16.11.2020 Boris            
            InventoryStock recordInventoryFromWarehouseStock = inventoryStock.Where(invS => invS.ProductID.ProductID == delivery.DeliveryNoteItemID.ProductID.ProductID && invS.LocationID == item.LocationFromID).FirstOrDefault();
            InventoryStock recordInventoryToWarehouseStock = inventoryStock.Where(invS => invS.ProductID.ProductID == delivery.DeliveryNoteItemID.ProductID.ProductID && invS.LocationID == item.LocationToID).FirstOrDefault();

            if (locationFromWarehouse)
            {
                if (recordInventoryFromWarehouseStock != null)
                {
                    //odštejemo zalogo, ker smo prestavili iz skladišča na drugo lokacijo
                    recordInventoryFromWarehouseStock.Quantity -= delivery.Quantity;   // zakaj sta 2 qnt !!!!!!
                    recordInventoryFromWarehouseStock.QuantityPcs -= 1;
                }
                else
                {
                    CreateNewRecordForStock(false, item.LocationFromID, item, delivery, unitOfWork);
                }
            }

            if (locationToWarehouse)
            {
                if (recordInventoryToWarehouseStock != null)
                {
                    //odštejemo zalogo, ker smo prestavili iz skladišča na drugo lokacijo
                    recordInventoryToWarehouseStock.Quantity += delivery.Quantity;   // zakaj sta 2 qnt !!!!!!
                    recordInventoryToWarehouseStock.QuantityPcs += 1;
                }
                else
                {
                    CreateNewRecordForStock(true, item.LocationToID, item, delivery, unitOfWork);
                }
            }
            // 16.11.2020 Boris

            //if (recordInventoryStock != null)
            //{
            //    if (locationFromWarehouse && !locationToWarehouse)
            //    { //odštejemo zalogo, ker smo prestavili iz skladišča na drugo lokacijo
            //        recordInventoryStock.Quantity -= delivery.Quantity;
            //        recordInventoryStock.QuantityPcs -= 1;
            //    }
            //    else if (!locationFromWarehouse && locationToWarehouse)
            //    {//seštejemo zalogo, ker smo prenseli nazaj v skladišče
            //        recordInventoryStock.Quantity += delivery.Quantity;
            //        recordInventoryStock.QuantityPcs += 1;
            //    }

            //    //TODO: Potrebno urediti še ko so bodo prevzemi delali iz skladišča na skladišče

            //    recordInventoryStock.Save();
            //}
        }

        public void ClearStockByIssueDocumentID(List<IssueDocumentPosition> pos)
        {
            if (pos != null)
            {
                //Poiščemo vse InventoryDeliverisLocation ki imajo NeedsMatching na true in jih zapišemo v seznam
                var result = pos.Where(p => p.MobileTransactionID.InventoryDeliveriesLocationID.NeedsMatching).Select(idl => idl.MobileTransactionID.InventoryDeliveriesLocationID).ToList();

                if (result != null && result.Count > 0)
                    MatchMobileTransWithInventoryDeliveries(result);
            }
        }
    }
}