using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class EmployeeRepository : IEmployeeRepository
    {
        Session session;

        public EmployeeRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }

        public Employee GetEmployeeByID(int eId, Session currentSession = null)
        {
            try
            {
                XPQuery<Employee> employee = null;

                if (currentSession != null)
                    employee = currentSession.Query<Employee>();
                else
                    employee = session.Query<Employee>();

                return employee.Where(e => e.EmployeeID == eId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_19, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveEmployee(Employee model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.EmployeeID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.EmployeeID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_20, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteEmployee(int eId)
        {
            try
            {
                GetEmployeeByID(eId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_21, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteEmployee(Employee model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_21, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Employee GetEmployeeByUserID(int userId, Session currentSession = null)
        {
            try
            {
                XPQuery<Users> employee = null;

                if (currentSession != null)
                    employee = currentSession.Query<Users>();
                else
                    employee = session.Query<Users>();

                var obj = employee.Where(u => u.UserID == userId).FirstOrDefault();

                if (obj != null)
                    return obj.EmployeeID;

                return null;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_19, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}