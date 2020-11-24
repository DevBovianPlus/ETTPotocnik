using DevExpress.Web;
using DevExpress.Xpo;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
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

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            session = XpoHelper.GetNewSession();
            XpoDSMobileTransaction.Session = session;

            ASPxGridViewMobileTransaction.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanelMobileTransaction_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewMobileTransaction.DataBind();
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
    }
}