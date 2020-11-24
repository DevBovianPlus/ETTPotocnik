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
    public partial class Client_popup : ServerMasterPage
    {
        Session session;
        int clientID;
        int userAction;
        Client model;
        IClientRepository clientRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUpInPopUp);
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
                    model = clientRepo.GetClientByID(clientID);

                    if (model != null)
                    {
                        GetClientProvider().SetSupplierModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.SupplierSession.ClientModel))
                    model = GetClientProvider().GetSupplierModel();
            }
        }

        private void FillForm()
        {
            txtName.Text = model.Name;
            txtCountry.Text = model.Country;
            txtPhone.Text = model.Phone;
            txtEmail.Text = model.Email;
            MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Client(session);
                model.ClientID = 0;
                //Zaenkrat ko s tme popup-om shranjujemo samo dobavitelje
                model.ClientTypeID = clientRepo.GetClientTypeByCode(Enums.ClientType.DOBAVITELJ.ToString(), model.Session);
            }
            else if( !add && model == null)
            {
                model = GetClientProvider().GetSupplierModel();
            }

            model.Name = txtName.Text;
            model.Country = txtCountry.Text;
            model.Phone = txtPhone.Text;
            model.Email = txtEmail.Text;
            model.Notes = MemoNotes.Text;

            clientRepo.SaveClient(model, PrincipalHelper.GetUserID());

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
            return clientRepo.DeleteClient(clientID);
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

            RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
            RemoveSession(Enums.SupplierSession.ClientID);
            RemoveSession(Enums.SupplierSession.ClientModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Client"), true);

        }
    }
}