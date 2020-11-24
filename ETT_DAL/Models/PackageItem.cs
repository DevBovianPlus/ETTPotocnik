using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models
{
    public class PackageItem
    {
        public string UID { get; set; }
        public string SID { get; set; }
        public PackageItem Parent { get; set; }
        public List<PackageItem> Children { get; set; }
        public int TreeLevel { get; set; }
        public bool Visited { get; set; }
        public string NodeTemplate { get; set; }//html šablona trenutnega vozlišča - list drevesa tega ne potrebuje
        public string DescendantTemplate { get; set; }//html šablona trenutnega vozlišča z vsemi potomci - list drevesa tega ne potrebuje
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string Employee { get; set; }
        public string TimeStamp { get; set; }
    }
}