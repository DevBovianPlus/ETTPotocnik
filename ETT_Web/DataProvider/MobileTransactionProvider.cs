using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class MobileTransactionProvider : ServerMasterPage
    {
        public void SetMobileTransactionModel(MobileTransaction model)
        {
            AddValueToSession(Enums.MobileTransactionSession.MobileTransactionModel, model);
        }

        public MobileTransaction GetMobileTransactionModel()
        {
            if (SessionHasValue(Enums.MobileTransactionSession.MobileTransactionModel))
                return (MobileTransaction)GetValueFromSession(Enums.MobileTransactionSession.MobileTransactionModel);

            return null;
        }
    }
}