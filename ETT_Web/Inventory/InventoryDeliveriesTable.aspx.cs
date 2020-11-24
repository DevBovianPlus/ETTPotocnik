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

namespace ETT_Web.Inventory
{
    public partial class InventoryDeliveriesTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            session = XpoHelper.GetNewSession();
            XpoDSInventoryDeliveries.Session = session;
            XpoDSInventoryDeliveries.Session.CaseSensitive = false;

            ASPxGridViewInventoryDeliveries.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewInventoryStock_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewInventoryDeliveries, btnAdd, btnEdit, btnDelete);
        }

        protected void ASPxGridViewInventoryDeliveries_DataBound(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewInventoryDeliveries_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ClearAllSessions(Enum.GetValues(typeof(Enums.InventoryDeliveriesSession)).Cast<Enums.InventoryDeliveriesSession>().ToList());

            object valueID = 0;
            int userAction = CommonMethods.ParseInt(e.Parameters);
            if (userAction != (int)Enums.UserAction.Add)
                 valueID = ASPxGridViewInventoryDeliveries.GetRowValues(ASPxGridViewInventoryDeliveries.FocusedRowIndex, "InventoryDeliveriesID");

            ASPxWebControl.RedirectOnCallback(GenerateURI("InventoryDeliveriesForm.aspx", userAction, valueID));
        }
    }
}