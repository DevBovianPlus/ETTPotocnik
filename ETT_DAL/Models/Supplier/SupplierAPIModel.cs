using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models.Supplier
{
    public class SupplierAPIModel
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string postal_code { get; set; }
        public string zip_code { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string contact_title { get; set; }
        public string contact_name { get; set; }
        public string contact_phone { get; set; }
        public string contact_fax { get; set; }
        public string contact_email { get; set; }
    }
}