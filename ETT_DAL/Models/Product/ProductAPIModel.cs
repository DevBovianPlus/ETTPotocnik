using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models.Product
{
    public class ProductAPIModel
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object deleted_at { get; set; }
        public string category_id { get; set; }
        public object user_id { get; set; }
        public string metric_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string manufacturers_code { get; set; }
        public string suppliers { get; set; }
    }
}