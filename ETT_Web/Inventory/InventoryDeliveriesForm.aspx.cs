using DevExpress.Web;
using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models.XML;
using ETT_Utilities.Common;
using ETT_Utilities.Helpers;
using ETT_Web.Infrastructure;
using ETT_Web.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace ETT_Web.Inventory
{
    public partial class InventoryDeliveriesForm : ServerMasterPage
    {
        Session session;
        int userAction;
        int invDelID;
        InventoryDeliveries model;
        IInventoryRepository inventoryRepo;
        ILocationRepository locationRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                userAction = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                invDelID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            session = XpoHelper.GetNewSession();

            XpoDSSupplier.Session = session;
            XpoDSLocation.Session = session;
            XpoDSProduct.Session = session;


            XpoDSInventoryDeliveriesLocation.Session = session;
            XpoDSLocation.Session = session;

            inventoryRepo = new InventoryRepository(session);
            locationRepo = new LocationRepository(session);

            ASPxGridViewInventoryDeliveriesLocation.Settings.GridLines = GridLines.Both;            
            GridLookupProduct.GridView.Settings.GridLines = GridLines.Both;            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = inventoryRepo.GetInventoryDeliveriesByID(invDelID);

                    if (model != null)
                    {
                        GetInventoryDeliveriesProvider().SetInventoryDeliveriesModel(model);
                        FillForm();
                    }
                }

                FillDefaultValues();
                UserActionConfirmBtnUpdate(btnSaveChanges, userAction);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.InventoryDeliveriesSession.InventoryDeliveriesModel))
                    model = GetInventoryDeliveriesProvider().GetInventoryDeliveriesModel();
            }
        }

        private void FillDefaultValues()
        {
            if (userAction == (int)Enums.UserAction.Add)
            {
                DateEditDeliveryNoteDate.Date = DateTime.Now;
            }
        }

        private void FillForm()
        {
            GridLookupProduct.Value = model.InventoryStockID.ProductID.ProductID;
            txtSupplierProductCode.Text = model.SupplierProductCode;
            txtUIDAtomeCode.Text = model.AtomeUID250;
            txtUIDPackaging.Text = model.PackagesUIDs;
            MemoNotes.Text = model.Notes;

            txtDeliveryNoteNumber.Text = model.DeliveryNoteItemID.DeliveryNoteID.DeliveryNoteNumber;
            DateEditDeliveryNoteDate.Date = model.DeliveryNoteItemID.DeliveryNoteID.DeliveryNoteDate;
            DateEditDeliveryRecivedMaterialDate.Date = model.DeliveryNoteItemID.DeliveryNoteID.RecivedMaterialDate;
            GridLookupLocation.Value = model.DeliveryNoteItemID.DeliveryNoteID.LocationID.LocationID;

            MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            /*if (add)
            {
                model = new InventoryDeliveries(session);
                model.InventoryDeliveriesID = 0;
            }
            else if (!add && model == null)
            {
                model = GetInventoryDeliveriesProvider().GetInventoryDeliveriesModel();
            }

            model.DeliveryNoteDate = DateEditDeliveryNoteDate.Date;
            model.DeliveryNoteNumber = txtDeliveryNoteNumber.Text;

            int supplierID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupSupplier));
            if (model.SupplierID != null)
                model.SupplierID = clientRepo.GetClientByID(supplierID, model.SupplierID.Session);
            else
                model.SupplierID = clientRepo.GetClientByID(supplierID);

            int locationID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupLocation));
            if (model.LocationID != null)
                model.LocationID = locationRepo.GetLocationByID(locationID, model.LocationID.Session);
            else
                model.LocationID = locationRepo.GetLocationByID(locationID);
                
            model.Notes = MemoNotes.Text;

            invDelLocID = deliveryNoteRepo.SaveDeliveryNote(model, PrincipalHelper.GetUserID());*/

            return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ProcessUserAction(false);
        }

        private bool DeleteObject()
        {
            return inventoryRepo.DeleteInventoryDeliveries(invDelID);
        }

        #region Common

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool exitFormPage = true)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = invDelID.ToString() }
            };

            if (isIDDeleted)
                redirectString = "InventoryDeliveriesTable.aspx";
            else if (!exitFormPage)
            {
                queryStrings.Insert(0, new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                redirectString = GenerateURI("InventoryDeliveriesTable.aspx", queryStrings);
            }
            else
                redirectString = GenerateURI("InventoryDeliveriesTable.aspx", queryStrings);

            List<Enums.EmployeeSession> list = Enum.GetValues(typeof(Enums.EmployeeSession)).Cast<Enums.EmployeeSession>().ToList();
            ClearAllSessions(list, redirectString);
        }

        private void ProcessUserAction(bool exitFormPage = true)
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (userAction)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing, exitFormPage);
            }
        }
        #endregion

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewInventoryDeliveriesLocation.DataBind();
            }
        }

       

        private void GridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "IsBuyer") return;

            if (Convert.ToBoolean(e.Value))
                e.DisplayText = "DA";
            else
                e.DisplayText = "NE";
        }
      

        protected void ASPxGridViewInventoryDeliveriesLocation_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "NeedsMatching")
            {
                e.DisplayText = CommonMethods.ParseBool(e.Value) == true ? "NE" : "DA";
            }
        }
    }
}