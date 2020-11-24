using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_Utilities.Common;
using ETT_Utilities.Helpers;
using ETT_Web.Infrastructure;

namespace ETT_Web.MasterPages {

    public partial class Main : System.Web.UI.MasterPage {

        private bool disableNavBar;
        private IActiveUserRepository activeUserRepo;
        private IUserActivityRepository userActRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            activeUserRepo = new ActiveUserRepository();
            userActRepo = new UserActivityRepository();
        }

        protected void Page_Load(object sender, EventArgs e) {

            if (Request.IsAuthenticated)
            {
                SetMainMenuBySignInRole();
                lblLogin.Text = PrincipalHelper.GetUserPrincipal().FirstName + " " + PrincipalHelper.GetUserPrincipal().LastName;

                CookieHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), DateTime.Now.ToString("dd M yyyy HH mm ss"));

                if (!String.IsNullOrEmpty(PrincipalHelper.GetUserPrincipal().ProfileImage))
                    headerProfileImage.Src = PrincipalHelper.GetUserPrincipal().ProfileImage.Replace(AppDomain.CurrentDomain.BaseDirectory, "/");
                else
                    headerProfileImage.Src = "/Images/defaultPerson.png";

                activeUserRepo.SaveLastRequest(PrincipalHelper.GetUserPrincipal().ID);
            }
            else
            {
                Session["PreviousPage"] = Request.RawUrl;
                CookieHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), "STOP");
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            CookieHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), "STOP");
            FormsAuthentication.SignOut();


            activeUserRepo.SaveUserLoggedInActivity(false, PrincipalHelper.GetUserPrincipal().ID);
            userActRepo.SaveUserActivity(Enums.UserActivityEnum.LOGOUT, PrincipalHelper.GetUserPrincipal().ActiveUserID, "");

            Response.Redirect("~/Home.aspx");
        }

        private void SetMainMenuBySignInRole()
        {
            if (PrincipalHelper.IsUserSuperAdmin())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.SuperAdmin.ToString());
            }
            else if (PrincipalHelper.IsUserAdmin())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Admin.ToString());
            }
            else if (PrincipalHelper.IsUserMiner())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Miner.ToString());
                ASPxPanelMenu.ClientVisible = false;
            }
            else if (PrincipalHelper.IsUserEditor())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Editor.ToString());
                //ASPxPanelMenu.ClientVisible = false;
            }
        }

        private void SetXmlDataSourceSetttings(string userRole)
        {
            XmlMenuDataSource.XPath = "MainMenu/" + userRole + "/Group";

            if (!DisableNavBar)
                NavBarMainMenu.Enabled = true;
            else
                NavBarMainMenu.Enabled = false;

        }

        public bool DisableNavBar
        {
            get { return disableNavBar; }
            set { disableNavBar = value; }
        }

        public ASPxNavBar NavigationBarMain
        {
            get { return NavBarMainMenu; }
        }

        public string PageHeadlineTitle
        {
            get { return PageHeadline.HeaderText; }
            set { PageHeadline.HeaderText = value; }
        }
    }
}