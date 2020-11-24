using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ETT_DAL.Models.XML
{
    public class Item
    {
        /// <summary>
        /// Atome/Product uid
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Concatenate string of hierarchy UID (from outer package to atome/product+)
        /// </summary>
        public string PackagesUIDs { get; set; }

        /// <summary>
        /// Atome/product xml element
        /// </summary>
        public XElement atomeElement { get; set; }

        /// <summary>
        /// Concatenate string of hierarchy SID (from outer package to atome/product+)
        /// </summary>
        public string PackagesSIDs { get; set; }


        public List<Item> Parents { get; set; }

        public decimal Quantity { get; set; }

        public string MeasuringUnitCode { get; set; }
    }
}