using DevExpress.Xpo;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Models.MobileTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IMobileTransactionRepository
    {
        MobileTransaction GetMobileTransactionByID(int mtId, Session currentSession = null);
        int SaveMobileTransaction(MobileTransaction model, int userID = 0);
        bool DeleteMobileTransaction(int mtId);

        bool DeleteMobileTransaction(MobileTransaction model);
        int SaveMobileTransaction(MobileTransactionAPIModel model);

        bool DeleteDuplicateMobileTransaction(Session currentSession = null);

        List<MobileTransactionModel> GetMobileTransactionByDates(DateTime dtFrom, DateTime dtTo, Session currentSession = null);
        List<MobileTransactionModel> GetDaySummaryMobileTransaction(DateTime dtFrom, DateTime dtTo, Session currentSession = null);
        List<DayTransaction> GetDaySummaryTransaction(DateTime dtFrom, DateTime dtTo, Session currentSession = null);
    }
}
