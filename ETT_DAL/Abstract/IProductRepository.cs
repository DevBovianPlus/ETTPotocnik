using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IProductRepository
    {
        Product GetProductByID(int pId, Session currentSession = null);
        int SaveProduct(Product model, int userID = 0);
        bool DeleteProduct(int pId);
        bool DeleteProduct(Product model);
        InventoryStock GetInventroyStockByProductName(string name, Session currentSession = null);
        InventoryStock GetInventroyStockByProductID(int productID, Session currentSession = null);
        Product GetProductByName(string name, Session currentSession = null);
        InventoryStock GetInventoryStockByID(int invId, Session currentSession = null);
        int SaveInventoryStock(InventoryStock model, int userID = 0);
        bool DeleteInventoryStock(int invId);
        bool DeleteInventoryStock(InventoryStock model);
        InventoryStock GetInventroyStockByProductIDAndLocationID(int productID, int locationID, Session currentSession = null);
        List<ProductAPIModel> GetProductsMobile();
    }
}
