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

namespace ETT_Web.CodeList
{
    public partial class ContactPerson_popup : ServerMasterPage
    {
        Session session;
        int contactPersonID;
        int clientID;
        int userAction;
        ContactPerson model;
        IClientRepository clientRepo;

        int contactPersonCount = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);
            contactPersonID = GetIntValueFromSession(Enums.SupplierSession.ContactPersonID);
            clientID = GetIntValueFromSession(Enums.SupplierSession.ClientID);

            session = XpoHelper.GetNewSession();
            clientRepo = new ClientRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = clientRepo.GetContactPersonByID(contactPersonID);

                    if (model != null)
                    {
                        GetClientProvider().SetContactPersonModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.SupplierSession.ContactPersonModel))
                    model = GetClientProvider().GetContactPersonModel();
            }
        }

        private void FillForm()
        {
            txtName.Text = model.Name;
            txtPhone.Text = model.Phone;
            txtMobilePhone.Text = model.MobilePhone;
            txtEmail.Text = model.Email;
            txtFax.Text = model.FAX;
            MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new ContactPerson(session);
                model.ContactPersonID = 0;
                model.ClientID = clientRepo.GetClientByID(clientID);
            }
            else if( !add && model == null)
            {
                model = GetClientProvider().GetContactPersonModel();
            }

            model.Name = txtName.Text;
            model.Phone = txtPhone.Text;
            model.MobilePhone = txtMobilePhone.Text;
            model.Email = txtEmail.Text;
            model.FAX = txtFax.Text;
            model.Notes = MemoNotes.Text;

            clientRepo.SaveContactPerson(model, PrincipalHelper.GetUserID());

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
                contactPersonCount = clientRepo.GetContactPersonCountByClientID(clientID);
                RemoveSessionsAndClosePopUP(confirm);
            }
            else
                ShowWarningPopup("Opozorilo", "Prišlo je do napake. Kontaktirajete administratorja!");
        }

        private bool DeleteObject()
        {
            return clientRepo.DeleteContactPerson(contactPersonID);
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
            RemoveSession(Enums.SupplierSession.ContactPersonID);
            RemoveSession(Enums.SupplierSession.ContactPersonModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}');", confirmCancelAction, "ContactPerson", contactPersonCount), true);

        }
    }
}