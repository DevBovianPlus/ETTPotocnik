using DevExpress.Web;
using ETT_Utilities.Common;
using ETT_Utilities.Helpers;
using ETT_Web.DataProvider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ETT_Web.Infrastructure
{
    public class ServerMasterPage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Request.IsAuthenticated) RedirectHome(1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        #region Session handeling

        protected void HandlePreviousPageSessions()
        {
            string pageName = Path.GetFileName(Request.PhysicalPath);

            if (GetValueFromSession(Enums.CommonSession.PreviousPageName) != null && !GetValueFromSession(Enums.CommonSession.PreviousPageName).ToString().Equals(pageName))
            {
                if (!SessionHasValue(Enums.CommonSession.PreviousPageSessions)) return;

                List<string> sessions = GetValueFromSession(Enums.CommonSession.PreviousPageSessions).ToString().Split(';').ToList();
                foreach (var item in sessions)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        Session.Remove(item);
                    }
                }
                AddValueToSession(Enums.CommonSession.PreviousPageSessions, null);

            }
            AddValueToSession(Enums.CommonSession.PreviousPageName, pageName);
        }
        protected void SetPreviousSessions(string sessionName)
        {
            if (SessionHasValue(Enums.CommonSession.PreviousPageSessions))
            {
                string sessionValues = GetValueFromSession(Enums.CommonSession.PreviousPageSessions).ToString();
                sessionValues += sessionName + ";";
                AddValueToSession(Enums.CommonSession.PreviousPageSessions, sessionValues);
            }
            else
                AddValueToSession(Enums.CommonSession.PreviousPageSessions, sessionName + ";");
        }

        protected void AddValueToSession(object sesionName, object value)
        {
            Session[sesionName.ToString()] = value;
        }

        protected object GetValueFromSession(object sessionName)
        {
            return Session[sessionName.ToString()];
        }

        protected string GetStringValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return "";

            return Session[sessionName.ToString()].ToString();
        }

        protected int GetIntValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return -1;

            return CommonMethods.ParseInt(GetStringValueFromSession(sessionName));
        }

        protected decimal GetDecimalValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return -1;

            return CommonMethods.ParseDecimal(GetStringValueFromSession(sessionName));
        }

        protected double GetDoubleValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return -1.0;

            return CommonMethods.ParseDouble(GetStringValueFromSession(sessionName));
        }

        protected bool GetBoolValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return false;

            return CommonMethods.ParseBool(Session[sessionName.ToString()].ToString());
        }

        protected bool SessionHasValue(object sessionName)
        {
            if (Session[sessionName.ToString()] != null)
                return true;

            return false;
        }

        protected void RemoveAllSesions()
        {
            Session.RemoveAll();
        }

        protected void RemoveSession(object sessionName)
        {
            Session.Remove(sessionName.ToString());
        }

        protected void ClearAllSessions<T>(List<T> sessionList)
        {
            foreach (var item in sessionList)
            {
                RemoveSession(item.ToString());
            }
        }

        protected void ClearAllSessions<T>(List<T> sessionList, string redirectPageUrl, bool isCallback = false)
        {
            foreach (var item in sessionList)
            {
                RemoveSession(item.ToString());
            }

            if (isCallback)
                ASPxWebControl.RedirectOnCallback(redirectPageUrl);
            else
                Response.Redirect(redirectPageUrl);
        }
        #endregion

        #region Generating URI and redirection
        protected void RedirectWithCustomURI(string pageName, int userAction, object recordID)
        {
            Response.Redirect(GenerateURI(pageName, userAction, recordID));
        }

        protected void RedirectWithCustomURI(string pageName, List<QueryStrings> queryList)
        {
            Response.Redirect(GenerateURI(pageName, queryList));
        }

        /// <summary>
        /// Method using for generating uri based on user action(add, edit, delete) and which entity record we want to manipulate.
        /// </summary>
        /// <param name="pageName">Page name.</param>
        /// <param name="userAction">Enums user action (add, edit, delete).</param>
        /// <param name="recordID">Entity record we want to manipulate.</param>
        /// <returns>Returns url for redirection.</returns>
        protected string GenerateURI(string pageName, int userAction, object recordID)
        {
            return pageName + "?" + Enums.QueryStringName.action.ToString() + "=" + userAction.ToString() + "&" + Enums.QueryStringName.recordId.ToString() + "=" + recordID.ToString();
        }

        /// <summary>
        /// Method using for generating uri with custom attributes.
        /// </summary>
        /// <param name="pageName">Page name.</param>
        /// <param name="item">QuerString item which contains atttribute and value.</param>
        /// <returns>Return query string.</returns>
        protected string GenerateURI(string pageName, QueryStrings item)
        {
            string querystring = "";
            if (item != null)
                querystring = GetQueryStringBuilderInstance().AddQueryItem(item);
            return pageName + "?" + querystring;
        }
        /// <summary>
        /// Method using for generating uri with custom multiple attributes.
        /// </summary>
        /// <param name="pageName">Page name.</param>
        /// <param name="item">QuerString list which contains atttribute and value.</param>
        /// <returns>Return query string.</returns>
        protected string GenerateURI(string pageName, List<QueryStrings> queryList)
        {
            string querystring = "";
            if (queryList.Count > 0)
                querystring = GetQueryStringBuilderInstance().AddQueryList(queryList);
            return pageName + "?" + querystring;
        }

        /// <summary>
        /// If the user doesn't have rights for opening page than we redirect user to Home page
        /// </summary>
        protected void RedirectHome()
        {
            bool isAuthenticated = this.Request.IsAuthenticated;
            bool isCallback = CommonMethods.IsCallbackRequest(this.Request);

            // 30.03.2019, Boris, get recordID for email redirect
            if (Request.QueryString["recordId"] != null && !isAuthenticated)
            {
                Int32 iRecordID = 0;
                iRecordID = CommonMethods.ParseInt(Request.QueryString["recordId"].ToString());
                AddValueToSession("HomeRecordID", iRecordID);
            }

            string redirectString = "~/Home.aspx" + (isAuthenticated ? "" : "?sessionExpired=true");

            if (isCallback)
                ASPxWebControl.RedirectOnCallback(redirectString);
            else
                Response.Redirect(redirectString);
        }
        protected void RedirectHome(int messagetype)
        {
            bool isCallback = CommonMethods.IsCallbackRequest(this.Request);
            if (isCallback)
                ASPxWebControl.RedirectOnCallback("~/Home.aspx?messageType=" + messagetype);
            else
                Response.Redirect("~/Home.aspx?messageType=" + messagetype);
        }
        #endregion

        #region Instance Extractor

        protected QueryStringBuilder GetQueryStringBuilderInstance()
        {
            QueryStringBuilder queryStringBuilder = null;

            if (queryStringBuilder == null)
                return new QueryStringBuilder();

            return queryStringBuilder;
        }

        protected MeasuringUnitProvder GetMeasuringUnitProvider()
        {
            MeasuringUnitProvder measuringUnit = null;

            if (measuringUnit == null)
                return new MeasuringUnitProvder();

            return measuringUnit;
        }

        protected ClientProvider GetClientProvider()
        {
            ClientProvider client = null;

            if (client == null)
                return new ClientProvider();

            return client;
        }

        protected LocationProvider GetLocationProvider()
        {
            LocationProvider location = null;

            if (location == null)
                return new LocationProvider();

            return location;
        }

        protected CategorieProvider GetCategorieProvider()
        {
            CategorieProvider categorie = null;

            if (categorie == null)
                return new CategorieProvider();

            return categorie;
        }

        protected ProductProvider GetProductProvider()
        {
            ProductProvider product = null;

            if (product == null)
                return new ProductProvider();

            return product;
        }

        protected EmployeeProvider GetEmployeeProvider()
        {
            EmployeeProvider employee = null;

            if (employee == null)
                return new EmployeeProvider();

            return employee;
        }

        protected DeliveryNoteProvider GetDeliveryNoteProvider()
        {
            DeliveryNoteProvider deliveryNote = null;

            if (deliveryNote == null)
                return new DeliveryNoteProvider();

            return deliveryNote;
        }

        protected InventoryStockProvider GetInventoryStockProvider()
        {
            InventoryStockProvider inventoryStock = null;

            if (inventoryStock == null)
                return new InventoryStockProvider();

            return inventoryStock;
        }

        protected MobileTransactionProvider GetMobileTransactionProvider()
        {
            MobileTransactionProvider mobileTrans = null;

            if (mobileTrans == null)
                return new MobileTransactionProvider();

            return mobileTrans;
        }

        protected InventoryDeliveriesProvider GetInventoryDeliveriesProvider()
        {
            InventoryDeliveriesProvider inventoryDeliveries = null;

            if (inventoryDeliveries == null)
                return new InventoryDeliveriesProvider();

            return inventoryDeliveries;
        }

        protected IssueDocumentProvider GetIssueDocumentProvider()
        {
            IssueDocumentProvider issueDocument = null;

            if (issueDocument == null)
                return new IssueDocumentProvider();

            return issueDocument;
        }
        #endregion

        #region User Action Buttons Handeling
        protected void UserActionConfirmBtnUpdate(ASPxButton button, int userAction, bool popUpBtn = false)
        {
            if (userAction == (int)Enums.UserAction.Delete)
            {
                button.ImageUrl = popUpBtn ? "~/Images/trashPopUp.png" : "~/Images/trash.png";
                button.Image.UrlHottracked = "~/Images/trashHover.png";

                if (popUpBtn)
                    button.Image.Width = Unit.Pixel(24);

                button.Text = "Izbriši";
            }
            else if (userAction == (int)Enums.UserAction.Add)
            {
                button.ImageUrl = popUpBtn ? "~/Images/addPopUp.png" : "~/Images/add.png";
                button.Image.UrlHottracked = "~/Images/addHover.png";

                if (popUpBtn)
                    button.Image.Width = Unit.Pixel(24);

                button.Text = "Shrani";
            }
            else
            {
                button.ImageUrl = popUpBtn ? "~/Images/editPopup.png" : "~/Images/edit.png";
                button.Image.UrlHottracked = "~/Images/editHover.png";

                if (popUpBtn)
                    button.Image.Width = Unit.Pixel(24);

                button.Text = "Shrani";
            }
        }

        protected void EnabledDeleteAndEditBtnPopUp(ASPxButton buttonEdit, ASPxButton buttonDelete, bool disable = true)
        {
            if (disable)
            {
                buttonEdit.ImageUrl = "~/Images/btnPopUpEditDisabled.png";
                buttonEdit.Text = "Spremeni";
                buttonEdit.ClientEnabled = false;

                buttonDelete.ImageUrl = "~/Images/btnPopUpDeleteDisabled.png";
                buttonDelete.Text = "Izbrisi";
                buttonDelete.ClientEnabled = false;
            }
            else
            {
                buttonEdit.ImageUrl = "~/Images/editForPopup.png";
                buttonEdit.Text = "Spremeni";
                buttonEdit.ClientEnabled = true;

                buttonDelete.ImageUrl = "~/Images/trashForPopUp.png";
                buttonDelete.Text = "Izbrisi";
                buttonDelete.ClientEnabled = true;
            }
        }
        protected void EnabledAddBtnPopUp(ASPxButton buttonAdd, bool disable = true)
        {
            if (disable)
            {
                buttonAdd.ImageUrl = "~/Images/addPopupDisabled.png";
                buttonAdd.Text = "Spremeni";
                buttonAdd.ClientEnabled = false;
            }
            else
            {
                buttonAdd.ImageUrl = "~/Images/addPopUp.png";
                buttonAdd.Text = "Spremeni";
                buttonAdd.ClientEnabled = true;
            }
        }
        #endregion

        protected object GetGridLookupValue(ASPxGridLookup lookup)
        {
            try
            {
                return lookup.Value;
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);
                return null;
            }
        }

        protected void ASPxGridLookupLoad_WidthMedium(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).GridView.Width = new Unit(500, UnitType.Pixel);
        }
        protected void ASPxGridLookupLoad_WidthLarge(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).GridView.Width = new Unit(700, UnitType.Pixel);
        }
        protected void ASPxGridLookupLoad_WidthSmall(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).GridView.Width = new Unit(300, UnitType.Pixel);
        }

        protected DataTable SerializeToDataTable<T>(List<T> list, string keyFieldName = "", string visibleColumn = "")
        {
            DataTable dt = new DataTable();
            string json = JsonConvert.SerializeObject(list);
            dt = JsonConvert.DeserializeObject<DataTable>(json);

            if (keyFieldName != "" && visibleColumn != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.NewRow();
                row[keyFieldName] = -1;
                row[visibleColumn] = "Izberi... ";
                dt.Rows.InsertAt(row, 0);
            }

            return dt;
        }

        protected void SetFocusedRowInGridView(ASPxGridView grid, object sessionName = null)
        {
            int index = 0;
            if (sessionName != null)
            {
                if (SessionHasValue(sessionName))
                {
                    index = grid.FindVisibleIndexByKeyValue(GetIntValueFromSession(sessionName));
                    RemoveSession(sessionName);
                }
            }
            grid.Settings.GridLines = GridLines.Both;
            grid.FocusedRowIndex = index;
            grid.ScrollToVisibleIndexOnClient = index;
        }

        public bool SetSessionsAndOpenPopUp(string eventParameter, object sessionToWrite, object entityID)
        {
            int callbackResult = 0;
            int.TryParse(eventParameter, out callbackResult);
            if (callbackResult > 0 && callbackResult <= 3)
            {
                if (callbackResult != (int)Enums.UserAction.Add && entityID == null) return false;

                switch (callbackResult)
                {
                    case (int)Enums.UserAction.Add:
                        AddValueToSession(Enums.CommonSession.UserActionPopUp, callbackResult);
                        AddValueToSession(sessionToWrite, 0);
                        break;

                    default://For editing and deleting is the same code.
                        AddValueToSession(Enums.CommonSession.UserActionPopUp.ToString(), callbackResult);
                        AddValueToSession(sessionToWrite, entityID);
                        break;

                }
                return true;
            }

            return false;
        }

        public void TabsVisible(List<string> tabs, bool show = false, string activeTab = "")
        {
            string json = JsonConvert.SerializeObject(tabs);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CommonJS", String.Format("ConfigureTabs('{0}', '{1}', '{2}');", json, show, activeTab), true);
        }

        public void AllowUserWithRole(params Enums.UserRole[] roles)
        {
            Enums.UserRole role = Enums.UserRole.None;

            Enum.TryParse(PrincipalHelper.GetUserPrincipal().Role, out role);

            bool allow = roles.Contains(role);

            if (!allow)
                RedirectHome();
        }

        public void EnableButtonBasedOnGridRows(ASPxGridView grid, ASPxButton add, ASPxButton edit, ASPxButton delete)
        {
            int rows = grid.VisibleRowCount;

            if (rows <= 0)
            {
                edit.ClientEnabled = false;
                delete.ClientEnabled = false;
            }
        }

        public void ShowWarningPopup(string title, string body)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CommonJS", String.Format("ShowWarningPopup('{0}', '{1}');", title, body), true);
        }

        public string ConcatenateURLForPrint(object valueID, string printReport, bool showPreview)
        {
            List<QueryStrings> list = new List<QueryStrings> {
                new QueryStrings { Attribute = Enums.QueryStringName.printReport.ToString(), Value = printReport },
                new QueryStrings { Attribute = Enums.QueryStringName.showPreviewReport.ToString(), Value = showPreview.ToString() }
            };

            if (valueID != null)
            {
                list.Add(new QueryStrings { Attribute = Enums.QueryStringName.printId.ToString(), Value = valueID.ToString() });
            }

            return GenerateURI("../Reports/ReportPreview.aspx", list);
        }
    }
}