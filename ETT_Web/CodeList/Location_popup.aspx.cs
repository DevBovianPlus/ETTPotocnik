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
    public partial class Location_popup : ServerMasterPage
    {
        Session session;
        int locationID;
        int userAction;
        Location model;
        ILocationRepository locationRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);
            locationID = GetIntValueFromSession(Enums.LocationSession.LocationID);

            session = XpoHelper.GetNewSession();
            locationRepo = new LocationRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = locationRepo.GetLocationByID(locationID);

                    if (model != null)
                    {
                        GetLocationProvider().SetLocationModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.LocationSession.LocationModel))
                    model = GetLocationProvider().GetLocationModel();
            }
        }

        private void FillForm()
        {
            txtName.Text = model.Name;
            chbxIsBuyer.Checked = model.IsBuyer;
            MemoNotes.Text = model.Notes;
            CheckBoxIsWarehouse.Checked = model.IsWarehouse;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Location(session);
                model.LocationID = 0;
            }
            else if( !add && model == null)
            {
                model = GetLocationProvider().GetLocationModel();
            }

            model.Name = txtName.Text;
            model.IsBuyer = chbxIsBuyer.Checked;
            model.Notes = MemoNotes.Text;
            model.IsWarehouse = CheckBoxIsWarehouse.Checked;

            locationRepo.SaveLocation(model, PrincipalHelper.GetUserID());

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
            return locationRepo.DeleteLocation(locationID);
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
            RemoveSession(Enums.LocationSession.LocationID);
            RemoveSession(Enums.LocationSession.LocationModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Location"), true);

        }
    }
}