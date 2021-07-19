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
    public class IssueDocumentRepository : IIssueDocumentRepository
    {
        Session session;
        ISettingsRepository settingsRepo;
        IClientRepository clientRepo;
        IProductRepository productRepo;

        public IssueDocumentRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;

            settingsRepo = new SettingsRepository(session);
            clientRepo = new ClientRepository(session);
            productRepo = new ProductRepository(session);
        }

        public IssueDocument GetIssueDocumentByID(int ID, Session currentSession = null)
        {
            try
            {
                XPQuery<IssueDocument> issueDocument = null;

                if (currentSession != null)
                    issueDocument = currentSession.Query<IssueDocument>();
                else
                    issueDocument = session.Query<IssueDocument>();

                return issueDocument.Where(isd => isd.IssueDocumentID == ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_37, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveIssueDocument(IssueDocument model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.IssueDocumentID == 0)
                {
                    model.IssueNumber = GetNextIssueDocumentNumber();
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.IssueDocumentID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_38, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteIssueDocument(int ID)
        {
            try
            {
                GetIssueDocumentByID(ID).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_39, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteIssueDocument(IssueDocument model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_39, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public IssueDocumentPosition GetIssueDocumentPositionByID(int ID, Session currentSession = null)
        {
            try
            {
                XPQuery<IssueDocumentPosition> issueDocumentPos = null;

                if (currentSession != null)
                    issueDocumentPos = currentSession.Query<IssueDocumentPosition>();
                else
                    issueDocumentPos = session.Query<IssueDocumentPosition>();

                return issueDocumentPos.Where(isdp => isdp.IssueDocumentPositionID == ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_37, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveIssueDocumentPosition(IssueDocumentPosition model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.IssueDocumentPositionID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.IssueDocumentPositionID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_38, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveIssueDocumentPositions(List<IssueDocumentPosition> model, int issueDocumentID, int userID = 0)
        {

        }

        public bool DeleteIssueDocumentPosition(int ID)
        {
            try
            {
                GetIssueDocumentPositionByID(ID).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_39, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteIssueDocumentPosition(IssueDocumentPosition model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_39, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private string GetNextIssueDocumentNumber()
        {
            var set = settingsRepo.GetSettings();

            if (set != null)
            {
                set.IssueDocumentNumber += 1;
                set.Save();
                return DateTime.Now.Year.ToString() + "/" + set.IssueDocumentNumber;
            }

            return "";
        }

        public IssueDocumentStatus GetIssueDocumentStatusByID(int ID, Session currentSession = null)
        {
            try
            {
                XPQuery<IssueDocumentStatus> issueDocumentStatus = null;

                if (currentSession != null)
                    issueDocumentStatus = currentSession.Query<IssueDocumentStatus>();
                else
                    issueDocumentStatus = session.Query<IssueDocumentStatus>();

                return issueDocumentStatus.Where(isds => isds.IssueDocumentStatusID == ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_37, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public IssueDocumentStatus GetIssueDocumentStatusByCode(object statusCode, Session currentSession = null)
        {
            try
            {
                XPQuery<IssueDocumentStatus> issueDocumentStatus = null;
                string code = statusCode.ToString();

                if (currentSession != null)
                    issueDocumentStatus = currentSession.Query<IssueDocumentStatus>();
                else
                    issueDocumentStatus = session.Query<IssueDocumentStatus>();

                return issueDocumentStatus.Where(isds => isds.Code == code).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_37, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int CreateIssueDocumentFromMobileTransactions(List<int> mobileTransactionsID, int userID, int buyerID)
        {
            UnitOfWork uow = XpoHelper.GetNewUnitOfWork();
            XPQuery<MobileTransaction> mobileTrans = uow.Query<MobileTransaction>();
            XPQuery<InventoryDeliveries> invDeliveries = uow.Query<InventoryDeliveries>();

            var mobileTransactions = mobileTrans.Where(mt => mobileTransactionsID.Any(mti => mti == mt.MobileTransactionID)).ToList();
            if (mobileTransactions != null && mobileTransactions.Count > 0)
            {
                var buyer = clientRepo.GetClientByID(buyerID, session);
                //from locationID we get buyer
                IssueDocument document = new IssueDocument(session);

                document.IssueDocumentID = 0;
                document.IssueNumber = GetNextIssueDocumentNumber();
                document.BuyerID = buyer;
                document.IssueStatus = GetIssueDocumentStatusByCode(Enums.IssueDocumentStatus.DELOVNA);
                document.IssueDate = DateTime.Now;
                document.Name = $"Izdaja materiala za kupca: {buyer.Name}";
                document.Notes = "Prenos iz mobilnih transkacij";
                document.InternalDocument = "";                
                document.tsInsert = DateTime.Now;
                document.tsInsertUserID = userID;
                document.tsUpdate = DateTime.Now;
                document.tsUpdateUserID = userID;

                document.Save();

                var issueDocument = GetIssueDocumentByID(document.IssueDocumentID, uow);
                IssueDocumentPosition pos = null;
                foreach (var item in mobileTransactions)
                {
                    pos = new IssueDocumentPosition(uow);

                    pos.IssueDocumentID = issueDocument;
                    pos.SupplierID = clientRepo.GetClientByID(item.SupplierID.ClientID, uow);
                    
                    //Kosovna količina
                    pos.Quantity = item.Quantity <= 0 ? invDeliveries.Count(inv => inv.PackagesUIDs.Contains(item.UIDCode)) : item.Quantity;// če se še ni shranila količina na mobilnih transkacijah jo poiščemo v invnetoryDeliveries

                    pos.UID250 = item.UIDCode;
                    pos.Name = item.ProductID.Name;
                    pos.Notes = "Prenos iz mobilnih transakcij";
                    pos.tsInsert = DateTime.Now;
                    pos.tsInsertUserID = userID;
                    pos.tsUpdate = DateTime.Now;
                    pos.tsUpdateUserID = userID;
                    pos.ProductID = productRepo.GetProductByID(item.ProductID.ProductID, uow);
                    decimal dFact = (pos.ProductID.Factor == 0 ? 0 : pos.ProductID.Factor);
                    pos.QuantityKG = dFact > 0 ? pos.Quantity * dFact : 0;

                    pos.MobileTransactionID = item;
                }

                uow.CommitChanges();


                return document.IssueDocumentID;
            }

            return -1;
        }

        public List<IssueDocumentPosition> GetIssueDocumentPositionsByDocumentID(int ID, Session currentSession = null)
        {
            try
            {
                XPQuery<IssueDocumentPosition> issueDocumentPos = null;

                if (currentSession != null)
                    issueDocumentPos = currentSession.Query<IssueDocumentPosition>();
                else
                    issueDocumentPos = session.Query<IssueDocumentPosition>();

                return issueDocumentPos.Where(isdp => isdp.IssueDocumentID.IssueDocumentID == ID).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_37, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}