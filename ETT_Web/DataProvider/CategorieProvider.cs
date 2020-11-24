using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class CategorieProvider : ServerMasterPage
    {
        public void SetCategorieModel(Categorie model)
        {
            AddValueToSession(Enums.CategorieSession.CategorieModel, model);
        }

        public Categorie GetCategorieModel()
        {
            if (SessionHasValue(Enums.CategorieSession.CategorieModel))
                return (Categorie)GetValueFromSession(Enums.CategorieSession.CategorieModel);

            return null;
        }
    }
}