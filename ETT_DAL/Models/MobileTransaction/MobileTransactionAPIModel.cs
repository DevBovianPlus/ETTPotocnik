using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models.MobileTransaction
{
    public class MobileTransactionAPIModel
    {
        public int user_id { get; set; }
        public string code { get; set; }
        public int location_from_id { get; set; }
        public int location_to_id { get; set; }
        public int supplier_id { get; set; }
        public int inventory_id { get; set; }
        public int transaction_type { get; set; }
        public DateTime created_at { get; set; }
    }
}