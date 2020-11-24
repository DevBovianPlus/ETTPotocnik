using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_WebAPI.Models
{
    public class ResponseContentModel<T>
    {
        public string total { get; set; }
        public T rows { get; set; }
        public Control control { get; set; }
        public string key { get; set; }
    }

    public class Control
    {
        public string page { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public string limit { get; set; }
    }
}