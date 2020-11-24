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

namespace ETT_Web.IssueDocuments
{
    public partial class IssueDocumentTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            session = XpoHelper.GetNewSession();
            XpoDSIssueDocument.Session = session;

            ASPxGridViewIssueDocument.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

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
                valueID = ASPxGridViewIssueDocument.GetRowValues(ASPxGridViewIssueDocument.FocusedRowIndex, "IssueDocumentID");

            ClearAllSessions(Enum.GetValues(typeof(Enums.IssueDocumentSession)).Cast<Enums.IssueDocumentSession>().ToList());
            RedirectWithCustomURI("IssueDocumentForm.aspx", (int)action, valueID);
        }

        protected void ASPxGridViewIssueDocument_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewIssueDocument, btnAdd, btnEdit, btnDelete);
        }

        protected void ASPxGridViewIssueDocument_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');

            if (split[0] == "DoubleClick")
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.IssueDocumentSession)).Cast<Enums.IssueDocumentSession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("IssueDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }
    }
}