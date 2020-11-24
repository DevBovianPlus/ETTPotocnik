using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models.XML
{
    public class SummaryItemModel
    {
        public string PSN { get; set; }
        public string SID { get; set;}
        public string ProducerProductCode { get; set; }
        public string ProducerProductName { get; set; }
        public int ItemQuantity { get; set; }
        public string PackagingLevel { get; set; }
        public DateTime ProductionDate { get; set; }
        public string NEW { get; set; }
        public decimal Length { get; set; }
        public string UnitOfMeasure { get; set; }
        public int CountOfTradeUnits { get; set; }
        public bool IsTopLevelElement { get; set; }
        public int ProductItemCount { get; set; }
    }
}