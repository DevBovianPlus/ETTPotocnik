using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models.Users
{
    public class UserAPIModel
    {
        public string id { get; set; }
        public string avatar { get; set; }
        public string group_id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        /// <summary>
        /// ne potrebujemo ga pošiljati preko api-ja
        /// </summary>
        public string password { get; set; }
        public string login_attempt { get; set; }
        public string created_at { get; set; }
        public string last_login { get; set; }
        public string updated_at { get; set; }
        public string active { get; set; }
    }
}