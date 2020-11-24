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

namespace ETT_Web.IssueDocuments
{
    public partial class SearchByUID_popup : ServerMasterPage
    {
        Session session;
        IInventoryRepository inventoryRepo;
        string uid;

        protected void Page_Init(object sender, EventArgs e)
        {
            uid = GetStringValueFromSession(Enums.IssueDocumentSession.SearchUIDValue);

            session = XpoHelper.GetNewSession();

            XpoDSInventoryDeliveries.Session = session;

            inventoryRepo = new InventoryRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUID250.Text = uid;
            }
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false, int selectedUID = 0)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.IssueDocumentSession.SearchUIDValue);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', {2});", confirmCancelAction, "SearchByUID", selectedUID.ToString()), true);

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var inventoryID = ASPxGridViewInventoryDeliveries.GetSelectedFieldValues("InventoryDeliveriesID").OfType<int>().FirstOrDefault();

            
            RemoveSessionsAndClosePopUP(true, inventoryID);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void CallbakcPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {

        }

        protected void ASPxGridViewInventoryDeliveries_DataBound(object sender, EventArgs e)
        {

        }
    }
}