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
    public partial class MeasuringUnitTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;
            session = XpoHelper.GetNewSession();

            XpoDSMeasuringUnit.Session = session;

            ASPxGridViewMeasuringUnit.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewMeasuringUnit.DataBind();
            }
            else
            {
                object measuringUnit = ASPxGridViewMeasuringUnit.GetRowValues(ASPxGridViewMeasuringUnit.FocusedRowIndex, "MeasuringUnitID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.MeasuringUnitSession.MeasuringUnitID, measuringUnit);

                if (openPopup)
                    PopupControlMeasuringUnit.ShowOnPageLoad = true;
            }
        }

        protected void PopupControlMeasuringUnit_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.MeasuringUnitSession.MeasuringUnitID);
            RemoveSession(Enums.MeasuringUnitSession.MeasuringUnitModel);
        }

        protected void ASPxGridViewMeasuringUnit_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewMeasuringUnit, btnAdd, btnEdit, btnDelete);
        }
    }
}