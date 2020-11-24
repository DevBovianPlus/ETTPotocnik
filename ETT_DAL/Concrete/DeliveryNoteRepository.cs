using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models;
using ETT_DAL.Models.XML;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class DeliveryNoteRepository : IDeliveryNoteRepository
    {
        Session session;

        IMeasuringUnitRepository measureRepo;
        IProductRepository productRepo;
        ILocationRepository locationRepo;
        ICategorieRepository categoryRepo;
        IClientRepository clientRepo;
        IUserRepository userRepo;

        public DeliveryNoteRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;

            measureRepo = new MeasuringUnitRepository(session);
            productRepo = new ProductRepository(session);
            locationRepo = new LocationRepository(session);
            categoryRepo = new CategorieRepository(session);
            clientRepo = new ClientRepository(session);
            userRepo = new UserRepository(session);
        }

        public DeliveryNote GetDeliveryNoteByID(int dnId, Session currentSession = null)
        {
            try
            {
                XPQuery<DeliveryNote> deliveryNote = null;

                if (currentSession != null)
                    deliveryNote = currentSession.Query<DeliveryNote>();
                else
                    deliveryNote = session.Query<DeliveryNote>();

                return deliveryNote.Where(dn => dn.DeliveryNoteID == dnId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveDeliveryNote(DeliveryNote model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.DeliveryNoteID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.DeliveryNoteID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteDeliveryNote(int dnId)
        {
            try
            {
                GetDeliveryNoteByID(dnId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_27, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteDeliveryNote(DeliveryNote model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_27, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        //DeliveryNoteItems

        public DeliveryNoteItem GetDeliveryNoteItemByID(int dniId, Session currentSession = null)
        {
            try
            {
                XPQuery<DeliveryNoteItem> deliveryNoteItem = null;

                if (currentSession != null)
                    deliveryNoteItem = currentSession.Query<DeliveryNoteItem>();
                else
                    deliveryNoteItem = session.Query<DeliveryNoteItem>();

                return deliveryNoteItem.Where(dni => dni.DeliveryNoteItemID == dniId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveDeliveryNoteItem(DeliveryNoteItem model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.DeliveryNoteItemID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.DeliveryNoteItemID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteDeliveryNoteItem(int dniId)
        {
            try
            {
                GetDeliveryNoteItemByID(dniId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_27, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteDeliveryNoteItem(DeliveryNoteItem model)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_27, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveSummaryToDeliveryNoteItem(List<SummaryItemModel> model, int deliveryNoteID, int locationID, int supplierID, List<Item> atomes, int userID = 0)
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {

                    foreach (var obj in model)
                    {
                        DeliveryNoteItem item = new DeliveryNoteItem(uow);
                        item.DeliveryNoteItemID = 0;

                        item.DeliveryNoteID = GetDeliveryNoteByID(deliveryNoteID, uow);
                        item.MeasuringUnitID = measureRepo.GetMeasuringUnitByCode(obj.UnitOfMeasure, uow);

                        item.PSN = obj.PSN;
                        item.SID = obj.SID;

                        item.SupplierProductCode = obj.ProducerProductCode;
                        item.SupplierProductName = obj.ProducerProductName.Substring(0, obj.ProducerProductName.IndexOf(" ") + 1);

                        //poiščemo artikel v šifrantu po imenu če obstaja. Če ne dodamo novega
                        var productItem = productRepo.GetProductByName(item.SupplierProductName, uow);

                        if (productItem != null)
                        {
                            item.ProductID = productItem;
                        }
                        else // produkta še ni v bazi zato ustvarimo novega
                        {
                            Product prod = new Product(session);

                            prod.ProductID = 0;
                            prod.MeasuringUnitID = measureRepo.GetMeasuringUnitByCode(obj.UnitOfMeasure, session);
                            prod.Name = item.SupplierProductName;
                            prod.Notes = "Avtomatsko dodani artikel iz strani sistema ETT!";
                            prod.SupplierCode = item.SupplierProductCode;
                            prod.CategoryID = categoryRepo.GetCategorieDefault(session);
                            prod.SupplierID = clientRepo.GetClientByID(supplierID);

                            int id = productRepo.SaveProduct(prod);

                            item.ProductID = uow.GetObjectByKey<Product>(id);
                        }

                        var inventoryProd = productRepo.GetInventroyStockByProductIDAndLocationID(item.ProductID.ProductID, locationID, uow);
                        var atome = atomes.Where(a => !String.IsNullOrEmpty(a.PackagesSIDs) && a.PackagesSIDs.Contains(obj.SID)).FirstOrDefault();

                        decimal atomeQuantity = 0M;
                        if (atome != null)
                            atomeQuantity = atome.Quantity;

                        int atomeCount = atomes.Count(a => !String.IsNullOrEmpty(a.PackagesSIDs) && a.PackagesSIDs.Contains(obj.SID));

                        if (inventoryProd != null)
                        {
                            inventoryProd.Quantity += (atomeCount * atomeQuantity);
                            inventoryProd.QuantityPcs += atomeCount;
                        }
                        else
                        {
                            InventoryStock inventory = new InventoryStock(session);
                            inventory.InventoryStockID = 0;
                            inventory.LocationID = locationRepo.GetLocationByID(locationID, session);
                            inventory.Notes = "Avtomatsko dodano vodenje zaloge";
                            inventory.ProductID = productRepo.GetProductByID(item.ProductID.ProductID, session);
                            inventory.Quantity = (atomeCount * atomeQuantity);
                            inventory.QuantityPcs = atomeCount;

                            productRepo.SaveInventoryStock(inventory);
                        }

                        //item.ProductID = ? Kako shraniti referenco na šifratn produktov ? Dinamično grajenje šifranta produktov ali??

                        item.Length = obj.Length;
                        item.NEW = obj.NEW;
                        item.ItemQuantity = obj.ItemQuantity;
                        item.CountOfTradeUnits = obj.CountOfTradeUnits;
                        item.PackagingLevel = obj.PackagingLevel;
                        item.ProductionDate = obj.ProductionDate;
                        item.ProductItemCount = obj.ProductItemCount;

                        item.tsUpdate = DateTime.Now;
                        item.tsUpdateUserID = userID;

                        item.tsInsert = DateTime.Now;
                        item.tsInsertUserID = userID;
                    }


                    uow.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<DeliveryNoteItem> GetDeliveryNoteItemsByDeliveryNoteID(int dnId, Session currentSession = null)
        {
            try
            {
                XPQuery<DeliveryNoteItem> deliveryNoteItem = null;

                if (currentSession != null)
                    deliveryNoteItem = currentSession.Query<DeliveryNoteItem>();
                else
                    deliveryNoteItem = session.Query<DeliveryNoteItem>();

                return deliveryNoteItem.Where(dni => dni.DeliveryNoteID.DeliveryNoteID == dnId).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        //InventoryDeliveries

        public void SaveInventoryDeliveries(List<Item> model, int deliveryNoteID, int locationID, int userID = 0)
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    List<DeliveryNoteItem> deliveryNoteItems = GetDeliveryNoteItemsByDeliveryNoteID(deliveryNoteID, uow);

                    foreach (var obj in model)
                    {
                        //v InventoryDeliveries shranimo vsaki atom v svoj zapis, skupaj z hierarhijo in ostalimi podatki
                        InventoryDeliveries item = new InventoryDeliveries(uow);
                        item.InventoryDeliveriesID = 0;

                        string topLevelPackageSID = GetTopLevelSID(obj);
                        var deliveryNoteItem = deliveryNoteItems.Where(dni => !String.IsNullOrEmpty(topLevelPackageSID) && dni.SID == topLevelPackageSID).FirstOrDefault();
                        item.DeliveryNoteItemID = deliveryNoteItem;

                        item.SupplierProductCode = deliveryNoteItem.SupplierProductCode;
                        item.AtomeUID250 = obj.UID;
                        item.PackagesUIDs = obj.PackagesUIDs;
                        item.PackagesSIDs = obj.PackagesSIDs;

                        if (deliveryNoteItem.ProductID != null)//preverimi če artikel obstaja. Če obstaja poiščemo zapis pregled zaloge in ga nastavimo na inventorydeliveries
                        {
                            var inventoryProd = productRepo.GetInventroyStockByProductIDAndLocationID(deliveryNoteItem.ProductID.ProductID, locationID, uow);

                            if (inventoryProd != null)
                            {
                                item.InventoryStockID = inventoryProd;
                            }
                        }
                        //item.Notes = "";

                        item.LastLocationID = locationRepo.GetLocationByID(locationID, uow);

                        item.tsUpdate = DateTime.Now;
                        item.tsUpdateUserID = userID;

                        item.tsInsert = DateTime.Now;
                        item.tsInsertUserID = userID;

                        item.Quantity = obj.Quantity;
                        item.UnitOfMeasureID = measureRepo.GetMeasuringUnitByCode(obj.MeasuringUnitCode, uow);

                        int id = 0;
                        if (obj.Parents != null)
                        {
                            for (int i = obj.Parents.Count - 1; i >= 0; i--)//v seznamu imamo shranjeno hierarhijo paketov od očeta do otroka.
                                                                            //tukaj sezam bere v obratnem vrstem redu (od otroka do očeta, ker v tabeli InventoryDeliveriesPackages shranjujemo hirerahijo
                                                                            //v vrstem redu od otroka do očeta (from bottom to top)).
                            {

                                Item parent = null;
                                //preverimo če obstaja na i+1 mestu starš 
                                if (i + 1 <= obj.Parents.Count - 1)
                                    parent = obj.Parents[i + 1];

                                InventoryDeliveriesPackages idp = new InventoryDeliveriesPackages(session);
                                idp.InventoryDeliveriesPackagesID = 0;
                                idp.ElementUID250 = obj.Parents[i].UID;
                                idp.ParentElementID = parent != null ? GetInventoryDeliveriesPackagesIDByUID(parent.UID) : 0;
                                idp.tsUpdate = DateTime.Now;
                                idp.tsUpdateUserID = userID;

                                idp.tsInsert = DateTime.Now;
                                idp.tsInsertUserID = userID;

                                //če ne obstaja takšen zapis v tabeli ki ima enak uid in parent id potem ga lahko shranimo. (zapisi so se podvajali po zgornji kodi)
                                var invDeliveriesPackage = InventoryDeliveriesPackagesExist(idp.ElementUID250, idp.ParentElementID);
                                if (invDeliveriesPackage == null)
                                {
                                    idp.Save();
                                    id = idp.InventoryDeliveriesPackagesID;
                                }
                                else
                                    id = invDeliveriesPackage.InventoryDeliveriesPackagesID;

                                if (i == 0)
                                {
                                    item.InventoryDeliveriesPackagesID = uow.GetObjectByKey<InventoryDeliveriesPackages>(id);
                                }
                            }
                        }

                        if (item.InventoryStockID != null)
                        {
                            //tukaj shranjujemo aktualne lokacije artiklov oz. atomov iz dobavnice.
                            InventoryDeliveriesLocation invDelLoc = new InventoryDeliveriesLocation(uow);
                            invDelLoc.InventoryDeliveriesID = item;
                            invDelLoc.InventoryDeliveriesLocationID = 0;
                            invDelLoc.LocationToID = locationRepo.GetLocationByID(item.InventoryStockID.LocationID.LocationID, uow);
                            invDelLoc.LocationFromID = null;
                            invDelLoc.UserID = userRepo.GetUserByID(userID, uow);
                            invDelLoc.tsInsert = DateTime.Now;
                            invDelLoc.tsInsertUserID = 0;
                            invDelLoc.tsUpdate = DateTime.Now;
                            invDelLoc.tsUpdateUserID = 0;
                        }
                    }

                    uow.CommitChanges();
                }

                var dn = GetDeliveryNoteByID(deliveryNoteID, session);
                dn.DeliveryNoteStatusID = GetDeliveryNoteStatusByCode(Enums.DeliveryNoteStatus.Completed, session);
                dn.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private string GetTopLevelSID(Item item)
        {
            //Ob sestavljanju stringa packagesIDs smo top level package postavili na prvo mesto stringa - glej DeliveryNoteForm, metoda ConstructAtomeHierarchySID
            return String.IsNullOrEmpty(item.PackagesSIDs) ? "" : item.PackagesSIDs.Split(CommonMethods.PackageDelimiter)[0].Trim();


        }

        public List<PackageItem> GroupByPackagesUIDs(int deliveryNoteItemID, Session currentSession = null)
        {
            try
            {
                XPQuery<InventoryDeliveries> inventoryDeliveries = null;

                if (currentSession != null)
                    inventoryDeliveries = currentSession.Query<InventoryDeliveries>();
                else
                    inventoryDeliveries = session.Query<InventoryDeliveries>();


                var list = inventoryDeliveries.Where(invd => invd.DeliveryNoteItemID.DeliveryNoteItemID == deliveryNoteItemID).ToList();
                List<PackageItem> topLevelTreeList = new List<PackageItem>();
                foreach (var item in list)
                {
                    string[] splitUID = item.PackagesUIDs.Split(',');
                    string[] splitSID = item.PackagesSIDs.Split(',');
                    topLevelTreeList = ConstructTreeOfPackages(splitUID, splitSID, topLevelTreeList);
                }

                return topLevelTreeList;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private List<PackageItem> ConstructTreeOfPackages(string[] splitedPackagesUID, string[] splitedPackagesSID, List<PackageItem> treePackages, int index = 0, PackageItem previousElement = null)
        {
            if (splitedPackagesUID.Length > 0 && index <= splitedPackagesUID.Length - 1)
            {
                PackageItem pItem = new PackageItem();
                pItem.UID = splitedPackagesUID[index].Trim();
                pItem.SID = splitedPackagesSID[index].Trim(); ;

                if (treePackages == null)
                {
                    treePackages = new List<PackageItem>();
                    pItem.Parent = null;
                    pItem.TreeLevel = splitedPackagesUID.Length - 1;// -1 dodamo zato ker se sam produkt (list) ne upošteva kot nivo oz level drevesa
                    treePackages.Add(pItem);
                    return ConstructTreeOfPackages(splitedPackagesUID, splitedPackagesSID, treePackages, ++index, pItem);//++index da se števec takoj poveča
                }

                if (previousElement != null)//previousElement nam pove da je ta element starš trenutno procesiranemu elementu (pItem)
                {
                    PackageItem item = null;
                    if (previousElement.Children == null)
                        previousElement.Children = new List<PackageItem>();
                    else
                        item = previousElement.Children.Where(pi => pi.UID == pItem.UID).FirstOrDefault();

                    if (item == null)//child element še ne obstaja obstaja
                    {
                        pItem.Parent = previousElement;
                        pItem.TreeLevel = (splitedPackagesUID.Length - 1) - index;// -1 dodamo zato ker se sam produkt (list) ne upošteva kot nivo oz level drevesa
                        previousElement.Children.Add(pItem);
                    }

                    return ConstructTreeOfPackages(splitedPackagesUID, splitedPackagesSID, treePackages, ++index, item ?? pItem);
                }
                else//imamo top level element, ker previousElement je null samo takrat kadar se prviš pokliče funkcija
                {
                    var treeItem = treePackages.Where(tlp => tlp.UID == pItem.UID).FirstOrDefault();
                    if (treeItem != null)//Iskani element že obstaja zato kreiramo rekurzijo
                    {
                        return ConstructTreeOfPackages(splitedPackagesUID, splitedPackagesSID, treePackages, ++index, treeItem);
                    }
                    else
                    {
                        pItem.Parent = null;
                        pItem.TreeLevel = splitedPackagesUID.Length - 1;
                        treePackages.Add(pItem);
                        return ConstructTreeOfPackages(splitedPackagesUID, splitedPackagesSID, treePackages, ++index, pItem);
                    }
                }

            }
            else
                return treePackages;
        }

        private int GetInventoryDeliveriesPackagesIDByUID(string uid)
        {
            try
            {
                XPQuery<InventoryDeliveriesPackages> inventoryDeliveriesPackages = null;

                inventoryDeliveriesPackages = session.Query<InventoryDeliveriesPackages>();


                var obj = inventoryDeliveriesPackages.Where(invdp => invdp.ElementUID250 == uid).FirstOrDefault();

                if (obj != null)
                    return obj.InventoryDeliveriesPackagesID;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private InventoryDeliveriesPackages InventoryDeliveriesPackagesExist(string uid, int parentID)
        {
            try
            {
                XPQuery<InventoryDeliveriesPackages> inventoryDeliveriesPackages = null;

                inventoryDeliveriesPackages = session.Query<InventoryDeliveriesPackages>();


                return inventoryDeliveriesPackages.Where(invdp => invdp.ElementUID250 == uid && invdp.ParentElementID == parentID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public DeliveryNoteStatus GetDeliveryNoteStatusByCode(Enums.DeliveryNoteStatus status, Session currentSession = null)
        {
            try
            {
                XPQuery<DeliveryNoteStatus> deliveryNoteStat = null;

                if (currentSession != null)
                    deliveryNoteStat = currentSession.Query<DeliveryNoteStatus>();
                else
                    deliveryNoteStat = session.Query<DeliveryNoteStatus>();

                return deliveryNoteStat.Where(dns => dns.Code == status.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}