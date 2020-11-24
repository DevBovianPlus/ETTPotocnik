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

namespace ETT_Web.IssueDocuments
{
    public partial class IssueDocument_popup : ServerMasterPage
    {
        Session session;
        IIssueDocumentRepository issueDocumentRepo;
        IClientRepository clientRepo;
        int issueDocumentPosID;
        int userAction;
        IssueDocumentPosition model;
        IInventoryRepository inventoryRepo;
        IProductRepository productRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);
            issueDocumentPosID = GetIntValueFromSession(Enums.IssueDocumentSession.IssueDocumentPositionID);

            session = XpoHelper.GetNewSession();

            XpoDSSupplier.Session = session;

            issueDocumentRepo = new IssueDocumentRepository(session);
            clientRepo = new ClientRepository(session);
            inventoryRepo = new InventoryRepository(session);
            productRepo = new ProductRepository(session);

            GridLookupSupplier.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = issueDocumentRepo.GetIssueDocumentPositionByID(issueDocumentPosID);

                    if (model != null)
                    {
                        GetIssueDocumentProvider().SetIssueDocumentPositionModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirm, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.IssueDocumentSession.IssueDocumentPositionModel))
                    model = GetIssueDocumentProvider().GetIssueDocumentPositionModel();
            }
        }

        private void FillForm()
        {
            txtUID250.Text = model.UID250;
            txtName.Text = model.Name;
            GridLookupSupplier.Value = model.SupplierID != null ? model.SupplierID.ClientID : -1;
            txtQuantity.Text = model.Quantity.ToString();
            MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new IssueDocumentPosition(session);
                model.IssueDocumentPositionID = 0;
            }
            else if (!add && model == null)
            {
                model = GetIssueDocumentProvider().GetIssueDocumentPositionModel();
            }

            model.IssueDocumentID = issueDocumentRepo.GetIssueDocumentByID(GetIntValueFromSession(Enums.IssueDocumentSession.IssueDocumentID), model.Session);

            int supplierID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupSupplier));
            if (model.SupplierID != null)
                model.SupplierID = clientRepo.GetClientByID(supplierID, model.SupplierID.Session);
            else
                model.SupplierID = clientRepo.GetClientByID(supplierID, model.Session);

            model.Quantity = CommonMethods.ParseDecimal(txtQuantity.Text);
            model.UID250 = !String.IsNullOrEmpty(txtUID250.Text) ? txtUID250.Text : model.UID250;
            model.Name = txtName.Text;
            model.Notes = MemoNotes.Text;

            issueDocumentRepo.SaveIssueDocumentPosition(model, PrincipalHelper.GetUserID());

            return true;
        }

        private bool DeleteObject()
        {
            return issueDocumentRepo.DeleteIssueDocumentPosition(issueDocumentPosID);
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

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.IssueDocumentSession.IssueDocumentID);
            RemoveSession(Enums.IssueDocumentSession.IssueDocumentPositionID);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "IssueDocumentPosition"), true);

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void CallbakcPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split(';');

            if (split[0] == "SearchByUID")
            {
                AddValueToSession(Enums.IssueDocumentSession.SearchUIDValue, txtUIDSearchString.Text);
                PopupControlSearchInventory.ShowOnPageLoad = true;
            }
            else if (split[0] == "FillIssueDocument")
            {
                var inventory = inventoryRepo.GetInventoryDeliveriesByID(CommonMethods.ParseInt(split[1]));

                if (inventory != null)
                {
                    if (userAction == (int)Enums.UserAction.Add)
                    {
                        model = new IssueDocumentPosition(session);
                        model.IssueDocumentPositionID = 0;
                        model.ProductID = productRepo.GetProductByID(inventory.InventoryStockID.ProductID.ProductID, inventory.InventoryStockID.ProductID.Session);

                        AddValueToSession(Enums.CommonSession.UserActionPopUp, (int)Enums.UserAction.Edit);
                    }
                    else
                    {
                        model = GetIssueDocumentProvider().GetIssueDocumentPositionModel();
                        model.ProductID = productRepo.GetProductByID(inventory.InventoryStockID.ProductID.ProductID, inventory.InventoryStockID.ProductID.Session);
                    }

                    model.UID250 = txtUID250.Text = inventory.AtomeUID250;
                    GridLookupSupplier.Value = inventory.DeliveryNoteItemID.DeliveryNoteID.SupplierID.ClientID;
                    txtName.Text = inventory.InventoryStockID.ProductID.Name;
                    txtUIDSearchString.Text = "";

                    GetIssueDocumentProvider().SetIssueDocumentPositionModel(model);
                }
            }
        }

        protected void PopupControlSearchInventory_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.IssueDocumentSession.SearchUIDValue);
        }
    }
}