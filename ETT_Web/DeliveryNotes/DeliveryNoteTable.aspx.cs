using DevExpress.Web;
using DevExpress.Xpo;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETT_Web.DeliveryNotes
{
    public partial class DeliveryNoteTable : ServerMasterPage
    {
        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            session = XpoHelper.GetNewSession();
            XpoDSDeliveryNote.Session = session;

            ASPxGridViewDeliveryNote.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewDeliveryNote_DataBound(object sender, EventArgs e)
        {
            EnableButtonBasedOnGridRows(ASPxGridViewDeliveryNote, btnAdd, btnEdit, btnDelete);
        }

        protected void ASPxGridViewDeliveryNote_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');

            if (split[0] == "DoubleClick")
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.DeliveryNoteSession)).Cast<Enums.DeliveryNoteSession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("DeliveryNoteForm.aspx", (int)Enums.UserAction.Edit, split[1]));
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
                valueID = ASPxGridViewDeliveryNote.GetRowValues(ASPxGridViewDeliveryNote.FocusedRowIndex, "DeliveryNoteID");

            ClearAllSessions(Enum.GetValues(typeof(Enums.DeliveryNoteSession)).Cast<Enums.DeliveryNoteSession>().ToList());
            RedirectWithCustomURI("DeliveryNoteForm.aspx", (int)action, valueID);
        }

        protected void ASPxGridViewDeliveryNote_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            ColorConverter c = new ColorConverter();

            string code = e.GetValue("DeliveryNoteStatusID.Code").ToString();
            if (code == Enums.DeliveryNoteStatus.Completed.ToString())
                e.Row.BackColor = (Color)c.ConvertFromString("#46e289");
            else if (code == Enums.DeliveryNoteStatus.Error.ToString())
                e.Row.BackColor = Color.Tomato;
            else if (code == Enums.DeliveryNoteStatus.In_Process.ToString())
                e.Row.BackColor = Color.Orange;
        }
    }
}