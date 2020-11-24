using DevExpress.Xpo;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Utilities.Exceptions;
using ETT_Utilities.Resources;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ETT_Web.Infrastructure.Authentication;

namespace ETT_Web
{
    public partial class Home : System.Web.UI.Page
    {

        Authentication auth;

        Session session = null;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            session = XpoHelper.GetNewSession();

            if (Request.IsAuthenticated)
            {
                MasterPageFile = "~/MasterPages/Main.Master";
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            auth = new Authentication(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Request.IsAuthenticated)
            {
                ASPxFormLayoutLogin.Visible = false;
                FormLayoutWrap.Style.Add("display", "none");
                HomeContent.Style.Add("display", "block");

                Session["SessionEndModal"] = true;

                this.Master.PageHeadlineTitle = this.Title;
            }
            else
            {
                HomeContent.Style.Add("display", "none");

                bool showWarning = CommonMethods.ParseBool(Session["SessionEndModal"]);
                if (showWarning)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SessionExparation", String.Format("$('#sessionEndModal').modal('show')"), true);
                    //RemoveSession("SessionEndModal");
                    Session.Remove("SessionEndModal");
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                }

                UserCredentials obj = auth.GetUsernameAndPassword();
                if (obj != null)
                {
                    if (String.IsNullOrEmpty(txtUsername.Text) && String.IsNullOrEmpty(txtPassword.Text))
                    {
                        txtUsername.Text = obj.Username;
                        txtPassword.ClientSideEvents.Init = "function(s, e) {s.SetText('" + obj.Password + "');}";
                    }
                }
            }
        }
        protected void LoginCallback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            bool signInSuccess = false;
            string message = "";
            string username = "";// CommonMethods.Trim(txtUsername.Text);
            string password = "";//CommonMethods.Trim(txtPassword.Text);

            try
            {

                if (e.Parameter.Contains("SignInUserCredentials"))
                {
                    username = CommonMethods.Trim(txtUsername.Text);
                    password = CommonMethods.Trim(txtPassword.Text);
                }

                if (username != "" && password != "")
                {
                    signInSuccess = auth.Authenticate(username, password, rememberMeCheckBox.Checked);
                }

            }
            catch (EmployeeCredentialsException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);
                message = AuthenticationValidation_Exception.res_01;
            }


            if (signInSuccess)
            {
                Session.Remove("PreviousPage");

                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("Expires", "0");
            }
            else
            {
                LoginCallback.JSProperties["cpResult"] = message;
            }
        }
    }
}