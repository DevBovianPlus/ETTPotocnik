using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Abstract
{
    public interface ICategorieRepository
    {
        Categorie GetCategorieByID(int cId, Session currentSession = null);
        int SaveCategorie(Categorie model, int userID = 0);
        bool DeleteCategorie(int cId);
        bool DeleteCategorie(Categorie model);
        Categorie GetCategorieDefault(Session currentSession = null);
    }
}