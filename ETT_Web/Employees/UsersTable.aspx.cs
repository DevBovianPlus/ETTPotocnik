﻿using DevExpress.Xpo;
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

namespace ETT_Web.Employees
{
    public partial class UsersTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;
            session = XpoHelper.GetNewSession();

            XpoDSUsers.Session = session;

            ASPxGridViewUsers.Settings.GridLines = GridLines.Both; 
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewUsers.DataBind();
            }
            else
            {
                object user = ASPxGridViewUsers.GetRowValues(ASPxGridViewUsers.FocusedRowIndex, "UserID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.EmployeeSession.UserID, user);

                if (openPopup)
                    PopupControlUser.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewUsers_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewUsers, btnAdd, btnEdit, btnDelete);
        }

        protected void PopupControlUser_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.EmployeeSession.UserID);
            RemoveSession(Enums.EmployeeSession.UserModel);
        }
    }
}