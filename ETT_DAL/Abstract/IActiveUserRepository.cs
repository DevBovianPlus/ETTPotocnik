using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IActiveUserRepository
    {
        ActiveUser GetActiveUserByUserID(int userID, Session currentSession = null);
        void SaveActiveUser(int userID, int sessionExpiresMin = 0);
        int SaveUserLoggedInActivity(bool active, int userID, int sessionExpiresMin = 0);
        void SaveLastRequest(int userID);
        List<ActiveUser> GetHistoryActiveUsers();
        void UpdateUsersLoginActivity();
        List<ActiveUser> GetAllActiveUsersForCurrentDay(Session currentSession = null);
        void UpdateActiveUsersFromPreviousDays();
        ActiveUser GetActiveUserByID(int activeUserID, Session currentSession = null);
    }
}
