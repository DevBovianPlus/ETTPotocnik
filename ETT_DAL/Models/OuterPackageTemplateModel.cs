using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models
{
    public class OuterPackageTemplateModel
    {
        public string PackageUID { get; set; }
        public string NewOuterPackage { get; set; }//če obstajajo še kakšne druge palete??? - več nivojev kot 4
        public string ChildPackages { get; set; }
    }
}