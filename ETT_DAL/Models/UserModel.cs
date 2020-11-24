using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public int RoleID { get; set; }
        public int RoleWeight { get; set; }
        public string Job { get; set; }
        public DateTime dateCreated { get; set; }
        public string ProfileImage { get; set; }

        public int ActiveUserID { get; set; }
    }
}