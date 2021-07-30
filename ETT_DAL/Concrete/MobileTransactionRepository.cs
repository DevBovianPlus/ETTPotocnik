using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models.MobileTransaction;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class MobileTransactionModel
    {
        public int RowCnt { get; set; }
        public int MobileTransactionID { get; set; }
        public int InventoryDeliveriesLocationID { get; set; }
        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        public int tsInsertUserID { get; set; }
        public int UnitOfMeasureID { get; set; }
        public int tsUpdateUserID { get; set; }
        public string UIDCode { get; set; }
        public string ScannedProductCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantitySum { get; set; }
        public decimal QuantitySumKg { get; set; }
        public DateTime tsUpdate { get; set; }
        public DateTime tsInsert { get; set; }
        public string Notes { get; set; }
        public string Uporabnik { get; set; }
        public string IzLokacije { get; set; }
        public string NaLokacijo { get; set; }
        public string Dobavitelj { get; set; }
        public string Produkt { get; set; }
        public bool IsBuyer { get; set; }
        public bool NeedMatching { get; set; }
        public int LocationToClientID { get; set; }

        public decimal Faktor { get; set; }

        public string DateSum { get; set; }
        public DateTime DateSumDate { get; set; }
    }

    public class MobileTransactionRepository : IMobileTransactionRepository
    {
        Session session;
        ILocationRepository locRepo;
        IUserRepository userRepo;
        IClientRepository clientRepo;
        IProductRepository productRepo;

        public MobileTransactionRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;

            locRepo = new LocationRepository(session);
            userRepo = new UserRepository(session);
            clientRepo = new ClientRepository(session);
            productRepo = new ProductRepository(session);
        }

        public MobileTransaction GetMobileTransactionByID(int mtId, Session currentSession = null)
        {
            try
            {
                XPQuery<MobileTransaction> mTransaction = null;

                if (currentSession != null)
                    mTransaction = currentSession.Query<MobileTransaction>();
                else
                    mTransaction = session.Query<MobileTransaction>();

                return mTransaction.Where(mt => mt.MobileTransactionID == mtId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_31, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<MobileTransactionModel> GetMobileTransactionByDates(DateTime dtFrom, DateTime dtTo, Session currentSession = null)
        {
            try
            {
                XPQuery<MobileTransaction> mTransaction = null;

                if (currentSession != null)
                    mTransaction = currentSession.Query<MobileTransaction>();
                else
                    mTransaction = session.Query<MobileTransaction>();


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
                                IsBuyer = x.InventoryDeliveriesLocationID.LocationToID.IsBuyer,
                                NeedMatching = x.InventoryDeliveriesLocationID.NeedsMatching,
                                LocationToClientID = x.InventoryDeliveriesLocationID.LocationToID.BuyerID.ClientID

                            };

                var lMobileTrans = query.ToList();

                int iCnt = 1;
                var lMobileTransOrder = lMobileTrans.Where(mt => mt.tsInsert >= dtFrom && mt.tsInsert <= dtTo).OrderByDescending(mt => mt.tsInsert).ToList();

                foreach (var itm in lMobileTransOrder)
                {
                    itm.RowCnt = iCnt++;
                }


                return lMobileTransOrder;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_31, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<MobileTransactionModel> GetDaySummaryMobileTransaction(DateTime dtFrom, DateTime dtTo, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryDeliveries> invDeliveries = currentSession.Query<InventoryDeliveries>();

                List<MobileTransactionModel> lstReturn = new List<MobileTransactionModel>();

                XPQuery<MobileTransaction> mTransaction = null;

                if (currentSession != null)
                    mTransaction = currentSession.Query<MobileTransaction>();
                else
                    mTransaction = session.Query<MobileTransaction>();


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

                var lMobileTrans = query.ToList();

                int iCnt = 1;
                DateTime dFromFilter = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, 0, 0, 0);
                DateTime dToFilter = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, 23, 59, 59);
                var lMobileTransOrder = lMobileTrans.Where(mt => mt.tsInsert >= dFromFilter && mt.tsInsert <= dToFilter).OrderByDescending(mt => mt.tsInsert).OrderByDescending(mt => mt.Produkt).OrderByDescending(mt => mt.IzLokacije).OrderByDescending(mt => mt.NaLokacijo).ToList();
                string sDateShort = "", sProduct = "", sIzLokacije = "", sNaLokacijo = "";
                decimal dCurrentQnt = 0;
                decimal dSummaryQnt = 0;
                MobileTransactionModel nTmodel = null;


                foreach (var itm in lMobileTransOrder)
                {
                    if (sDateShort != itm.tsInsert.ToShortDateString() || sProduct != itm.Produkt || sIzLokacije != itm.IzLokacije || sNaLokacijo != itm.NaLokacijo)
                    {
                        nTmodel = new MobileTransactionModel();
                        dSummaryQnt = 0;
                        nTmodel.DateSum = itm.tsInsert.ToShortDateString();
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




                    itm.RowCnt = iCnt++;


                }
                iCnt = 1;
                foreach (var itm in lstReturn)
                {
                    itm.RowCnt = iCnt++;
                    Product pr = productRepo.GetProductByID(itm.ProductID);
                    decimal dFact = (pr == null ? 1 : pr.Factor);
                    itm.QuantitySumKg = dFact > 0 ? itm.QuantitySum * dFact : 0;
                }


                return lstReturn;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_31, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<DayTransaction> GetDaySummaryTransaction(DateTime dtFrom, DateTime dtTo, Session currentSession = null)
        {
            try
            {
                List<DayTransaction> lDayTransOrder = new List<DayTransaction>();
                XPQuery<DayTransaction> dayTrans = currentSession.Query<DayTransaction>();

                if (dayTrans.Count() > 0)
                {

                    DateTime dFromFilter = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, 0, 0, 0);
                    DateTime dToFilter = new DateTime(dtTo.Year, dtTo.Month, dtTo.Day, 23, 59, 59);
                    lDayTransOrder = dayTrans.Where(mt => mt.CurrentDay >= dFromFilter && mt.CurrentDay <= dToFilter).ToList();



                    lDayTransOrder = lDayTransOrder.OrderByDescending(mt => mt.CurrentDay).ThenBy(mt => mt.NaLokacijo).ToList();

                    int iCnt = 1;
                    foreach (var itm in lDayTransOrder)
                    {
                        itm.RowCnt = iCnt++;
                        Product pr = productRepo.GetProductByID(itm.ProductID);
                        decimal dFact = (pr == null ? 1 : pr.Factor);
                        itm.QuantityKG = dFact > 0 ? itm.Quantity * dFact : 0;
                        itm.QuantityKG = Math.Round(itm.QuantityKG, 0);
                        itm.Save();
                    }

                }

                return lDayTransOrder;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_31, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveMobileTransaction(MobileTransaction model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.MobileTransactionID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.MobileTransactionID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_32, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteMobileTransaction(int mtId)
        {
            try
            {
                GetMobileTransactionByID(mtId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_33, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteMobileTransaction(MobileTransaction model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_33, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public int SaveMobileTransaction(MobileTransactionAPIModel model)
        {
            try
            {
                XPQuery<MobileTransaction> mTransaction = null;

                string sCode = GetUIDCode(model.code);
                mTransaction = session.Query<MobileTransaction>();

                int iCnt = mTransaction.Where(t => t.UIDCode == sCode && t.tsInsert == model.created_at && t.tsInsertUserID == model.user_id).Count();
                if (iCnt != 0) return 0;

                XPQuery<InventoryDeliveries> invDeliveries = session.Query<InventoryDeliveries>();

                MobileTransaction mobile = new MobileTransaction(session);
                mobile.InventoryDeliveriesLocationID = null;
                mobile.MobileTransactionID = 0;
                mobile.Notes = "";
                mobile.ScannedProductCode = model.code;
                mobile.UIDCode = GetUIDCode(model.code);
                mobile.SupplierID = clientRepo.GetClientByID(model.supplier_id, mobile.Session);
                mobile.ProductID = productRepo.GetProductByID(model.inventory_id, mobile.Session);

                mobile.tsUpdate = DateTime.Now;
                mobile.tsUpdateUserID = model.user_id;

                if (mobile.MobileTransactionID == 0)
                {
                    mobile.tsInsert = model.created_at;
                    mobile.tsInsertUserID = model.user_id;
                }

                mobile.InventoryDeliveriesLocationID = SaveInventoryDeliveriesLocation(model, mobile.Session);
                mobile.Quantity = invDeliveries.Count(inv => inv.PackagesUIDs.Contains(mobile.UIDCode));//kosovna količina
                mobile.Save();

                return mobile.MobileTransactionID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_32, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private InventoryDeliveriesLocation SaveInventoryDeliveriesLocation(MobileTransactionAPIModel model, Session sess)
        {
            InventoryDeliveriesLocation loc = new InventoryDeliveriesLocation(sess);
            loc.InventoryDeliveriesID = null;
            loc.InventoryDeliveriesLocationID = 0;
            loc.IsMobileTransaction = true;
            loc.LocationFromID = locRepo.GetLocationByID(model.location_from_id, loc.Session);
            loc.LocationToID = locRepo.GetLocationByID(model.location_to_id, loc.Session);
            loc.Notes = "";
            loc.UserID = userRepo.GetUserByID(model.user_id, loc.Session);
            loc.tsUpdate = DateTime.Now;
            loc.tsUpdateUserID = model.user_id;
            loc.NeedsMatching = true;

            if (loc.InventoryDeliveriesLocationID == 0)
            {
                loc.tsInsert = DateTime.Now;
                loc.tsInsertUserID = model.user_id;
            }

            loc.Save();

            return loc;
        }

        private string GetUIDCode(string code)
        {
            string uidStartCode = ((int)Enums.PackageCodeStructure.UIDStevilkaIzdelka).ToString();
            string[] split = code.Split('\u001d');

            var uid = split.Where(i => i.StartsWith(uidStartCode)).FirstOrDefault();
            if (uid != null)
            {
                return uid.Substring(uidStartCode.Length);
            }
            return "";
        }

        public bool DeleteDuplicateMobileTransaction(Session currentSession = null)
        {
            try
            {
                int iCnt = 0;

                XPQuery<MobileTransaction> mTransaction = null;

                if (currentSession != null)
                    mTransaction = currentSession.Query<MobileTransaction>();
                else
                    mTransaction = session.Query<MobileTransaction>();


                //find duplicates rows
                var duplicateValues = (from row in mTransaction
                                       let UIDCode = row.UIDCode
                                       let tsInsert = row.tsInsert
                                       let tsInsertUserID = row.tsInsertUserID
                                       group row by new { UIDCode, tsInsert, tsInsertUserID } into grp
                                       where grp.Count() > 1
                                       select new
                                       {
                                           DupID = grp.Key.UIDCode,
                                           DupInsertTS = grp.Key.tsInsert,
                                           DupInsertUserID = grp.Key.tsInsertUserID,
                                           cnt = grp.Count()
                                       }).ToList();
                CommonMethods.LogThis("Število podvojenih zapisov: " + duplicateValues.Count.ToString());
                foreach (var item in duplicateValues)
                {
                    iCnt++;

                    var ByUIDCodeValues = mTransaction.Where(d => d.UIDCode == item.DupID).OrderBy(s => s.MobileTransactionID).ToList();
                    var firstID = ByUIDCodeValues.FirstOrDefault();

                    if (firstID != null)
                    {
                        var removeTrans = ByUIDCodeValues.Where(f => f.MobileTransactionID != firstID.MobileTransactionID).ToList();

                        foreach (var remT in removeTrans)
                        {
                            remT.Delete();
                        }

                    }

                    if (iCnt % 1000 == 0)
                    {
                        CommonMethods.LogThis("Zapis: " + iCnt + "/ " + duplicateValues.Count());
                    }

                }

                return true;
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