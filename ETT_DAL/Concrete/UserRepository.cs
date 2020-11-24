using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Models;
using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Utilities.Exceptions;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETT_DAL.Helpers;
using ETT_DAL.Models.Users;

namespace ETT_DAL.Concrete
{
    public class UserRepository : IUserRepository
    {
        Session session;

        public UserRepository(Session session = null)
        {
            if (session == null)
                session = XpoHelper.GetNewSession();

            this.session = session;
        }

        public UserModel UserLogIn(string userName, string password)
        {
            try
            {
                XPQuery<Users> list = session.Query<Users>();

                Users user = list.Where(u => (u.Username != null && u.Username.CompareTo(userName) == 0) && (u.Password != null && u.Password.CompareTo(password) == 0)).FirstOrDefault();
                UserModel model = null;

                if (user != null)
                {
                    if (!user.GrantAppAccess)
                        throw new EmployeeCredentialsException(AuthenticationValidation_Exception.res_03);


                    if (String.Compare(user.Username, userName, false) != 0 && String.Compare(user.Password, password) != 0)
                        return null;

                    model = FillUserModel(user);

                }

                return model;
            }
            catch (EmployeeCredentialsException ex)
            {
                throw new EmployeeCredentialsException(ex.Message);
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private UserModel FillUserModel(Users user)
        {
            UserModel model = new UserModel();

            model.ID = user.UserID;

            model.FirstName = user.EmployeeID.Firstname;
            model.LastName = user.EmployeeID.Lastname;
            model.Email = user.EmployeeID.Email;
            model.ProfileImage = user.EmployeeID.Picture;



            model.RoleName = user.RoleID.Name;

            if (user.RoleID != null)
            {
                model.RoleID = user.RoleID.RoleID;
                model.Role = user.RoleID.Code;
                model.RoleName = user.RoleID.Name;
            }

            return model;
        }

        public Users GetUserByID(int uId, Session currentSession = null)
        {
            try
            {
                XPQuery<Users> user = null;

                if (currentSession != null)
                    user = currentSession.Query<Users>();
                else
                    user = session.Query<Users>();

                return user.Where(u => u.UserID == uId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveUser(Users model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.UserID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.UserID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_02, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteUser(int uId)
        {
            try
            {
                GetUserByID(uId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_03, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteUser(Users model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_03, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Role GetRoleByID(int rId, Session currentSession = null)
        {
            try
            {
                XPQuery<Role> role = null;

                if (currentSession != null)
                    role = currentSession.Query<Role>();
                else
                    role = session.Query<Role>();

                return role.Where(r => r.RoleID == rId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Role GetRoleByCode(string roleCode, Session currentSession = null)
        {
            try
            {
                XPQuery<Role> role = null;

                if (currentSession != null)
                    role = currentSession.Query<Role>();
                else
                    role = session.Query<Role>();

                return role.Where(r => r.Code == roleCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetUserCountByEmployeeID(int eId, Session currentSession = null)
        {
            try
            {
                XPQuery<Users> user = null;

                if (currentSession != null)
                    user = currentSession.Query<Users>();
                else
                    user = session.Query<Users>();

                return user.Where(u => u.EmployeeID.EmployeeID == eId).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<UserAPIModel> GetUsersMobile()
        {
            try
            {
                XPQuery<Users> user = session.Query<Users>();
                List<UserAPIModel> usersAPI = new List<UserAPIModel>();
                var roleCode = Enums.UserRole.Miner.ToString();
                var users = (from u in user
                             where u.RoleID.Code == roleCode
                             select u).ToList();

                foreach (var u in users)
                {
                    var usr = new UserAPIModel
                    {
                        active = u.GrantAppAccess ? "1" : "0",
                        avatar = "",
                        created_at = u.tsInsert.Date.ToString("yyyy-MM-dd") + " " + u.tsInsert.ToString("HH:mm:ss"),//u.tsInsert.Year.ToString() + "-" + u.tsInsert.Month.ToString() + "-" + u.tsInsert.Day.ToString() + " " + u.tsInsert.Hour + ":" + u.tsInsert.Minute + ":" + u.tsInsert.Second,
                        email = u.EmployeeID.Email,
                        first_name = u.EmployeeID.Firstname,
                        group_id = u.RoleID.Code + " ",
                        id = u.UserID.ToString(),
                        last_login = u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault() != null ?
                                    (u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.Date.ToString("yyyy-MM-dd") +
                                    " " +
                                    u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.ToString("HH:mm:ss")) : "",
                        last_name = u.EmployeeID.Lastname,
                        login_attempt = "0",
                        name = u.RoleID.Name,
                        password = "",
                        updated_at = u.tsUpdate.Date.ToString("yyyy-MM-dd") + " " + u.tsUpdate.ToString("HH:mm:ss"),
                        username = u.Username
                    };
                    usersAPI.Add(usr);
                }

                var query = from u in user
                            select new UserAPIModel
                            {
                                active = u.GrantAppAccess ? "1" : "0",
                                avatar = "",
                                created_at = u.tsInsert.Year.ToString() + "-" + u.tsInsert.Month.ToString() + "-" + u.tsInsert.Day.ToString() + " " + u.tsInsert.Hour + ":" + u.tsInsert.Minute + ":" + u.tsInsert.Second,
                                email = u.EmployeeID.Email,
                                first_name = u.EmployeeID.Firstname,
                                group_id = u.RoleID.Code,
                                id = u.UserID.ToString(),
                                last_login = u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault() != null ?
                                    (u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.Year + "-" +
                                    u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.Month + "-" +
                                    u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.Day + " " +
                                    u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.Hour + ":" +
                                    u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.Minute + ":" +
                                    u.ActiveUsers.OrderByDescending(o => o.LoginDate).FirstOrDefault().LoginDate.Second) : "",
                                last_name = u.EmployeeID.Lastname,
                                login_attempt = "0",
                                name = u.RoleID.Name,
                                password = "",
                                updated_at = u.tsUpdate.Year.ToString() + "-" + u.tsUpdate.Month.ToString() + "-" + u.tsUpdate.Day.ToString() + " " + u.tsUpdate.Hour + ":" + u.tsUpdate.Minute + ":" + u.tsUpdate.Second,
                                username = u.Username
                            };

                return usersAPI;
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