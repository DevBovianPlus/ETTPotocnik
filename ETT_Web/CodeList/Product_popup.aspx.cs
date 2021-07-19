using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETT_Web.CodeList
{
    public partial class Product_popup : ServerMasterPage
    {
        Session session;
        int productID;
        int userAction;
        Product model;
        IProductRepository productRepo;
        IClientRepository clientRepo;
        ICategorieRepository categoryRepo;
        IMeasuringUnitRepository measuringUnitRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);
            productID = GetIntValueFromSession(Enums.ProductSession.ProductID);

            session = XpoHelper.GetNewSession();

            productRepo = new ProductRepository(session);
            clientRepo = new ClientRepository(session);
            categoryRepo = new CategorieRepository(session);
            measuringUnitRepo = new MeasuringUnitRepository(session);

            XpoDSSupplier.Session = session;
            XpoDSCategory.Session = session;
            XpoDSMeasuringUnit.Session = session;

            GridLookupSupplier.GridView.Settings.GridLines = GridLines.Both;
            GridLookupCategory.GridView.Settings.GridLines = GridLines.Both;
            GridLookupMeasuringUnit.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = productRepo.GetProductByID(productID);


                    if (model != null)
                    {
                        GetProductProvider().SetProductModel(model);
                        FillForm();
                    }
                }

                decimal dFact = (model.Factor == 0 ? 1 : model.Factor);
                txtFaktor.Text = dFact.ToString();
                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.ProductSession.ProductModel))
                    model = GetProductProvider().GetProductModel();
            }
        }

        private void FillForm()
        {
            txtName.Text = model.Name;
            GridLookupSupplier.Value = model.SupplierID != null ? model.SupplierID.ClientID : 0;
            txtSupplierCode.Text = model.SupplierCode;
            txtPSN.Text = model.PSN;
            GridLookupCategory.Value = model.CategoryID != null ? model.CategoryID.CategorieID : 0;
            GridLookupMeasuringUnit.Value = model.MeasuringUnitID != null ? model.MeasuringUnitID.MeasuringUnitID : 0;
            MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Product(session);
                model.ProductID = 0;
            }
            else if (!add && model == null)
            {
                model = GetProductProvider().GetProductModel();
            }

            model.Name = txtName.Text;

            int supplierID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupSupplier));
            if (model.SupplierID != null)
                model.SupplierID = clientRepo.GetClientByID(supplierID, model.SupplierID.Session);
            else
                model.SupplierID = clientRepo.GetClientByID(supplierID);

            model.SupplierCode = txtSupplierCode.Text;
            model.PSN = txtPSN.Text;
            model.Factor = CommonMethods.ParseDecimal(txtFaktor.Text);

            int categoryID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupCategory));
            if (model.CategoryID != null)
                model.CategoryID = categoryRepo.GetCategorieByID(categoryID, model.CategoryID.Session);
            else
                model.CategoryID = categoryRepo.GetCategorieByID(categoryID);

            int measuringUnitID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupMeasuringUnit));
            if (model.MeasuringUnitID != null)
                model.MeasuringUnitID = measuringUnitRepo.GetMeasuringUnitByID(measuringUnitID, model.MeasuringUnitID.Session);
            else
                model.MeasuringUnitID = measuringUnitRepo.GetMeasuringUnitByID(measuringUnitID);

            model.Notes = MemoNotes.Text;

            productRepo.SaveProduct(model, PrincipalHelper.GetUserID());

            return true;
        }

        private void ProcessUserAction()
        {
            bool isValid = false;
            bool confirm = false;

            switch (userAction)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    confirm = true;
                    break;
            }

            if (isValid)
            {
                RemoveSessionsAndClosePopUP(confirm);
            }
            else
                ShowWarningPopup("Opozorilo", "Prišlo je do napake. Kontaktirajete administratorja!");
        }

        private bool DeleteObject()
        {
            return productRepo.DeleteProduct(productID);
        }

        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void btnConfirmPopup_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.ProductSession.ProductID);
            RemoveSession(Enums.ProductSession.ProductModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Product"), true);

        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            AddValueToSession(Enums.CommonSession.UserActionPopUpInPopUp, (int)Enums.UserAction.Add);
            if (e.Parameter == "CreateSupplier")
            {
                AddValueToSession(Enums.SupplierSession.ClientID, 0);
                PopupControlSupplier.ShowOnPageLoad = true;
            }
            else if (e.Parameter == "CreateCategory")
            {
                AddValueToSession(Enums.CategorieSession.CategorieID, 0);
                PopupControlCategorie.ShowOnPageLoad = true;
            }
            else if (e.Parameter == "CreateMesuringUnit")
            {
                AddValueToSession(Enums.MeasuringUnitSession.MeasuringUnitID, 0);
                PopupControlMeasuringUnit.ShowOnPageLoad = true;
            }
        }

        protected void PopupControlSupplier_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
            RemoveSession(Enums.SupplierSession.ClientID);
            RemoveSession(Enums.SupplierSession.ClientModel);
        }

        protected void PopupControlCategorie_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
            RemoveSession(Enums.CategorieSession.CategorieID);
            RemoveSession(Enums.CategorieSession.CategorieModel);
        }

        protected void PopupControlMeasuringUnit_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
            RemoveSession(Enums.MeasuringUnitSession.MeasuringUnitID);
            RemoveSession(Enums.MeasuringUnitSession.MeasuringUnitModel);
        }
    }
}