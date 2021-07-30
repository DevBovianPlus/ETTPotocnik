using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ETT_UtilityServiceDay.Common
{
    public class CommonMethods
    {
        public static void LogThis(string message)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            string sMsg = DateTime.Now + " " + message + Environment.NewLine;

            File.AppendAllText(directory + "log.txt", sMsg);

        }

        public static void getError(Exception e, ref string errors)
        {
            if (e.GetType() != typeof(HttpException)) errors += " -------- " + e.ToString();
            if (e.InnerException != null) getError(e.InnerException, ref errors);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public static string ConcatenateErrorIN_DB(string resource, string error, string methodName)
        {
            return resource + " in method : " + methodName + " Error : " + error;
        }
    }
}
