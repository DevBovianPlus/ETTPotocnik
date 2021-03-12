using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using System;
using System.Collections.Generic;

namespace ETT_SchedularLibrary
{
    public class Users
    {
        private IEmployeeRepository utilityRepo;
        private Session session;

        public Employee GetEmployee()
        {
            session = XpoHelper.GetNewSession();
            utilityRepo = new EmployeeRepository(session);

            return utilityRepo.GetEmployeeByID(2, session);
        }
    }
}
