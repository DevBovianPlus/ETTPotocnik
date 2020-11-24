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
    public class ActiveUserRepository : IActiveUserRepository
    {
        Session session;
        IEmployeeRepository employeeRepo;
        IUserRepository userRepo;

        public ActiveUserRepository(Session session = null)
        {
            if (session == null)
                session = XpoHelper.GetNewSession();

            this.session = session;

            employeeRepo = new EmployeeRepository(session);
            userRepo = new UserRepository(session);
        }

        public ActiveUser GetActiveUserByUserID(int userID, Session currentSession = null)
        {
            try
            {
                XPQuery<ActiveUser> activeUser = null;

                if (currentSession != null)
                    activeUser = currentSession.Query<ActiveUser>();
                else
                    activeUser = session.Query<ActiveUser>();

                return activeUser.Where(au => au.UserID.UserID == userID).OrderByDescending(au => au.LoginDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveActiveUser(int userID, int sessionExpiresMin = 0)
        {
            try
            {
                if (!IsUserTrackAllowed()) return;

                ActiveUser activeUser = GetActiveUserByUserID(userID);

                if (activeUser != null)
                {
                    //če v trenutnem dnevu še ni zabeležene prijave se doda nova drugače se posodobijo vrednosti na obstoječi prijavi
                    if (activeUser.LoginDate.Date == DateTime.Today.Date)
                    {
                        activeUser.LoginDate = DateTime.Now;
                        activeUser.IsActive = true;
                        activeUser.LastRequestTS = DateTime.Now;
                        activeUser.RequestCount += 1;
                        activeUser.SessionExpireMin = sessionExpiresMin;

                        activeUser.Save();
                    }
                    else
                    {
                        CreateModel(userID).Save();
                    }
                }
                else
                {
                    CreateModel(userID).Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_23, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveUserLoggedInActivity(bool active, int userID, int sessionExpiresMin = 0)
        {
            try
            {
                if (!IsUserTrackAllowed()) return -1;

                ActiveUser activeUser = GetActiveUserByUserID(userID);

                if (activeUser != null)
                {
                    //če v trenutnem dnevu še ni zabeležene prijave se doda nova drugače se posodobijo vrednosti na obstoječi prijavi
                    if (activeUser.LoginDate.Date == DateTime.Today.Date)
                    {
                        activeUser.LoginDate = DateTime.Now;
                        activeUser.IsActive = active;
                        activeUser.LastRequestTS = DateTime.Now;
                        activeUser.RequestCount += 1;
                        activeUser.SessionExpireMin = active ? sessionExpiresMin : activeUser.SessionExpireMin;

                        activeUser.Save();
                        return activeUser.ActiveUserID;
                    }
                    else
                    {
                        activeUser = CreateModel(userID, active, sessionExpiresMin);
                        activeUser.Save();
                        return activeUser.ActiveUserID;
                    }
                }
                else
                {
                    activeUser = CreateModel(userID, active, sessionExpiresMin);
                    activeUser.Save();
                    return activeUser.ActiveUserID;
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_23, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private ActiveUser CreateModel(int userID, bool isActive = true, int sessionExpiresMin = 0)
        {
            ActiveUser newUser = new ActiveUser(session);
            newUser.ActiveUserID = 0;
            newUser.IsActive = true;
            newUser.LastRequestTS = DateTime.Now;
            newUser.LoginDate = DateTime.Now;
            newUser.RequestCount += 1;
            newUser.ts = DateTime.Now;
            newUser.SessionExpireMin = sessionExpiresMin;
            newUser.UserID = userRepo.GetUserByID(userID, newUser.Session);

            return newUser;
        }

        public void SaveLastRequest(int userID)
        {
            try
            {
                if (!IsUserTrackAllowed()) return;

                ActiveUser activeUser = GetActiveUserByUserID(userID);

                if (activeUser != null)
                {
                    activeUser.LastRequestTS = DateTime.Now;
                    activeUser.RequestCount += 1;

                    activeUser.Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_23, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<ActiveUser> GetAllActiveUsersForCurrentDay(Session currentSession = null)
        {
            try
            {
                XPQuery<ActiveUser> activeUser = null;

                if (currentSession != null)
                    activeUser = currentSession.Query<ActiveUser>();
                else
                    activeUser = session.Query<ActiveUser>();

                return activeUser.Where(au => au.IsActive && au.LoginDate.Date == DateTime.Today.Date).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<ActiveUser> GetHistoryActiveUsers()
        {
            try
            {
                XPQuery<ActiveUser> activeUser = session.Query<ActiveUser>();

                return activeUser.Where(au => au.ActiveUserID == au.ActiveUserID).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void UpdateUsersLoginActivity()
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    //Prijave ki niso bile zaprte iz prejšnjih dni avtomatsko zapremo
                    UpdateActiveUsersFromPreviousDays();
                    List<ActiveUser> activeUsers = GetAllActiveUsersForCurrentDay(uow);
                    if (activeUsers != null)
                    {
                        DateTime currentDate = DateTime.Now;
                        foreach (var item in activeUsers)
                        {
                            //če je čas zadnjega request-a  + čas poteka seja večji (kasneje) od trenutnega časa potem nastavimo user IsActive polje na false
                            if (item.LastRequestTS.AddMinutes(item.SessionExpireMin).CompareTo(currentDate) <= 0)
                            {
                                item.IsActive = false;
                            }
                        }
                        uow.CommitChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_23, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private bool IsUserTrackAllowed()
        {
            try
            {
                bool isAllowed = CommonMethods.ParseBool(ConfigurationManager.AppSettings["TrackUserLogInActivity"].ToString());
                return isAllowed;
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + " " + ex.StackTrace);
                return false;
            }
        }

        public void UpdateActiveUsersFromPreviousDays()
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    XPQuery<ActiveUser> activeUser = uow.Query<ActiveUser>();
                    List<ActiveUser> activeUsers = activeUser.Where(au => au.IsActive && au.LoginDate.Date < DateTime.Today.Date).ToList();
                    if (activeUsers != null)
                    {
                        foreach (var item in activeUsers)
                        {
                            item.IsActive = false;
                        }
                        uow.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public ActiveUser GetActiveUserByID(int activeUserID, Session currentSession = null)
        {
            try
            {
                XPQuery<ActiveUser> activeUser = null;

                if (currentSession != null)
                    activeUser = currentSession.Query<ActiveUser>();
                else
                    activeUser = session.Query<ActiveUser>();

                return activeUser.Where(au => au.ActiveUserID == activeUserID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}