using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models.Product;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class ProductRepository : IProductRepository
    {
        Session session;

        public ProductRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }

        public Product GetProductByID(int pId, Session currentSession = null)
        {
            try
            {
                XPQuery<Product> product = null;

                if (currentSession != null)
                    product = currentSession.Query<Product>();
                else
                    product = session.Query<Product>();

                return product.Where(p => p.ProductID == pId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_16, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Product GetProductByName(string name, Session currentSession = null)
        {
            try
            {
                XPQuery<Product> product = null;

                if (currentSession != null)
                    product = currentSession.Query<Product>();
                else
                    product = session.Query<Product>();

                return product.Where(p => p.Name.Contains(name)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_16, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveProduct(Product model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.ProductID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.ProductID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_17, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteProduct(int pId)
        {
            try
            {
                GetProductByID(pId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_18, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteProduct(Product model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_18, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        //InventoryStock

        public InventoryStock GetInventroyStockByProductName(string name, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryStock> invStock = null;

                if (currentSession != null)
                    invStock = currentSession.Query<InventoryStock>();
                else
                    invStock = session.Query<InventoryStock>();

                return invStock.Where(invs => invs.ProductID.Name.Contains(name)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public InventoryStock GetInventroyStockByProductID(int productID, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryStock> invStock = null;

                if (currentSession != null)
                    invStock = currentSession.Query<InventoryStock>();
                else
                    invStock = session.Query<InventoryStock>();

                return invStock.Where(invs => invs.ProductID.ProductID == productID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public InventoryStock GetInventroyStockByProductIDAndLocationID(int productID, int locationID, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryStock> invStock = null;

                if (currentSession != null)
                    invStock = currentSession.Query<InventoryStock>();
                else
                    invStock = session.Query<InventoryStock>();

                return invStock.Where(invs => invs.ProductID.ProductID == productID && invs.LocationID.LocationID == locationID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public InventoryStock GetInventoryStockByID(int invId, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryStock> inventoryStock = null;

                if (currentSession != null)
                    inventoryStock = currentSession.Query<InventoryStock>();
                else
                    inventoryStock = session.Query<InventoryStock>();

                return inventoryStock.Where(p => p.InventoryStockID == invId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveInventoryStock(InventoryStock model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.InventoryStockID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.InventoryStockID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_29, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteInventoryStock(int invId)
        {
            try
            {
                GetInventoryStockByID(invId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_30, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteInventoryStock(InventoryStock model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_30, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<ProductAPIModel> GetProductsMobile()
        {
            try
            {
                XPQuery<Product> product = session.Query<Product>();

                var query = from prod in product
                            select new ProductAPIModel
                            {
                                category_id = prod.CategoryID.ToString(),
                                created_at = "",
                                deleted_at = null,
                                description = prod.Notes,
                                id = prod.ProductID.ToString(),
                                manufacturers_code = prod.SupplierCode,
                                metric_id = prod.MeasuringUnitID.ToString(),
                                name = prod.Name,
                                suppliers = prod.SupplierID.ToString(),
                                updated_at = "",
                                user_id = null
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_16, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}