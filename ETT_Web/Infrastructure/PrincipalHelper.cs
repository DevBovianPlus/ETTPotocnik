using ETT_Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.Infrastructure
{
    public class PrincipalHelper
    {
        public static UserPrincipal GetUserPrincipal()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return (UserPrincipal)HttpContext.Current.User;
            }

            return null;
        }

        public static bool IsUserSuperAdmin()
        {
            var princip = GetUserPrincipal();

            return princip != null ? princip.IsInRole(Enums.UserRole.SuperAdmin.ToString()) : false;
        }

        public static bool IsUserAdmin()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.Admin.ToString()) : false;
        }

        public static bool IsUserMiner()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.Miner.ToString()) : false;
        }

        public static bool IsUserEditor()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.Editor.ToString()) : false;
        }

        public static int GetUserID()
        {
            var princip = GetUserPrincipal();

            return princip != null ? princip.ID : 0;
        }
    }
}