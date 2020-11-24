using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_WebAPI.Models
{
    public class WebAPIErrorModel
    {
        public string error { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }
}