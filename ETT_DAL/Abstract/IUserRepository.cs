using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Models;
using ETT_DAL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IUserRepository
    {
        UserModel UserLogIn(string userName, string password);
        Users GetUserByID(int uId, Session currentSession = null);
        int SaveUser(Users model, int userID = 0);
        bool DeleteUser(int uId);
        bool DeleteUser(Users model);
        Role GetRoleByID(int rId, Session currentSession = null);
        Role GetRoleByCode(string roleCode, Session currentSession = null);
        int GetUserCountByEmployeeID(int eId, Session currentSession = null);
        List<UserAPIModel> GetUsersMobile();
    }
}
