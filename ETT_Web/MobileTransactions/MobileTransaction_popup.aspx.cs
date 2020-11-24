using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETT_Web.MobileTransactions
{
    public partial class MobileTransaction_popup : ServerMasterPage
    {
        Session session;
        int mobileTransactionID;
        int userAction;
        MobileTransaction model;
        IProductRepository productRepo;
        ILocationRepository locationRepo;
        IMobileTransactionRepository mobileTransRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (SessionHasValue(Enums.CommonSession.UserActionPopUpInPopUp))
                userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUpInPopUp);
            else
                userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);

            mobileTransactionID = GetIntValueFromSession(Enums.MobileTransactionSession.MobileTransactionID);

            session = XpoHelper.GetNewSession();
            productRepo = new ProductRepository(session);
            locationRepo = new LocationRepository(session);
            mobileTransRepo = new MobileTransactionRepository(session);

            //XpoDSLocation.Session = session;
            //XpoDSProduct.Session = session;
            //XpoDSSupplier.Session = session;
            //XpoDSUser.Session = session;

            GridLookupProduct.GridView.Settings.GridLines = GridLines.Both;
            GridLookupLocationFrom.GridView.Settings.GridLines = GridLines.Both;
            GridLookupLocationTo.GridView.Settings.GridLines = GridLines.Both;
            GridLookupUser.GridView.Settings.GridLines = GridLines.Both;
            GridLookupSupplier.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = mobileTransRepo.GetMobileTransactionByID(mobileTransactionID);

                    if (model != null)
                    {
                        GetMobileTransactionProvider().SetMobileTransactionModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.MobileTransactionSession.MobileTransactionModel))
                    model = GetMobileTransactionProvider().GetMobileTransactionModel();
            }
        }

        private void FillForm()
        {
            //GridLookupUser.Value = model.InventoryDeliveriesLocationID != null ? model.InventoryDeliveriesLocationID.UserID.UserID : 0;
            //txtScannedCode.Text = model.ScannedProductCode;
            //GridLookupProduct.Value = model.InventoryDeliveriesLocationID != null ? (model.InventoryDeliveriesLocationID.InventoryDeliveriesID != null) ? model.InventoryDeliveriesLocationID.InventoryDeliveriesID.InventoryStockID.ProductID.ProductID : 0 : 0;
            //GridLookupSupplier.Value = model.InventoryDeliveriesLocationID != null ? (model.InventoryDeliveriesLocationID.InventoryDeliveriesID != null) ? model.InventoryDeliveriesLocationID.InventoryDeliveriesID.DeliveryNoteItemID.DeliveryNoteID.SupplierID.ClientID : 0 : 0;
            //GridLookupLocationFrom.Value = model.InventoryDeliveriesLocationID.LocationFromID != null ? model.InventoryDeliveriesLocationID.LocationFromID.LocationID : 0;
            //GridLookupLocationFrom.Value = model.InventoryDeliveriesLocationID.LocationToID != null ? model.InventoryDeliveriesLocationID.LocationToID.LocationID : 0;
            //MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new MobileTransaction(session);
                model.MobileTransactionID = 0;
            }
            else if( !add && model == null)
            {
                model = GetMobileTransactionProvider().GetMobileTransactionModel();
            }

            /*int productID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupProduct));
            if (model.ProductID != null)
                model.ProductID = productRepo.GetProductByID(productID, model.ProductID.Session);
            else
                model.ProductID = productRepo.GetProductByID(productID);*/

            
            model.Notes = MemoNotes.Text;

            mobileTransRepo.SaveMobileTransaction(model, PrincipalHelper.GetUserID());

            return true;
        }

        private void ProcessUserAction()
        {
            bool isValid = false;
            bool confirm = false;

            switch (userAction)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    confirm = true;
                    break;
            }

            if (isValid)
            {
                RemoveSessionsAndClosePopUP(confirm);
            }
            else
                ShowWarningPopup("Opozorilo", "Prišlo je do napake. Kontaktirajete administratorja!");
        }

        private bool DeleteObject()
        {
            return mobileTransRepo.DeleteMobileTransaction(mobileTransactionID);
        }

        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void btnConfirmPopup_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            
            RemoveSession(Enums.CommonSession.UserActionPopUp);

            RemoveSession(Enums.MobileTransactionSession.MobileTransactionID);
            RemoveSession(Enums.MobileTransactionSession.MobileTransactionModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "MobileTransaction"), true);

        }
    }
}