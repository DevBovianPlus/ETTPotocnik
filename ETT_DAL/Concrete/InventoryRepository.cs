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
    public class InventoryRepository : IInventoryRepository
    {
        Session session;

        public InventoryRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }

        public InventoryDeliveries GetInventoryDeliveriesByID(int lId, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryDeliveries> invDel = null;

                if (currentSession != null)
                    invDel = currentSession.Query<InventoryDeliveries>();
                else
                    invDel = session.Query<InventoryDeliveries>();

                return invDel.Where(i => i.InventoryDeliveriesID == lId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_34, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveInventoryDeliveries(InventoryDeliveries model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.InventoryDeliveriesID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.InventoryDeliveriesID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_35, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteInventoryDeliveries(int lId)
        {
            try
            {
                GetInventoryDeliveriesByID(lId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_36, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteInventoryDeliveries(InventoryDeliveries model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_36, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int CountInventoryDeliverisByPackageUID(string uid, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryDeliveries> invDel = null;

                if (currentSession != null)
                    invDel = currentSession.Query<InventoryDeliveries>();
                else
                    invDel = session.Query<InventoryDeliveries>();

                return invDel.Count(i => i.PackagesUIDs.Contains(uid));
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_34, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}