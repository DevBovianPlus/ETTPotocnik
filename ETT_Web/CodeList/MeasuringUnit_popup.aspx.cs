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
    public partial class MeasuringUnit_popup : ServerMasterPage
    {
        Session session;
        int measuringUnitID;
        int userAction;
        MeasuringUnit model;
        IMeasuringUnitRepository measureRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (SessionHasValue(Enums.CommonSession.UserActionPopUpInPopUp))
                userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUpInPopUp);
            else
                userAction = GetIntValueFromSession(Enums.CommonSession.UserActionPopUp);

            measuringUnitID = GetIntValueFromSession(Enums.MeasuringUnitSession.MeasuringUnitID);

            session = XpoHelper.GetNewSession();
            measureRepo = new MeasuringUnitRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = measureRepo.GetMeasuringUnitByID(measuringUnitID);

                    if (model != null)
                    {
                        GetMeasuringUnitProvider().SetMeasuringUnitModel(model);
                        FillForm();
                    }
                }

                UserActionConfirmBtnUpdate(btnConfirmPopup, userAction, true);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.MeasuringUnitSession.MeasuringUnitModel))
                    model = GetMeasuringUnitProvider().GetMeasuringUnitModel();
            }
        }

        private void FillForm()
        {
            txtName.Text = model.Name;
            txtSymbol.Text = model.Symbol;
            MemoNotes.Text = model.Notes;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new MeasuringUnit(session);
                model.MeasuringUnitID = 0;
            }
            else if( !add && model == null)
            {
                model = GetMeasuringUnitProvider().GetMeasuringUnitModel();
            }

            model.Name = txtName.Text;
            model.Symbol = txtSymbol.Text;
            model.Notes = MemoNotes.Text;

            measureRepo.SaveMeasuringUnit(model, PrincipalHelper.GetUserID());

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
            return measureRepo.DeleteMeasuringUnit(measuringUnitID);
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

            if (SessionHasValue(Enums.CommonSession.UserActionPopUpInPopUp))
                RemoveSession(Enums.CommonSession.UserActionPopUpInPopUp);
            else
                RemoveSession(Enums.CommonSession.UserActionPopUp);

            RemoveSession(Enums.MeasuringUnitSession.MeasuringUnitID);
            RemoveSession(Enums.MeasuringUnitSession.MeasuringUnitModel);

            ClientScript.RegisterStartupScript(GetType(), "CommonJS", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "MeasuringUnit"), true);

        }
    }
}