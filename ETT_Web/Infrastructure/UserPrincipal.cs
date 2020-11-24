using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ETT_Web.Infrastructure
{
    public class UserPrincipal : IPrincipal
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int RoleWeight { get; set; }
        public int ActiveUserID { get; set; }

        public IIdentity Identity
        {
            get;
            set;
        }

        public bool IsInRole(string role)
        {
            return Role == role;
        }
    }
}