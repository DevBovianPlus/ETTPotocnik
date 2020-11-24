using DevExpress.Web;
using DevExpress.Xpo;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETT_Web.Employees
{
    public partial class EmployeeTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            session = XpoHelper.GetNewSession();
            XpoDSEmployee.Session = session;

            ASPxGridViewEmployee.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewEmployee_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewEmployee, btnAdd, btnEdit, btnDelete);
        }

        protected void ASPxGridViewEmployee_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');

            if (split[0] == "DoubleClick")
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.EmployeeSession)).Cast<Enums.EmployeeSession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("EmployeeForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            RedirectBasedOnUserAction(Enums.UserAction.Edit);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            RedirectBasedOnUserAction(Enums.UserAction.Add);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            RedirectBasedOnUserAction(Enums.UserAction.Delete);
        }

        private void RedirectBasedOnUserAction(Enums.UserAction action)
        {
            object valueID = 0;
            if(action != Enums.UserAction.Add)
                valueID = ASPxGridViewEmployee.GetRowValues(ASPxGridViewEmployee.FocusedRowIndex, "ClientID");

            ClearAllSessions(Enum.GetValues(typeof(Enums.EmployeeSession)).Cast<Enums.EmployeeSession>().ToList());
            RedirectWithCustomURI("EmployeeForm.aspx", (int)action, valueID);
        }
    }
}