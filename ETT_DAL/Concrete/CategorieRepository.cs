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
    public class CategorieRepository : ICategorieRepository
    {
        Session session;

        public CategorieRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }

        public Categorie GetCategorieByID(int cId, Session currentSession = null)
        {
            try
            {
                XPQuery<Categorie> categorie = null;

                if (currentSession != null)
                    categorie = currentSession.Query<Categorie>();
                else
                    categorie = session.Query<Categorie>();

                return categorie.Where(c => c.CategorieID == cId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveCategorie(Categorie model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.CategorieID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.CategorieID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_14, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteCategorie(int cId)
        {
            try
            {
                GetCategorieByID(cId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_15, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteCategorie(Categorie model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_15, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Categorie GetCategorieDefault(Session currentSession = null)
        {
            try
            {
                XPQuery<Categorie> categorie= null;

                if (currentSession != null)
                    categorie = currentSession.Query<Categorie>();
                else
                    categorie = session.Query<Categorie>();

                var obj = categorie.Where(c=> c.Name.Contains("Neznana")).FirstOrDefault();

                if (obj == null)
                {
                    obj = new Categorie(session);
                    obj.Name = "Neznana kategorija";
                    obj.Code = "NEZNANO";
                    obj.Notes = "Avtomatsko generirana privzeta kategorija iz strani ETT aplikacije!";
                    obj.tsInsert = DateTime.Now;
                    obj.tsInsertUserID = 0;
                    obj.tsUpdate = DateTime.Now;
                    obj.tsUpdateUserID = 0;
                    obj.Save();
                }

                return obj;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}