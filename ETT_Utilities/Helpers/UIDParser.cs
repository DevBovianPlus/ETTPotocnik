using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Utilities.Helpers
{
    public class UIDParser
    {

        public static string ExtractUIDFromCode(string code)
        {
            int index =  code.IndexOf(((int)Codes.UIDCode).ToString());
            //code.
            return "#";
        }
    }

    enum Codes
    {
        CountryCode = 90,
        UIDCode = 250,
        ManufactoryDate = 11,
        ProductCode = 240,
    }
}