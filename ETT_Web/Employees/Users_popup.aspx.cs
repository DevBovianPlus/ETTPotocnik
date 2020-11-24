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

namespace ETT_Web.Employees
{
    public partial class Users_popup : ServerMasterPage
    {
        Session session;
        int userID;
        int userAction;
        Users model;
        IUserRepository userRepo;
        IEmployeeRepository employeeRepo;
        int employeeID;
        int userCount = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            
            userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);
            userID = GetIntValueFromSession(Enums.EmployeeSession.UserID);

            if (SessionHasValue(Enums.EmployeeSession.EmployeeID))
                employeeID = GetIntValueFromSession(Enums.EmployeeSession.EmployeeID);
            else
                employeeID = -1;

            session = XpoHelper.GetNewSession();
            userRepo = new UserRepository(session);
            employeeRepo = new EmployeeRepository(session);

            XpoDSEmployee.Session = session;
            XpoDSRole.Session = session;

            GridLookupEmployee.GridView.Settings.GridLines = GridLines.Both;
            GridLookupRole.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //če dodajamo userje iz forme za zaposlene
                if (employeeID > 0)
                {
                    GridLookupEmployee.Value = employeeID;
                    GridLookupEmployee.ClientEnabled = false;
                }

                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = userRepo.GetUserByID(userID);

                    if (model != null)
                    {
                        GetEmployeeProvider().SetUserModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.EmployeeSession.UserModel))
                    model = GetEmployeeProvider().GetUserModel();
            }
        }

        private void FillForm()
        {
            GridLookupEmployee.Value = model.EmployeeID != null ? model.EmployeeID.EmployeeID : 0;
            txtUsername.Text = model.Username;
            txtPassword.Text = model.Password;
            GridLookupRole.Value = model.RoleID != null ? model.RoleID.RoleID : 0;
            chbxGrantAccess.Checked = model.GrantAppAccess;            
            MemoNotes.Text = model.Notes;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPass", String.Format("clientTxtPassword.SetText('{0}');", model.Password), true);
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Users(session);
                model.UserID = 0;
            }
            else if (!add && model == null)
            {
                model = GetEmployeeProvider().GetUserModel();
            }

            int employeeID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupEmployee));
            if (model.EmployeeID != null)
                model.EmployeeID = employeeRepo.GetEmployeeByID(employeeID, model.EmployeeID.Session);
            else
                model.EmployeeID = employeeRepo.GetEmployeeByID(employeeID);

            model.Username = txtUsername.Text;
            model.Password = txtPassword.Text;

            int roleID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupRole));
            if (model.RoleID != null)
                model.RoleID = userRepo.GetRoleByID(roleID, model.RoleID.Session);
            else
                model.RoleID = userRepo.GetRoleByID(roleID);

            model.GrantAppAccess = chbxGrantAccess.Checked;

            model.Notes = MemoNotes.Text;

            userRepo.SaveUser(model, PrincipalHelper.GetUserID());

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
                userCount = userRepo.GetUserCountByEmployeeID(model.EmployeeID.EmployeeID);
                RemoveSessionsAndClosePopUP(confirm);
            }
            else
                ShowWarningPopup("Opozorilo", "Prišlo je do napake. Kontaktirajete administratorja!");
        }

        private bool DeleteObject()
        {
            return userRepo.DeleteUser(userID);
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
            RemoveSession(Enums.EmployeeSession.EmployeeID);
            RemoveSession(Enums.EmployeeSession.UserID);
            RemoveSession(Enums.EmployeeSession.UserModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}');", confirmCancelAction, "User", userCount), true);

        }
    }
}