using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Utilities.Exceptions
{
    public class EmployeeCredentialsException : Exception
    {
        public EmployeeCredentialsException(string message)
           : base(message)
        {
        }

        public EmployeeCredentialsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}