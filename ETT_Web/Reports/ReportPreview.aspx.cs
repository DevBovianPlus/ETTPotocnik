﻿using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.UI;
using ETT_Web.Infrastructure;
using ETT_Utilities.Common;

namespace ETT_Web.Report
{
    public partial class ReportPreview : ServerMasterPage
    {
        string printReport = "";
        int printID = -1;
        bool showPreview = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;
            this.Master.DisableNavBar = true;

            printReport = CommonMethods.Trim(Request.QueryString[Enums.QueryStringName.printReport.ToString()].ToString());
            printID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.printId.ToString()] != null ? Request.QueryString[Enums.QueryStringName.printId.ToString()].ToString() : "-1");
            showPreview = CommonMethods.ParseBool(Request.QueryString[Enums.QueryStringName.showPreviewReport.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Master.StyleDisplayHeader = "none";
            ShowReport();
        }

        private void ShowReport()
        {
            switch (printReport)
            {
                case "IssueDocument":
                    IssueDocumentReport report = new IssueDocumentReport(String.Format("{0} {1}", PrincipalHelper.GetUserPrincipal().FirstName, PrincipalHelper.GetUserPrincipal().LastName));
                    report.Parameters["IssueDocumentID_param"].Value = printID;
                    SetReportPreview(showPreview, report, false);
                    
                    break;
            }
        }

        private void SetReportPreview(bool preview, XtraReport report, bool createDocument = true)
        {
            if (createDocument)
                report.CreateDocument();

            //report.ExportToPdf("");//Shranjevanje poročila na disk

            if (preview)
                WebDocumentViewer.OpenReport(report);
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PdfExportOptions opts = new PdfExportOptions();
                    opts.ShowPrintDialogOnOpen = true;
                    report.ExportToPdf(ms, opts);

                    WriteDocumentToResponse(ms.ToArray(), "pdf", true, "Report_" + DateTime.Now.ToString("dd_MM_YYYY-HH_mm_ss_") + DateTime.Now.TimeOfDay.TotalMilliseconds.ToString());
                }
            }
        }

        private void WriteDocumentToResponse(byte[] documentData, string format, bool isInline, string fileName)
        {
            string contentType = "application/pdf";
            string disposition = (isInline) ? "inline" : "attachment";

            if (format == "png")
                contentType = "image/png";
            else if (format == "jpeg" || format == "jpg")
                contentType = "image/JPEG";

            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
            Response.BinaryWrite(documentData);
            Response.End();
        }
    }
}