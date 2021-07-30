using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETT_Web.MobileTransactions
{
    public partial class DayTransactionSummaryTable : ServerMasterPage
    {
        Session session;
        IIssueDocumentRepository issueDocumentRepo;
        IMobileTransactionRepository mobileTransactionRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = this.Title;

            session = XpoHelper.GetNewSession();


            ASPxGridViewSumarryTransaction.Settings.GridLines = GridLines.Both;
            issueDocumentRepo = new IssueDocumentRepository(session);
            mobileTransactionRepo = new MobileTransactionRepository(session);

            dtDayOfTranasationOd.Date = DateTime.Now;
            dtDayOfTranasationDo.Date = DateTime.Now;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void ASPxGridViewSumarryTransaction_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "NeedMatching")
            {
                if (Convert.ToBoolean(e.Value))
                    e.DisplayText = "NE";
                else
                    e.DisplayText = "DA";
            }
        }



        protected void ASPxGridViewSumarryTransaction_DataBinding(object sender, EventArgs e)
        {
            List<DayTransaction> obj = (List<DayTransaction>)GetValueFromSession(Enums.MobileTransactionSession.DayilySummaryModel);

            if (obj != null)
            {                
                (sender as ASPxGridView).DataSource = obj;
                (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
                
            }

        }



        protected void btnPotrdi_Click(object sender, EventArgs e)
        {
            Master.NavigationBarMain.DataBind();
        }

        protected void CallbackPanelSumarryTransaction_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "AcceptFilter")
            {
                DateTime dtFrom = dtDayOfTranasationOd.Date;
                DateTime dtTo = dtDayOfTranasationDo.Date;

                if (dtFrom > DateTime.MinValue)
                {
                    List<DayTransaction> list = mobileTransactionRepo.GetDaySummaryTransaction(dtFrom, dtTo, session);
                    AddValueToSession(Enums.MobileTransactionSession.DayilySummaryModel, list);

                    ASPxGridViewSumarryTransaction.DataBind();
                    Master.NavigationBarMain.DataBind();
                }

                
            }
        }

        protected void ASPxGridViewSumarryTransaction_DataBound(object sender, EventArgs e)
        {            
        }
    }
}


   