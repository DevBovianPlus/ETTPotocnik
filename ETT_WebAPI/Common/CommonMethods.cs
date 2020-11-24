using ETT_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ETT_WebAPI.Common
{
    public static class CommonMethods
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static WebAPIEnums.AuthenticationStatus AuthenticateWebAPI(string token)
        {
            try
            {
                if (!String.IsNullOrEmpty(token))
                {
                    token = token.Remove(0, token.IndexOf(" ") + 1);
                    string value = Base64Decode(token);
                    string[] split = value.Split(':');

                    string user = ConfigurationManager.AppSettings["User"].ToString();
                    string pass = ConfigurationManager.AppSettings["Pass"].ToString();

                    if ((user != null && user.CompareTo(split[0]) == 0) && (pass != null && pass.CompareTo(split[1]) == 0))
                    {
                        if (String.Compare(split[0], user, false) != 0 && String.Compare(split[1], pass) != 0)
                            return WebAPIEnums.AuthenticationStatus.CREDENTIAL_INVALID;
                    }
                    else
                        return WebAPIEnums.AuthenticationStatus.CREDENTIAL_INVALID;

                }
                else
                    return WebAPIEnums.AuthenticationStatus.FAILED;

                return WebAPIEnums.AuthenticationStatus.OK;
            }
            catch (Exception ex)
            {
                return WebAPIEnums.AuthenticationStatus.FAILED;
            }
        }

        public static WebAPIErrorModel GetAutheticationError(WebAPIEnums.AuthenticationStatus error)
        {
            return new WebAPIErrorModel
            {
                error = "true",
                code = GetHttpStatusCode(error).ToString(),
                message = GetAuthError(error)
            };
        }

        private static string GetAuthError(WebAPIEnums.AuthenticationStatus error)
        {
            if (error == WebAPIEnums.AuthenticationStatus.CREDENTIAL_INVALID)
                return "Invalid authenticated params!";
            else if (error == WebAPIEnums.AuthenticationStatus.FAILED)
                return "Invalid authorization!";

            return "Unknown error occurred";
        }

        private static int GetHttpStatusCode(WebAPIEnums.AuthenticationStatus error)
        {
            if (error == WebAPIEnums.AuthenticationStatus.CREDENTIAL_INVALID)
                return (int)HttpStatusCode.Unauthorized;
            else if (error == WebAPIEnums.AuthenticationStatus.FAILED)
                return (int)HttpStatusCode.BadRequest;

            return (int)HttpStatusCode.BadRequest;
        }
    }
}