using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class ProductProvider : ServerMasterPage
    {
        public void SetProductModel(Product model)
        {
            AddValueToSession(Enums.ProductSession.ProductModel, model);
        }

        public Product GetProductModel()
        {
            if (SessionHasValue(Enums.ProductSession.ProductModel))
                return (Product)GetValueFromSession(Enums.ProductSession.ProductModel);

            return null;
        }
    }
}