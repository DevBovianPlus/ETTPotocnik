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
                    mobile.tsInsert = DateTime.Now;
                    mobile.tsInsertUserID = model.user_id;
                }

                mobile.InventoryDeliveriesLocationID = SaveInventoryDeliveriesLocation(model, mobile.Session);

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
    }
}