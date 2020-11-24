using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IUserActivityRepository
    {
        void SaveUserActivity(Enums.UserActivityEnum code, int activeUserID, string dbTableName, string notes = "");
        List<UserActivity> GetUserActivityByActiveUserID(int auId);
    }
}
