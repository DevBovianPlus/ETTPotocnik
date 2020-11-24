using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Models
{
    public class PackageTemplateModel
    {
        public string PackageUID { get; set; }
        public string PackageSID { get; set; }
        public string ChildPackages { get; set; }
        public string Products { get; set; }
        public string NumberOfProducts { get; set; }

    }
}