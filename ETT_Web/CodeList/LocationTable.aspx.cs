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
    public partial class LocationTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;
            session = XpoHelper.GetNewSession();

            XpoDSLocation.Session = session;

            ASPxGridViewLocation.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewLocation.DataBind();
            }
            else
            {
                object location = ASPxGridViewLocation.GetRowValues(ASPxGridViewLocation.FocusedRowIndex, "LocationID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.LocationSession.LocationID, location);

                if (openPopup)
                    PopupControlLocation.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewLocation_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewLocation, btnAdd, btnEdit, btnDelete);
        }

        protected void ASPxGridViewLocation_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "IsBuyer")
            {
                if (Convert.ToBoolean(e.Value))
                    e.DisplayText = "DA";
                else
                    e.DisplayText = "NE";
            }

            if (e.Column.FieldName == "IsWarehouse")
            {
                if (Convert.ToBoolean(e.Value))
                    e.DisplayText = "DA";
                else
                    e.DisplayText = "NE";
            }
        }

        protected void PopupControlLocation_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.LocationSession.LocationID);
            RemoveSession(Enums.LocationSession.LocationModel);
        }
    }
}