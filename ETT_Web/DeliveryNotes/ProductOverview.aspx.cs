using DevExpress.Web;
using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models;
using ETT_DAL.Models.XML;
using ETT_Utilities.Common;
using ETT_Utilities.Helpers;
using ETT_Web.Infrastructure;
using ETT_Web.Widgets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace ETT_Web.DeliveryNotes
{
    public partial class ProductOverview : ServerMasterPage
    {
        Session session;
        int deliveryNoteItemID;
        DeliveryNote model;
        IDeliveryNoteRepository deliveryNoteRepo;
        IClientRepository clientRepo;
        ILocationRepository locationRepo;

        string productTemplateString;
        string childPackageTemplateString;
        string outerPackageTemplateString;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                deliveryNoteItemID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            session = XpoHelper.GetNewSession();

            deliveryNoteRepo = new DeliveryNoteRepository(session);
            clientRepo = new ClientRepository(session);
            locationRepo = new LocationRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<PackageItem> groupedPackages = deliveryNoteRepo.GroupByPackagesUIDs(deliveryNoteItemID);

                PackageTemplateModel pTemplateModel = new PackageTemplateModel();
                ProductTemplateModel prodTemplateModel = new ProductTemplateModel();


                
                string templateInnerPackagePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["INNER_PACKAGE"].ToString()).Replace("\"", "\\");
                string templateProductPath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["PRODUCT"].ToString()).Replace("\"", "\\");

                string templateOuterPackagePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["OUTER_PACKAGE"].ToString()).Replace("\"", "\\");
                //uporabljamo če imamo outer package template kot nastavljeno kot master!
                string templateChildPackagePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["CHILD_PACKAGE"].ToString()).Replace("\"", "\\");

                string templateProductTransactionPath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["TRANSACTION"].ToString()).Replace("\"", "\\");

                StreamReader reader = new StreamReader(templateInnerPackagePath);

                string innerPackageTemplateString = reader.ReadToEnd().Replace("\r\n", "");

                reader = new StreamReader(templateProductPath);
                productTemplateString = reader.ReadToEnd().Replace("\r\n","");

                reader = new StreamReader(templateOuterPackagePath);
                outerPackageTemplateString = reader.ReadToEnd().Replace("\r\n", "");

                reader = new StreamReader(templateChildPackagePath);
                childPackageTemplateString = reader.ReadToEnd().Replace("\r\n", "");

                //TODO: implementiraj še možnosti, kadar je v paketu še paleta ali več.

                string generatedTemplate = "";

                foreach (var item in groupedPackages)
                {
                    pTemplateModel = new PackageTemplateModel();
                    prodTemplateModel = new ProductTemplateModel();
                    pTemplateModel.PackageUID = item.UID;

                    if (item.TreeLevel > 1)//če je tree level večji od 1 potem vemo da obstaja še zunanje pakiranje(paleta ali zunanja škatla ali kaj drugega)
                    {
                        generatedTemplate += ConstructTemplate(item, "", "");
                    }
                    else
                    {
                        foreach (var obj in item.Children)
                        {
                            prodTemplateModel.ProductUID = obj.UID;
                            pTemplateModel.Products += ReplaceDefaultValuesInTemplate(prodTemplateModel, productTemplateString);
                        }
                        generatedTemplate += ReplaceDefaultValuesInTemplate(pTemplateModel, innerPackageTemplateString);
                    }
                }

                
                packagingConatiner.InnerHtml = generatedTemplate;
            }
            else
            {
                 
            }
        }

        private string ConstructTemplate(PackageItem item, string template, string productionTemplate, string outerProdTemplate ="")
        {
            if (item == null) return outerProdTemplate;

            int index = GetNextUnvisitedChildIndex(item);
            string prodTemplate = productionTemplate;

            if (index >= 0)
            {
                return ConstructTemplate(item.Children[index], template, prodTemplate);
            }
            else if (AllChildrenVisited(item))//če so bili obiskani že vsi otroci
            {
                if (item.TreeLevel == 1)
                {
                    prodTemplate += ReplaceDefaultValuesInTemplate(new PackageTemplateModel() { NumberOfProducts = item.Children.Count.ToString(), PackageSID = item.SID, PackageUID = item.UID, Products = template }, childPackageTemplateString);
                    template = "";
                }
                else if (item.TreeLevel == 2)
                {
                    outerProdTemplate += ReplaceDefaultValuesInTemplate(new OuterPackageTemplateModel() { ChildPackages = prodTemplate, PackageUID = item.UID }, outerPackageTemplateString);
                    prodTemplate = "";
                }
                else if (item.TreeLevel > 2)
                { }

                item.Visited = true;
                return ConstructTemplate(item.Parent, template, prodTemplate, outerProdTemplate);
            }
            else if (item.Parent != null) // prišli smo do  zadnjega neobiskanega vozlišča - sprehajamo se po drevesu vse dokler ne pridemo do korena drevesa.
            {
                //Nastavimo šablono

                if (IsLeaf(item))
                    template += ReplaceDefaultValuesInTemplate(new ProductTemplateModel() { ProductUID = item.UID, ProductSID = item.SID }, productTemplateString);
                else if (item.TreeLevel == 1)
                {
                    prodTemplate += ReplaceDefaultValuesInTemplate(new PackageTemplateModel() { NumberOfProducts = item.Children.Count.ToString(), PackageSID = item.SID, PackageUID = item.UID, Products = template }, childPackageTemplateString);
                    template = "";
                }
                else if (item.TreeLevel == 2)
                {

                    outerProdTemplate += ReplaceDefaultValuesInTemplate(new OuterPackageTemplateModel() { ChildPackages = prodTemplate, PackageUID = item.UID }, outerPackageTemplateString);
                    prodTemplate = "";


                }
                else if (item.TreeLevel > 2)
                { }

                item.Visited = true;
                return ConstructTemplate(item.Parent, template, prodTemplate, outerProdTemplate);
            }
            else
            {
                return outerProdTemplate;
            }
        }

        private int GetNextUnvisitedChildIndex(PackageItem item)
        {
            if (item.Children != null && item.Children.Count > 0)
            {
                for (int i = 0; i < item.Children.Count; i++)
                {
                    if (!item.Children[i].Visited)//če je otrok še neobiskan vrnemo index otroka
                        return i;
                }
            }

            return -1;
        }

        private bool AllChildrenVisited(PackageItem item)
        {
            if (item.Children != null && item.Children.Count > 0)
                return item.Children.Where(child => !child.Visited).FirstOrDefault() != null ? false : true;//preverimo če so vsi otroci v seznamu že obiskani.
            else if (item.Children == null)
                return false;//vrnemo false zaradi tega, da lahko list drevesa pregledamo

            return true;
        }

        public bool IsLeaf(PackageItem item) {
            if (item.Children != null && item.Children.Count > 0)
                return false;

            return true;
        }

        private void FillDefaultValues()
        {
            
        }

        private void FillForm()
        {
            HtmlGenericControl control = (HtmlGenericControl)productOverviewItem.FindControl("productOverviewBadge");
            control.InnerText = model.DeliveryNoteItems.Count.ToString();
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }


        #region Common

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool exitFormPage = true)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = deliveryNoteItemID.ToString() }
            };

            if (isIDDeleted)
                redirectString = "DeliveryNoteTable.aspx";
            else if (!exitFormPage)
            {
                queryStrings.Insert(0, new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                redirectString = GenerateURI("DeliveryNoteForm.aspx", queryStrings);
            }
            else
                redirectString = GenerateURI("DeliveryNoteTable.aspx", queryStrings);

            List<Enums.EmployeeSession> list = Enum.GetValues(typeof(Enums.EmployeeSession)).Cast<Enums.EmployeeSession>().ToList();
            ClearAllSessions(list, redirectString);
        }

        #endregion

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
           
        }

        private string ReplaceDefaultValuesInTemplate(Object o, string template)
        {
            string result = "";
            string value = template;
            Type type = o.GetType();
            object[] indexArgs = { 0 };

            PropertyInfo[] myFields = type.GetProperties(BindingFlags.Public
                | BindingFlags.Instance);

            for (int i = 0; i < myFields.Length; i++)
            {
                try
                {
                    string sRepVal = myFields[i].GetValue(o, null) == null ? "" : myFields[i].GetValue(o, null).ToString();
                    sRepVal = sRepVal.Replace("\n", "<br>");
                    value = value.Replace("$%" + myFields[i].Name + "%$", sRepVal);
                }
                catch (Exception ex)
                {
                    CommonMethods.LogThis(ex.Message);
                }
            }

            result = value;
            return result;
        }
    }
}