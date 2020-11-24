using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models.Location
{
    public class LocationAPIModel
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object parent_id { get; set; }
        public object lft { get; set; }
        public object rgt { get; set; }
        public object depth { get; set; }
        public string name { get; set; }
        public object belongs_to { get; set; }
        public string is_client { get; set; }
        public string description { get; set; }
    }
}