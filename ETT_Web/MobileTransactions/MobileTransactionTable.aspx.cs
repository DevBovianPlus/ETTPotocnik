using DevExpress.Web;
using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETT_Web.MobileTransactions
{
    public partial class MobileTransactionTable : ServerMasterPage
    {
        Session session;
        IIssueDocumentRepository issueDocumentRepo;
        IMobileTransactionRepository mobileTransactionRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            session = XpoHelper.GetNewSession();
            XpoDSMobileTransaction.Session = session;

            ASPxGridViewMobileTransaction.Settings.GridLines = GridLines.Both;
            issueDocumentRepo = new IssueDocumentRepository(session);
            mobileTransactionRepo = new MobileTransactionRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxGridViewMobileTransaction.DataBind();
        }

        protected void CallbackPanelMobileTransaction_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewMobileTransaction.DataBind();
            }
            else if (e.Parameter == "TransferToIssueDocument")
            {
                //pridobimo seznam izbranih lokacij
                List<int> selectedItems = ASPxGridViewMobileTransaction.GetSelectedFieldValues("InventoryDeliveriesLocationID.LocationToID.BuyerID.ClientID").OfType<int>().ToList();

                //preverimi če imajo vse izbrane transkacije enakega kupca
                int count = selectedItems.Count(si => si == selectedItems[0]);
                if (selectedItems.Count > 0 && selectedItems.Count == count)
                {
                    List<int> selectedItemsID = ASPxGridViewMobileTransaction.GetSelectedFieldValues("MobileTransactionID").OfType<int>().ToList();
                    issueDocumentRepo.CreateIssueDocumentFromMobileTransactions(selectedItemsID, PrincipalHelper.GetUserID(), selectedItems[0]);
                    ASPxGridViewMobileTransaction.Selection.UnselectAll();
                    CallbackPanelMobileTransaction.JSProperties["cpErrorIssueDocumentCreated"] = true;
                }
                else
                {
                    CallbackPanelMobileTransaction.JSProperties["cpErrorDifferentBuyers"] = true;
                    ASPxGridViewMobileTransaction.Selection.UnselectAll();
                    return;
                }
            }
            else
            {
                object mobileTransaction = ASPxGridViewMobileTransaction.GetRowValues(ASPxGridViewMobileTransaction.FocusedRowIndex, "MobileTransactionID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.MobileTransactionSession.MobileTransactionID, mobileTransaction);

                if (openPopup)
                    PopupControlMobileTransaction.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewMobileTransaction_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewMobileTransaction, btnAdd, btnEdit, btnDelete);
        }

        protected void PopupControlMobileTransaction_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.MobileTransactionSession.MobileTransactionID);
            RemoveSession(Enums.MobileTransactionSession.MobileTransactionModel);
        }

        protected void ASPxGridViewMobileTransaction_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "InventoryDeliveriesLocationID.NeedsMatching")
            {
                if (Convert.ToBoolean(e.Value))
                    e.DisplayText = "NE";
                else
                    e.DisplayText = "DA";
            }
        }

        protected void ASPxGridViewMobileTransaction_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            object isBuyer = ASPxGridViewMobileTransaction.GetRowValues(e.VisibleIndex, "InventoryDeliveriesLocationID.LocationToID.IsBuyer");
            object needsMatching = ASPxGridViewMobileTransaction.GetRowValues(e.VisibleIndex, "InventoryDeliveriesLocationID.NeedsMatching");

            if (CommonMethods.ParseBool(isBuyer) && CommonMethods.ParseBool(needsMatching))
                e.Visible = true;
            else
                e.Visible = false;
        }


        protected void ASPxGridViewMobileTransaction_DataBinding(object sender, EventArgs e)
        {
            DateTime dtFrom = DateTime.Now;
            DateTime dtTo = DateTime.MinValue;

            if (chkShowTransactionVse.Checked)
            {
                dtFrom = DateTime.Now.AddYears(-10);
                dtTo = DateTime.Now;
            }
            else
            {
                dtFrom = DateTime.Now.AddMonths(-3);
                dtTo = DateTime.Now;
            }

            List<MobileTransaction> list = mobileTransactionRepo.GetMobileTransactionByDates(dtFrom, dtTo, session);

            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }
        
        protected void btnIzberiVse_Click(object sender, EventArgs e)
        {
            ASPxGridViewMobileTransaction.DataBind();
            Master.NavigationBarMain.DataBind();
        }
    }
}