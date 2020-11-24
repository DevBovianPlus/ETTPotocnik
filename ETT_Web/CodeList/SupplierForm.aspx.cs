using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Utilities.Helpers;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ETT_Web.CodeList
{
    public partial class SupplierForm : ServerMasterPage
    {
        Session session;
        int userAction;
        int clientID;
        Client model;
        IClientRepository clientRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                userAction = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                clientID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            session = XpoHelper.GetNewSession();

            XpoDSContactPerson.Session = session;

            clientRepo = new ClientRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = clientRepo.GetClientByID(clientID);

                    if (model != null)
                    {
                        GetClientProvider().SetSupplierModel(model);
                        FillForm();
                    }
                }
                else
                    contactPersonItem.Attributes["class"] = "disabled";

                FillDefaultValues();
                UserActionConfirmBtnUpdate(btnSaveChanges, userAction);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.SupplierSession.ClientModel))
                    model = GetClientProvider().GetSupplierModel();
            }
        }

        private void FillDefaultValues()
        {
            if (model != null)
            {
                txtClientType.Text = model.ClientTypeID.Name;
            }
        }

        private void FillForm()
        {
            txtName.Text = model.Name;
            txtLongName.Text = model.LongName;
            txtAddress.Text = model.Address;
            txtPostalcode.Text = model.Postcode;
            txtCity.Text = model.PostName;
            txtCountry.Text = model.Country;
            txtEmail.Text = model.Email;
            txtPhone.Text = model.Phone;
            txtFax.Text = model.FAX;
            txtBankAccount.Text = model.BankAccount;
            txtTaxNumber.Text = model.TaxNumber;
            txtRegistrationNumber.Text = model.RegistrationNumber;
            txtIdentificationNumber.Text = model.IdentificationNumber;
            chbxTaxPayer.Checked = model.Taxpayer;
            chbxEUMember.Checked = model.EUMember;
            MemoNotes.Text = model.Notes;

            HtmlGenericControl control = (HtmlGenericControl)contactPersonItem.FindControl("contactPersonBadge");
            control.InnerText = model.ContactPersons.Count.ToString();
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Client(session);
                model.ClientID = 0;
                model.ClientTypeID = clientRepo.GetClientTypeByCode(Enums.ClientType.DOBAVITELJ.ToString(), model.Session);
            }
            else if (!add && model == null)
            {
                model = GetClientProvider().GetSupplierModel();
            }

            model.Name = txtName.Text;
            model.LongName = txtLongName.Text;
            model.Address = txtAddress.Text;
            model.Postcode = txtPostalcode.Text;
            model.PostName = txtCity.Text;
            model.Country = txtCountry.Text;
            model.Email = txtEmail.Text;
            model.Phone = txtPhone.Text;
            model.FAX = txtFax.Text;
            model.BankAccount = txtBankAccount.Text;
            model.TaxNumber = txtTaxNumber.Text;
            model.RegistrationNumber = txtRegistrationNumber.Text;
            model.IdentificationNumber = txtIdentificationNumber.Text;
            model.Taxpayer = chbxTaxPayer.Checked;
            model.EUMember = chbxEUMember.Checked;
            model.Notes = MemoNotes.Text;

            clientID = clientRepo.SaveClient(model, PrincipalHelper.GetUserID());

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
            return clientRepo.DeleteClient(clientID);
        }

        #region Common

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool exitFormPage = true)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = clientID.ToString() }
            };

            
            if (isIDDeleted)
                redirectString = "SupplierTable.aspx";
            else if (!exitFormPage)
            {
                queryStrings.Insert(0, new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                redirectString = GenerateURI("SupplierForm.aspx", queryStrings);
            }
            else
                redirectString = GenerateURI("SupplierTable.aspx", queryStrings);

            List<Enums.SupplierSession> list = Enum.GetValues(typeof(Enums.SupplierSession)).Cast<Enums.SupplierSession>().ToList();
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

        protected void ASPxGridViewContactPerson_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewContactPerson, btnAdd, btnEdit, btnDelete);
        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewContactPerson.DataBind();
            }
            else
            {
                object contactPerson = ASPxGridViewContactPerson.GetRowValues(ASPxGridViewContactPerson.FocusedRowIndex, "ContactPersonID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.SupplierSession.ContactPersonID, contactPerson);
                AddValueToSession(Enums.SupplierSession.ClientID, clientID);
                if (openPopup)
                    PopupControlContactPerson.ShowOnPageLoad = true;
            }
        }

        protected void PopupControlContactPerson_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.SupplierSession.ContactPersonID);
            RemoveSession(Enums.SupplierSession.ContactPersonModel);
        }
    }
}