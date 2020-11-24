using DevExpress.Xpo;
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
    }
}
