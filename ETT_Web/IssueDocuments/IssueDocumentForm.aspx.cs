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

namespace ETT_Web.IssueDocuments
{
    public partial class IssueDocumentForm : ServerMasterPage
    {
        UnitOfWork session;
        int userAction;
        int issueDocumentID;
        IssueDocument model;
        IIssueDocumentRepository issueDocumentRepo;
        IClientRepository clientRepo;
        IUtilityServiceRepository utilityRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = Title;
            Master.DisableNavBar = true;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                userAction = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                issueDocumentID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            session = XpoHelper.GetNewUnitOfWork();

            XpoDSBuyer.Session = session;
            XpoDSLocation.Session = session;
            XpoDSIssueDocumentPosition.Session = session;

            issueDocumentRepo = new IssueDocumentRepository(session);
            clientRepo = new ClientRepository(session);
            utilityRepo = new UtilityServiceRepository(session);

            ASPxGridViewIssueDocumentPosition.Settings.GridLines = GridLines.Both;

            GridLookupBuyer.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = issueDocumentRepo.GetIssueDocumentByID(issueDocumentID, session);

                    if (model != null)
                    {
                        GetIssueDocumentProvider().SetIssueDocumentModel(model);
                        FillForm();
                    }
                }

                FillDefaultValues();
                UserActionConfirmBtnUpdate(btnSaveChanges, userAction);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.IssueDocumentSession.IssueDocumentModel))
                    model = GetIssueDocumentProvider().GetIssueDocumentModel();
            }
        }

        private void FillDefaultValues()
        {
            if (userAction == (int)Enums.UserAction.Add)
            {
                DateEditIssueDocumentDate.Date = DateTime.Now;
            }

            if (SessionHasValue(Enums.CommonSession.OpenPopupAfterRefresh))
            {
                AddValueToSession(Enums.IssueDocumentSession.IssueDocumentID, issueDocumentID);
                PopupControlIssueDocumentPos.ShowOnPageLoad = true;
                RemoveSession(Enums.CommonSession.OpenPopupAfterRefresh);
            }
        }

        private void EnableButtons(bool enable = false)
        {
            btnSave.ClientEnabled = enable;
            btnSaveChanges.ClientEnabled = enable;

            btnCompleteIssueDocument.ClientVisible = enable;

            btnAdd.ClientEnabled = enable;
            btnEdit.ClientEnabled = enable;
            btnDelete.ClientEnabled = enable;
        }

        private void FillForm()
        {
            txtIssueDocumentNumber.Text = model.IssueNumber;
            txtName.Text = model.Name;
            DateEditIssueDocumentDate.Date = model.IssueDate;
            GridLookupBuyer.Value = model.BuyerID != null ? model.BuyerID.ClientID : 0;
            txtInternalDocument.Text = model.InternalDocument;
            txtInvoiceNumber.Text = model.InvoiceNumber;
            MemoNotes.Text = model.Notes;
            txtIssueStatus.Text = model.IssueStatus.Name;
            txtPermissonDoc.Text = model.PermissionDoc;

            if (model.IssueStatus.Code == Enums.IssueDocumentStatus.ZAKLJUCENO.ToString())
                EnableButtons();
            else
                EnableButtons(true);

            /*HtmlGenericControl control = (HtmlGenericControl)deliveryNoteItem.FindControl("deliveryNoteProductBadge");
            control.InnerText = model.DeliveryNoteItems.Count.ToString();*/
        }

        private bool AddOrEditEntityObject(bool add = false, bool completeIssueDocument = false)
        {
            if (add)
            {
                model = new IssueDocument(session);
                model.IssueDocumentID = 0;
                model.IssueStatus = issueDocumentRepo.GetIssueDocumentStatusByCode(Enums.IssueDocumentStatus.DELOVNA, model.Session);
            }
            else if (!add && model == null)
            {
                model = GetIssueDocumentProvider().GetIssueDocumentModel();
            }

            model.IssueNumber = txtIssueDocumentNumber.Text;
            model.Name = txtName.Text;
            model.IssueDate = DateEditIssueDocumentDate.Date;

            int buyerID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupBuyer));
            if (model.BuyerID != null)
                model.BuyerID = clientRepo.GetClientByID(buyerID, model.BuyerID.Session);
            else
                model.BuyerID = clientRepo.GetClientByID(buyerID);

            model.InternalDocument = txtInternalDocument.Text;
            model.InvoiceNumber = txtInvoiceNumber.Text;
            model.Notes = MemoNotes.Text;
            model.PermissionDoc = txtPermissonDoc.Text;


            if (completeIssueDocument)
            {
                model.IssueStatus = issueDocumentRepo.GetIssueDocumentStatusByCode(Enums.IssueDocumentStatus.ZAKLJUCENO, model.Session);
                utilityRepo.ClearStockByIssueDocumentID(model.IssueDocumentPositions.ToList(), (UnitOfWork)model.Session);
            }

            issueDocumentID = issueDocumentRepo.SaveIssueDocument(model, PrincipalHelper.GetUserID());

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
            return issueDocumentRepo.DeleteIssueDocument(issueDocumentID);
        }

        #region Common

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool exitFormPage = true, bool completeAndPrint = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = issueDocumentID.ToString() }
            };

            if (completeAndPrint)//ko zaključimo izdajnico se redirectamo na printanje
            {
                Response.Redirect(ConcatenateURLForPrint(issueDocumentID, "IssueDocument", true));
            }
            else
            {
                if (isIDDeleted)
                    redirectString = "IssueDocumentTable.aspx";
                else if (!exitFormPage)
                {
                    queryStrings.Insert(0, new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                    redirectString = GenerateURI("IssueDocumentForm.aspx", queryStrings);
                }
                else
                    redirectString = GenerateURI("IssueDocumentTable.aspx", queryStrings);

                List<Enums.EmployeeSession> list = Enum.GetValues(typeof(Enums.EmployeeSession)).Cast<Enums.EmployeeSession>().ToList();
                ClearAllSessions(list, redirectString);
            }
        }

        private void ProcessUserAction(bool exitFormPage = true, bool completeIssueDocument = false)
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (userAction)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true, completeIssueDocument);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject(false, completeIssueDocument);
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing, exitFormPage, completeIssueDocument);
            }
        }
        #endregion

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split(';');

            if (split[0] == "UserAction")
            {
                object position = ASPxGridViewIssueDocumentPosition.GetRowValues(ASPxGridViewIssueDocumentPosition.FocusedRowIndex, "IssueDocumentPositionID");
                bool openPopup = SetSessionsAndOpenPopUp(split[1], Enums.IssueDocumentSession.IssueDocumentPositionID, position);
                AddValueToSession(Enums.IssueDocumentSession.IssueDocumentID, issueDocumentID);

                if (userAction == (int)Enums.UserAction.Add)
                {
                    AddOrEditEntityObject(true);
                    AddValueToSession(Enums.CommonSession.OpenPopupAfterRefresh, true);
                    ASPxWebControl.RedirectOnCallback(GenerateURI("IssueDocumentForm.aspx", (int)Enums.UserAction.Edit, issueDocumentID));
                }
                else
                {
                    if (openPopup)
                        PopupControlIssueDocumentPos.ShowOnPageLoad = true;
                }
            }
        }

        protected void PopupControlIssueDocumentPos_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.IssueDocumentSession.IssueDocumentID);
            RemoveSession(Enums.IssueDocumentSession.IssueDocumentPositionID);
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
        }

        protected void ASPxGridViewIssueDocumentPosition_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewIssueDocumentPosition, btnAdd, btnEdit, btnDelete);
        }

        protected void btnCompleteIssueDocument_Click(object sender, EventArgs e)
        {
            if (ASPxGridViewIssueDocumentPosition.VisibleRowCount > 0)
                ProcessUserAction(true, true);
            else
            {
                ShowWarningPopup("Prazna Izdajnica!", "Na izdajnico je potrebno dodati pozicije oz. material.");
                Master.NavigationBarMain.DataBind();
            }
        }
    }
}