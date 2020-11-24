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
using System.Drawing;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;

namespace ETT_Web.Admin
{
    public partial class TrackUserLogin : ServerMasterPage
    {
        Session session;
        private IActiveUserRepository activeUserRepo;
        bool filterByPeriod;
        IUserActivityRepository userActRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            AllowUserWithRole(Enums.UserRole.SuperAdmin);

            session = XpoHelper.GetNewSession();

            activeUserRepo = new ActiveUserRepository(session);
            userActRepo = new UserActivityRepository(session);

            ASPxGridViewActiveUser.Settings.GridLines = GridLines.Both; 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxGridViewActiveUser.DataBind();
            }
        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "FilerByPeriod")
            {
                filterByPeriod = true;
                ASPxGridViewActiveUser.DataBind();
            }
        }

        protected void ASPxGridViewActiveUser_DataBinding(object sender, EventArgs e)
        {
            activeUserRepo.UpdateUsersLoginActivity();
            List<ActiveUser> list = activeUserRepo.GetHistoryActiveUsers();

            if (list != null && FilterThroughHistoryCheckBox.Checked)
            {
                list = list.Where(au => au.LoginDate.Date >= DateEditDateFrom.Date.Date && au.LoginDate.Date <= DateEditDateTo.Date.Date).ToList();
            }
            else if (list != null)
            {
                list = list.Where(au => au.LoginDate.Date == DateTime.Today.Date).ToList();
            }

            (sender as ASPxGridView).DataSource = list.OrderByDescending(l => l.IsActive);
        }

        protected void ASPxGridViewActiveUser_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "IsActive")
            {
                e.DisplayText = CommonMethods.ParseBool(e.Value) == true ? "DA" : "NE";
            }
        }

        protected void ASPxGridViewActiveUser_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName.Equals("IsActive"))
            {
                if (!CommonMethods.ParseBool(e.GetValue("IsActive")))
                {
                    e.Cell.BackColor = Color.Tomato;
                }
                else if (CommonMethods.ParseBool(e.GetValue("IsActive")))
                {
                    e.Cell.BackColor = Color.LightGreen;
                }
            }
        }

        protected void ASPxGridViewUserActivity_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["ActiveUserID"] = (sender as ASPxGridView).GetMasterRowKeyValue();
        }

        protected void ASPxGridViewUserActivity_DataBinding(object sender, EventArgs e)
        {
            int activeUserId = CommonMethods.ParseInt(Session["ActiveUserID"]);
            (sender as ASPxGridView).DataSource = userActRepo.GetUserActivityByActiveUserID(activeUserId);
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }
    }
}