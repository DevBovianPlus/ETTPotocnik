using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models
{
    public class ProductTemplateModel
    {
        public string ProductUID { get; set; }
        public string ProductSID { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string TransactionDateTime { get; set; }//datum in čas premika
        public string TransactionManager { get; set; }//ime in priimek zaposlenega ki je napravil premik
        public bool IsTransactionMade { get; set; }//je premik bil opravljen
        public string DisplayTransaction { get; set; }
    }
}