using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_WebAPI.Common
{
    public class WebAPIEnums
    {
        public enum AuthenticationStatus
        {
            OK,
            FAILED,
            CREDENTIAL_INVALID
        }

        public enum Module
        {
            inventory,
            location,
            supplier,
            users,
        }
    }
}