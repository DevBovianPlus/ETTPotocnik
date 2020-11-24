using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class EmployeeProvider : ServerMasterPage
    {
        public void SetEmployeeModel(Employee model)
        {
            AddValueToSession(Enums.EmployeeSession.EmployeeModel, model);
        }

        public Employee GetEmployeeModel()
        {
            if (SessionHasValue(Enums.EmployeeSession.EmployeeModel))
                return (Employee)GetValueFromSession(Enums.EmployeeSession.EmployeeModel);

            return null;
        }

        public void SetUserModel(Users model)
        {
            AddValueToSession(Enums.EmployeeSession.UserModel, model);
        }

        public Users GetUserModel()
        {
            if (SessionHasValue(Enums.EmployeeSession.UserModel))
                return (Users)GetValueFromSession(Enums.EmployeeSession.UserModel);

            return null;
        }
    }
}