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
    public partial class InventoryStockTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            session = XpoHelper.GetNewSession();
            XpoDSInventoryStock.Session = session;

            ASPxGridViewInventoryStock.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanelInventoryStock_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewInventoryStock.DataBind();
            }
            else
            {
                object inventoryStock = ASPxGridViewInventoryStock.GetRowValues(ASPxGridViewInventoryStock.FocusedRowIndex, "InventoryStockID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.InventoryStockSession.InventoryStockID, inventoryStock);

                if (openPopup)
                    PopupControlInventoryStock.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewInventoryStock_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewInventoryStock, btnAdd, btnEdit, btnDelete);
        }

        protected void PopupControlInventoryStock_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.InventoryStockSession.InventoryStockID);
            RemoveSession(Enums.InventoryStockSession.InventoryStockModel);
        }
    }
}