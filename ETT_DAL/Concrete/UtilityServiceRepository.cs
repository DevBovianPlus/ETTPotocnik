﻿using DevExpress.Xpo;
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

        private List<MobileTransactionModel> allMobileTransactions = null;

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

                return idl.Where(i => i.NeedsMatching && !i.LocationToID.IsBuyer).OrderByDescending(o => o.tsInsert).ToList();
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
        public void MatchMobileTransWithInventoryDeliveries(List<InventoryDeliveriesLocation> issueDocumentTransactions = null, UnitOfWork uow = null)
        {
            int iCnt = 0;

            try
            {
                CommonMethods.LogThis("MatchMobileTransWithInventoryDeliveries START ");

                if (uow == null)
                    uow = XpoHelper.GetNewUnitOfWork();

                XPQuery<InventoryDeliveries> id = uow.Query<InventoryDeliveries>();

                var transactionForMatching = issueDocumentTransactions ?? GetInventoryDeliveryLocationsThatNeedsMatching(uow);
                foreach (var item in transactionForMatching)
                {
                    if (item.MobileTransactions != null && item.MobileTransactions.Count > 0)
                    {
                        iCnt++;
                        var uid = item.MobileTransactions.FirstOrDefault().UIDCode;
                        var deliveries = id.Where(inv => inv.PackagesUIDs.Contains(uid)).ToList();

                        if (deliveries != null && deliveries.Count > 0)
                        {
                            MatchDeliveriesAndTransactions(item, deliveries, uow);
                            item.NeedsMatching = false;
                            item.tsUpdate = DateTime.Now;
                        }

                        if (iCnt % 1000 == 0)
                        {
                            CommonMethods.LogThis("MatchMobileTransWithInventoryDeliveries Zapis: " + iCnt + "/ " + transactionForMatching.Count());
                        }
                    }
                    uow.CommitChanges();
                }



                uow.CommitChanges();

                CommonMethods.LogThis("MatchMobileTransWithInventoryDeliveries END ");
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                error = iCnt + " - " + error;
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
            clonedInventoryDeliveriesLocation.tsInsert = itemToClone.tsInsert;
            clonedInventoryDeliveriesLocation.tsUpdate = itemToClone.tsUpdate;
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
                    recordInventoryFromWarehouseStock.Quantity -= delivery.Quantity;   // količina v kg
                    recordInventoryFromWarehouseStock.QuantityPcs -= 1; // količina v kos
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
                    recordInventoryToWarehouseStock.Quantity += delivery.Quantity;
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

        public void ClearStockByIssueDocumentID(List<IssueDocumentPosition> pos, UnitOfWork uow)
        {
            if (pos != null)
            {
                //Poiščemo vse InventoryDeliverisLocation ki imajo NeedsMatching na true in jih zapišemo v seznam
                var result = pos.Where(p => p.MobileTransactionID.InventoryDeliveriesLocationID.NeedsMatching).Select(idl => idl.MobileTransactionID.InventoryDeliveriesLocationID).ToList();

                if (result != null && result.Count > 0)
                    MatchMobileTransWithInventoryDeliveries(result, uow);
            }
        }

        private List<MobileTransactionModel> ReturnDaySumTranasactions(DateTime dtDatum, Session currentSession)
        {
            List<MobileTransactionModel> lstReturn = new List<MobileTransactionModel>();

            XPQuery<MobileTransaction> mTransaction = null;
            XPQuery<InventoryDeliveries> invDeliveries = currentSession.Query<InventoryDeliveries>();

            if (currentSession != null)
                mTransaction = currentSession.Query<MobileTransaction>();
            else
                mTransaction = session.Query<MobileTransaction>();

            if (allMobileTransactions == null) allMobileTransactions = new List<MobileTransactionModel>();

            if (allMobileTransactions.Count == 0)
            {
                var query = from x in mTransaction
                            select new MobileTransactionModel
                            {
                                RowCnt = x.RowCnt,
                                MobileTransactionID = x.MobileTransactionID,
                                InventoryDeliveriesLocationID = x.InventoryDeliveriesLocationID.InventoryDeliveriesLocationID,
                                Uporabnik = x.InventoryDeliveriesLocationID.UserID.EmployeeID.Firstname + ' ' + x.InventoryDeliveriesLocationID.UserID.EmployeeID.Lastname,
                                ScannedProductCode = x.ScannedProductCode,
                                Notes = x.Notes,
                                tsInsert = x.tsInsert,
                                tsInsertUserID = x.tsInsertUserID,
                                tsUpdate = x.tsUpdate,
                                tsUpdateUserID = x.tsUpdateUserID,
                                UIDCode = x.UIDCode,
                                SupplierID = x.SupplierID.ClientID,
                                ProductID = x.ProductID.ProductID,
                                Quantity = x.Quantity,
                                IzLokacije = x.InventoryDeliveriesLocationID.LocationFromID.Name,
                                NaLokacijo = x.InventoryDeliveriesLocationID.LocationToID.Name,
                                Dobavitelj = x.SupplierID.Name,
                                Produkt = x.ProductID.Name,
                                Faktor = x.ProductID.Factor
                            };

                allMobileTransactions = query.ToList();
            }

            DateTime dFromFilter = new DateTime(dtDatum.Year, dtDatum.Month, dtDatum.Day, 0, 0, 0);
            DateTime dToFilter = new DateTime(dtDatum.Year, dtDatum.Month, dtDatum.Day, 23, 59, 59);
            var lMobileTransOrder = allMobileTransactions.Where(mt => mt.tsInsert >= dFromFilter && mt.tsInsert <= dToFilter).OrderByDescending(mt => mt.tsInsert).OrderByDescending(mt => mt.Produkt).OrderByDescending(mt => mt.IzLokacije).OrderByDescending(mt => mt.NaLokacijo).ToList();
            string sDateShort = "", sProduct = "", sIzLokacije = "", sNaLokacijo = "";
            decimal dCurrentQnt = 0;
            decimal dSummaryQnt = 0;
            MobileTransactionModel nTmodel = null;
            int iCnt = 0;

            foreach (var itm in lMobileTransOrder)
            {
                if (itm.ProductID == 0) continue;

                iCnt++;
                if (sDateShort != itm.tsInsert.ToShortDateString() || sProduct != itm.Produkt || sIzLokacije != itm.IzLokacije || sNaLokacijo != itm.NaLokacijo)
                {
                    nTmodel = new MobileTransactionModel();
                    dSummaryQnt = 0;
                    nTmodel.DateSum = itm.tsInsert.ToShortDateString();
                    nTmodel.DateSumDate = itm.tsInsert;
                    nTmodel.ProductID = itm.ProductID;
                    nTmodel.Produkt = itm.Produkt;
                    dCurrentQnt = itm.Quantity <= 0 ? invDeliveries.Count(inv => inv.PackagesUIDs.Contains(itm.UIDCode)) : itm.Quantity;// če se še ni shranila količina na mobilnih transkacijah jo poiščemo v invnetoryDeliveries
                    dSummaryQnt += dCurrentQnt;

                    nTmodel.NaLokacijo = itm.NaLokacijo;
                    nTmodel.IzLokacije = itm.IzLokacije;

                    lstReturn.Add(nTmodel);
                }
                else
                {
                    dCurrentQnt = itm.Quantity <= 0 ? invDeliveries.Count(inv => inv.PackagesUIDs.Contains(itm.UIDCode)) : itm.Quantity;// če se še ni shranila količina na mobilnih transkacijah jo poiščemo v invnetoryDeliveries
                    dSummaryQnt += dCurrentQnt;

                }

                nTmodel.QuantitySum = dSummaryQnt;




                sDateShort = itm.tsInsert.ToShortDateString();

                sProduct = itm.Produkt;
                sIzLokacije = itm.IzLokacije;
                sNaLokacijo = itm.NaLokacijo;



                if (iCnt % 100 == 0)
                {
                    CommonMethods.LogThis("Zapis: " + iCnt + "/ " + lMobileTransOrder.Count());
                }



            }

            return lstReturn;
        }

        private void SaveDayTransaction(List<MobileTransactionModel> lstReturnAll, Session currentSession = null)
        {

            foreach (var itm in lstReturnAll)
            {
                Product pr = productRepo.GetProductByID(itm.ProductID);
                decimal dFact = (pr == null ? 1 : pr.Factor);
                itm.QuantitySumKg = dFact > 0 ? itm.QuantitySum * dFact : 0;

                DayTransaction dayTrans = new DayTransaction(currentSession);
                dayTrans.CurrentDay = itm.DateSumDate;
                dayTrans.CurrentDayStr = itm.DateSum;
                dayTrans.IzLokacije = itm.IzLokacije;
                dayTrans.NaLokacijo = itm.NaLokacijo;
                dayTrans.ProductID = itm.ProductID;
                dayTrans.Product = itm.Produkt;
                dayTrans.Quantity = itm.QuantitySum;
                dayTrans.QuantityKG = itm.QuantitySumKg;

                if (dayTrans.DayTransactionID == 0)
                {
                    dayTrans.tsInsert = DateTime.Now;
                    dayTrans.tsInsertUserID = 1;
                }

                dayTrans.Save();
            }
        }

        public List<MobileTransactionModel> SetDaySummaryTransaction(DateTime dtFrom, DateTime dtTo, Session currentSession = null)
        {
            try
            {
                List<MobileTransactionModel> lstReturnAll = new List<MobileTransactionModel>();

                XPQuery<DayTransaction> dayTransactions = currentSession.Query<DayTransaction>();


                for (var retday = dtFrom.Date; retday <= dtTo; retday = retday.AddDays(1))
                {
                    if (dayTransactions.Where(dt => dt.CurrentDay.Date == retday.Date).Count() == 0)
                    {
                        List<MobileTransactionModel> lstReturn = ReturnDaySumTranasactions(retday, currentSession);
                        SaveDayTransaction(lstReturn, currentSession);
                    }
                }





                return lstReturnAll;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_31, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}