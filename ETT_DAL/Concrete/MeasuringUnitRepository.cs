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
    public class MeasuringUnitRepository : IMeasuringUnitRepository
    {
        Session session;

        public MeasuringUnitRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }

        public MeasuringUnit GetMeasuringUnitByID(int muId, Session currentSession = null)
        {
            try
            {
                XPQuery<MeasuringUnit> measuringUnit = null;

                if (currentSession != null)
                    measuringUnit = currentSession.Query<MeasuringUnit>();
                else
                    measuringUnit = session.Query<MeasuringUnit>();

                return measuringUnit.Where(mu => mu.MeasuringUnitID == muId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_04, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveMeasuringUnit(MeasuringUnit model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.MeasuringUnitID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.MeasuringUnitID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_05, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteMeasuringUnit(int muId)
        {
            try
            {
                GetMeasuringUnitByID(muId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_06, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteMeasuringUnit(MeasuringUnit model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_06, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public MeasuringUnit GetMeasuringUnitByCode(string code, Session currentSession = null)
        {
            try
            {
                XPQuery<MeasuringUnit> measuringUnit = null;

                if (currentSession != null)
                    measuringUnit = currentSession.Query<MeasuringUnit>();
                else
                    measuringUnit = session.Query<MeasuringUnit>();


                var item = measuringUnit.Where(mu => mu.Symbol.ToUpper().Contains(code.ToUpper())).FirstOrDefault();

                if (item != null)
                    return item;
                else
                {
                    //Dodajmo nov zapis v šifran Merskih enot
                    item = new MeasuringUnit(session);
                    item.MeasuringUnitID = 0;
                    item.Name = code.ToLower();
                    item.Notes = "Avtomatsko dodana merska enota v šifrant Merskih enot!";
                    item.Symbol = code;

                    item.tsInsert = DateTime.Now;
                    item.tsInsertUserID = 0;
                    item.tsUpdate = DateTime.Now;
                    item.tsUpdateUserID = 0;

                    item.Save();

                    if (currentSession != null)
                        item = GetMeasuringUnitByID(item.MeasuringUnitID, currentSession);
                }

                return item;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_04, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}