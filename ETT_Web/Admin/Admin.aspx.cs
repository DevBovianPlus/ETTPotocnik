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
    public partial class Admin : ServerMasterPage
    {
        Session session;
        IUtilityServiceRepository utilityServiceRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            AllowUserWithRole(Enums.UserRole.SuperAdmin);

            session = XpoHelper.GetNewSession();

            utilityServiceRepo = new UtilityServiceRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnMatchMobileTransactions_Click(object sender, EventArgs e)
        {
            utilityServiceRepo.MatchMobileTransWithInventoryDeliveries();
            Master.NavigationBarMain.DataBind();
        }
    }
}