using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class SettingsRepository : ISettingsRepository
    {
        Session session;

        public SettingsRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }

        public Settings GetSettings()
        {
            try
            {
                XPQuery<Settings> settings = session.Query<Settings>();

                return settings.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_40, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}