using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.Models;
using ETT_Utilities.Common;
using ETT_Utilities.Exceptions;
using ETT_Utilities.Helpers;
using ETT_Utilities.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ETT_Web.Infrastructure
{
    public class Authentication
    {
        private IUserRepository userRepo;
        private IActiveUserRepository activeUserRepo;
        private IUserActivityRepository userActRepo;

        public Authentication(Session session = null)
        {
            userRepo = new UserRepository(session);
            activeUserRepo = new ActiveUserRepository(session);
            userActRepo = new UserActivityRepository(session);
        }


        public bool Authenticate(string username, string password, bool rememberMe)
        {
            UserModel user = null;

            user = userRepo.UserLogIn(username, password);
            SerializeUser(user);

            if (rememberMe)
            {
                if (HttpContext.Current.Request.Cookies["RememberMeCookie"] != null)
                    CookieHelper.TryRemoveCookie("RememberMeCookie");

                string jsonUser = JsonConvert.SerializeObject(new UserCredentials { Password = password, Username = username });
                FormsAuthenticationTicket rememberMeTicket = new FormsAuthenticationTicket(2, username, DateTime.Now, DateTime.Now.AddDays(30), true, jsonUser);
                string encyptTicket = FormsAuthentication.Encrypt(rememberMeTicket);
                HttpCookie rememberMeCookie = new HttpCookie("RememberMeCookie", encyptTicket) { HttpOnly = false, Expires = DateTime.Now.AddMonths(1) };
                HttpContext.Current.Response.Cookies.Add(rememberMeCookie);
            }

            return true;
        }

        private void SerializeUser(UserModel user)
        {
            if (user != null)
            {
                string sessionExpires = ConfigurationManager.AppSettings["SessionTimeoutInMinutes"].ToString();

                //Set user activity to true
                user.ActiveUserID = activeUserRepo.SaveUserLoggedInActivity(true, user.ID, CommonMethods.ParseInt(sessionExpires));
                userActRepo.SaveUserActivity(Enums.UserActivityEnum.LOGIN, user.ActiveUserID, "");

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string userData = serializer.Serialize(user);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     Guid.NewGuid().ToString(),
                     DateTime.Now,
                     DateTime.Now.AddMinutes(CommonMethods.ParseDouble(sessionExpires)),
                     false,
                     userData);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket) { HttpOnly = false, Expires = DateTime.Now.AddMonths(1) };
                HttpContext.Current.Response.Cookies.Add(faCookie);

                CookieHelper.SetCookieValue(Enums.Cookies.SessionExpires.ToString(), sessionExpires);

            }
            else
                throw new EmployeeCredentialsException(AuthenticationValidation_Exception.res_01);
        }

        public UserCredentials GetUsernameAndPassword()
        {
            var rememberhCookie = HttpContext.Current.Request.Cookies["RememberMeCookie"];
            if (rememberhCookie != null)
            {
                FormsAuthenticationTicket rememberTicket = FormsAuthentication.Decrypt(rememberhCookie.Value);

                UserCredentials obj = JsonConvert.DeserializeObject<UserCredentials>(rememberTicket.UserData);

                return obj;
            }
            return null;
        }

        public class UserCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}