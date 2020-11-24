using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class UserActivityRepository : IUserActivityRepository
    {
        Session session;
        IActiveUserRepository activeUserRepo;

        public UserActivityRepository(Session session = null)
        {
            if (session == null)
                session = XpoHelper.GetNewSession();

            this.session = session;

            activeUserRepo = new ActiveUserRepository(session);
        }

        public void SaveUserActivity(Enums.UserActivityEnum code, int activeUserID, string dbTableName, string notes = "")
        {
            UserActivity userAct = new UserActivity(session);

            userAct.UserActivityID = 0;
            userAct.ActiveUserID = activeUserRepo.GetActiveUserByID(activeUserID, session);
            userAct.Code = code.ToString();
            userAct.Name = GetStringBasedOnCode(code) + dbTableName;
            userAct.Notes = notes;
            userAct.ts = DateTime.Now;

            userAct.Save();
        }

        private string GetStringBasedOnCode(Enums.UserActivityEnum code)
        {
            if (code == Enums.UserActivityEnum.LOGIN)
                return UserActivityRes.res_01;
            else if (code == Enums.UserActivityEnum.LOGOUT)
                return UserActivityRes.res_02;
            else if (code == Enums.UserActivityEnum.ADD_ENTRY)
                return UserActivityRes.res_03;
            else if (code == Enums.UserActivityEnum.EDIT_ENTRY)
                return UserActivityRes.res_04;
            else if (code == Enums.UserActivityEnum.DELETE_ENTRY)
                return UserActivityRes.res_05;

            return "";
        }

        public List<UserActivity> GetUserActivityByActiveUserID(int auId)
        {
            try
            {
                XPQuery<UserActivity> userAct = null;

                userAct = session.Query<UserActivity>();

                return userAct.Where(ua => ua.ActiveUserID.ActiveUserID == auId).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}