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

namespace ETT_Web.Inventory
{
    public partial class InventoryStock_popup : ServerMasterPage
    {
        Session session;
        int inventoryStockID;
        int userAction;
        InventoryStock model;
        IProductRepository productRepo;
        ILocationRepository locationRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (SessionHasValue(Enums.CommonSession.UserActionPopUpInPopUp))
                userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUpInPopUp);
            else
                userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);

            inventoryStockID = GetIntValueFromSession(Enums.InventoryStockSession.InventoryStockID);

            session = XpoHelper.GetNewSession();
            productRepo = new ProductRepository(session);
            locationRepo = new LocationRepository(session);

            XpoDSLocation.Session = session;
            XpoDSProduct.Session = session;

            GridLookupProduct.GridView.Settings.GridLines = GridLines.Both;
            GridLookupLocation.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = productRepo.GetInventoryStockByID(inventoryStockID);

                    if (model != null)
                    {
                        GetInventoryStockProvider().SetInventoryStockModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.InventoryStockSession.InventoryStockModel))
                    model = GetInventoryStockProvider().GetInventoryStockModel();
            }
        }

        private void FillForm()
        {

            GridLookupProduct.Value = model.ProductID != null ? model.ProductID.ProductID : 0;
            GridLookupLocation.Value = model.LocationID != null ? model.LocationID.LocationID : 0;
            txtQuantity.Text = model.Quantity.ToString("N2");
            MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new InventoryStock(session);
                model.InventoryStockID = 0;
            }
            else if( !add && model == null)
            {
                model = GetInventoryStockProvider().GetInventoryStockModel();
            }

            int productID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupProduct));
            if (model.ProductID != null)
                model.ProductID = productRepo.GetProductByID(productID, model.ProductID.Session);
            else
                model.ProductID = productRepo.GetProductByID(productID);

            int locationID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupLocation));
            if (model.LocationID != null)
                model.LocationID = locationRepo.GetLocationByID(locationID, model.LocationID.Session);
            else
                model.LocationID = locationRepo.GetLocationByID(locationID);


            model.Quantity = CommonMethods.ParseDecimal(txtQuantity.Text);
            model.Notes = MemoNotes.Text;

            productRepo.SaveInventoryStock(model, PrincipalHelper.GetUserID());

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
            return productRepo.DeleteInventoryStock(inventoryStockID);
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

            if (SessionHasValue(Enums.CommonSession.UserActionPopUpInPopUp))
                RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
            else
                RemoveSession(Enums.CommonSession.UserActionPopUp);

            RemoveSession(Enums.InventoryStockSession.InventoryStockID);
            RemoveSession(Enums.InventoryStockSession.InventoryStockModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "InventoryStock"), true);

        }
    }
}