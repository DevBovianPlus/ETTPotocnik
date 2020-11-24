using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Abstract
{
    public interface IEmployeeRepository
    {
        Employee GetEmployeeByID(int eId, Session currentSession = null);
        int SaveEmployee(Employee model, int userID = 0);
        bool DeleteEmployee(int eId);
        bool DeleteEmployee(Employee model);
        Employee GetEmployeeByUserID(int userId, Session currentSession = null);
    }
}