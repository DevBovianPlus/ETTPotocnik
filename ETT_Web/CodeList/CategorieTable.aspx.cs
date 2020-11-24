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
    public partial class CategorieTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;
            session = XpoHelper.GetNewSession();

            XpoDSCategorie.Session = session;

            ASPxGridViewCategorie.Settings.GridLines = GridLines.Both; 
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewCategorie.DataBind();
            }
            else
            {
                object categorie = ASPxGridViewCategorie.GetRowValues(ASPxGridViewCategorie.FocusedRowIndex, "CategorieID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.CategorieSession.CategorieID, categorie);

                if (openPopup)
                    PopupControlCategorie.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewCategorie_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewCategorie, btnAdd, btnEdit, btnDelete);
        }

        protected void PopupControlCategorie_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.CategorieSession.CategorieID);
            RemoveSession(Enums.CategorieSession.CategorieModel);
        }
    }
}