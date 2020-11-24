using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Utilities.Helpers;
using ETT_Web.Infrastructure;
using ETT_Web.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ETT_Web.Employees
{
    public partial class EmployeeForm : ServerMasterPage
    {
        Session session;
        int userAction;
        int employeeID;
        Employee model;
        IEmployeeRepository employeeRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                userAction = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                employeeID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            session = XpoHelper.GetNewSession();

            XpoDSUsers.Session = session;

            employeeRepo = new EmployeeRepository(session);

            ASPxGridViewUsers.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = employeeRepo.GetEmployeeByID(employeeID);

                    if (model != null)
                    {
                        GetEmployeeProvider().SetEmployeeModel(model);
                        FillForm();
                    }
                }
                else
                    userCredentialsItem.Attributes["class"] = "disabled";

                FillDefaultValues();
                UserActionConfirmBtnUpdate(btnSaveChanges, userAction);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.EmployeeSession.EmployeeModel))
                    model = GetEmployeeProvider().GetEmployeeModel();
            }
        }

        private void FillDefaultValues()
        {
            if (model != null)
            {
                //txtClientType.Text = model.ClientTypeID.Name;
            }
        }

        private void FillForm()
        {
            txtName.Text = model.Firstname;
            txtLastName.Text = model.Lastname;
            DateEditBirthDate.Date = model.BirthDate;
            txtAddress.Text = model.Address;
            txtPostcode.Text = model.Postcode;
            txtCity.Text = model.Post;

            txtEmail.Text = model.Email;
            txtPhone.Text = model.Phone;

            MemoNotes.Text = model.Notes;

            if(!String.IsNullOrEmpty(model.Picture))
                UploadProfile.ProfileImage.Src = model.Picture.Replace(AppDomain.CurrentDomain.BaseDirectory, "/");

            HtmlGenericControl control = (HtmlGenericControl)userCredentialsItem.FindControl("userCredentialsBadge");
            control.InnerText = model.UsersCollection.Count.ToString();
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Employee(session);
                model.EmployeeID = 0;
            }
            else if (!add && model == null)
            {
                model = GetEmployeeProvider().GetEmployeeModel();
            }

            model.Firstname = txtName.Text;
            model.Lastname = txtLastName.Text;
            model.BirthDate = DateEditBirthDate.Date;
            model.Address = txtAddress.Text;
            model.Postcode = txtPostcode.Text;
            model.Post = txtCity.Text;

            model.Email = txtEmail.Text;
            model.Phone = txtPhone.Text;

            model.Notes = MemoNotes.Text;

            if (!String.IsNullOrEmpty(model.Picture))
            {
                UploadProfile.ProfileImage.Src = model.Picture.Replace(AppDomain.CurrentDomain.BaseDirectory, "\\");
            }

            employeeID = employeeRepo.SaveEmployee(model, PrincipalHelper.GetUserID());

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
            return employeeRepo.DeleteEmployee(employeeID);
        }

        #region Common

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool exitFormPage = true)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = employeeID.ToString() }
            };

            if (isIDDeleted)
                redirectString = "EmployeeTable.aspx";
            else if (!exitFormPage)
            {
                queryStrings.Insert(0, new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                redirectString = GenerateURI("EmployeeForm.aspx", queryStrings);
            }
            else
                redirectString = GenerateURI("EmployeeTable.aspx", queryStrings);

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
                ASPxGridViewUsers.DataBind();
            }
            else
            {
                object user = ASPxGridViewUsers.GetRowValues(ASPxGridViewUsers.FocusedRowIndex, "UserID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.EmployeeSession.UserID, user);
                AddValueToSession(Enums.EmployeeSession.EmployeeID, employeeID);
                if (openPopup)
                    PopupControlUsers.ShowOnPageLoad = true;
            }
        }

        protected void UploadProfile_ImageUpdated(object sender, EventArgs e)
        {
            if (model == null && userAction == (int)Enums.UserAction.Add)
                model = new Employee(session);
            else if (model == null)
                model = GetEmployeeProvider().GetEmployeeModel();

            model.Picture = (sender as ImageUploadWidget).ImageFullFileName;
            GetEmployeeProvider().SetEmployeeModel(model);
        }

        protected void ASPxGridViewUsers_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewUsers, btnAdd, btnEdit, btnDelete);
        }

        protected void PopupControlUsers_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.EmployeeSession.EmployeeID);
            RemoveSession(Enums.EmployeeSession.UserID);
            RemoveSession(Enums.EmployeeSession.UserModel);
        }
    }
}