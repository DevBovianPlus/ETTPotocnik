using DevExpress.Xpo;
using ETT_DAL.Helpers;
using ETT_DAL.ETTPotocnik;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using ETT_Utilities.Common;

namespace ETT_Web.CodeList
{
    public partial class ProductTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;
            session = XpoHelper.GetNewSession();

            XpoDSProduct.Session = session;

            ASPxGridViewProduct.Settings.GridLines = GridLines.Both; 
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewProduct.DataBind();
            }
            else
            {
                object product = ASPxGridViewProduct.GetRowValues(ASPxGridViewProduct.FocusedRowIndex, "ProductID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.ProductSession.ProductID, product);

                if (openPopup)
                    PopupControlProduct.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewProduct_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewProduct, btnAdd, btnEdit, btnDelete);
        }

        protected void PopupControlProduct_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.ProductSession.ProductID);
            RemoveSession(Enums.ProductSession.ProductModel);
        }
    }
}