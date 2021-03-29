using DevExpress.Web;
using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models.XML;
using ETT_Utilities.Common;
using ETT_Utilities.Helpers;
using ETT_Web.Infrastructure;
using ETT_Web.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace ETT_Web.DeliveryNotes
{
    public partial class DeliveryNoteForm : ServerMasterPage
    {
        Session session;
        int userAction;
        int deliveryNoteID;
        DeliveryNote model;
        IDeliveryNoteRepository deliveryNoteRepo;
        IClientRepository clientRepo;
        ILocationRepository locationRepo;
        string processError = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                userAction = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                deliveryNoteID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            session = XpoHelper.GetNewSession();

            XpoDSSupplier.Session = session;
            XpoDSLocation.Session = session;
            XpoDSDeliveryNoteItem.Session = session;

            deliveryNoteRepo = new DeliveryNoteRepository(session);
            clientRepo = new ClientRepository(session);
            locationRepo = new LocationRepository(session);

            ASPxGridViewDeliveryNoteItem.Settings.GridLines = GridLines.Both;

            GridLookupLocation.GridView.CustomColumnDisplayText += GridView_CustomColumnDisplayText;
            GridLookupLocation.GridView.Settings.GridLines = GridLines.Both;
            GridLookupSupplier.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userAction != (int)Enums.UserAction.Add)
                {
                    model = deliveryNoteRepo.GetDeliveryNoteByID(deliveryNoteID);

                    if (model != null)
                    {
                        GetDeliveryNoteProvider().SetDeliveryNoteModel(model);
                        FillForm();
                    }
                }
                else
                    deliveryNoteItem.Attributes["class"] = "disabled";

                FillDefaultValues();
                UserActionConfirmBtnUpdate(btnSaveChanges, userAction);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.DeliveryNoteSession.DeliveryNoteModel))
                    model = GetDeliveryNoteProvider().GetDeliveryNoteModel();
            }
        }

        private void FillDefaultValues()
        {
            if (userAction == (int)Enums.UserAction.Add)
            {
                DateEditDeliveryNoteDate.Date = DateTime.Now;
                GetDeliveryNoteProvider().SetDeliveryNoteStatus(Enums.DeliveryNoteStatus.Not_Processed);
            }

            if (SessionHasValue(Enums.CommonSession.DownloadDocument))
            {
                DocumentEntity obj = (DocumentEntity)GetValueFromSession(Enums.CommonSession.DownloadDocument);

                byte[] byteFile = File.ReadAllBytes(Server.MapPath(obj.Url));
                string resultExtension = Path.GetExtension(obj.Name);
                string format = "pdf";
                if (resultExtension.Equals(".jpg"))
                    format = "jpg";
                else if (resultExtension.Equals(".jpeg"))
                    format = "jpeg";
                else if (resultExtension.Equals(".png"))
                    format = "png";
                else if (resultExtension.Equals(".xlsx"))
                    format = "xlsx";
                else if (resultExtension.Equals(".xls"))
                    format = "xls";

                RemoveSession(Enums.CommonSession.DownloadDocument);
                WriteDocumentToResponse(byteFile, format, false, obj.Name);
            }
        }

        private void FillForm()
        {
            DateEditDeliveryNoteDate.Date = model.DeliveryNoteDate;
            txtDeliveryNoteNumber.Text = model.DeliveryNoteNumber;
            GridLookupSupplier.Value = model.SupplierID != null ? model.SupplierID.ClientID : 0;
            GridLookupLocation.Value = model.LocationID != null ? model.LocationID.LocationID : 0;
            MemoNotes.Text = model.Notes;
            memError.Text = model.ProcessError;

            // if(!String.IsNullOrEmpty(model.Picture))
            //UploadProfile.ProfileImage.Src = model.Picture.Replace(AppDomain.CurrentDomain.BaseDirectory, "/");

            Enums.DeliveryNoteStatus dnStatus = GetDeliveryNoteProvider().GetDeliveryNoteStatus();
            Enum.TryParse(model.DeliveryNoteStatusID.Code, out dnStatus);

            GetDeliveryNoteProvider().SetDeliveryNoteStatus(dnStatus);

            HtmlGenericControl control = (HtmlGenericControl)deliveryNoteItem.FindControl("deliveryNoteProductBadge");
            control.InnerText = model.DeliveryNoteItems.Count.ToString();
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new DeliveryNote(session);
                model.DeliveryNoteID = 0;
            }
            else if (!add && model == null)
            {
                model = GetDeliveryNoteProvider().GetDeliveryNoteModel();
            }

            model.DeliveryNoteDate = DateEditDeliveryNoteDate.Date;
            model.DeliveryNoteNumber = txtDeliveryNoteNumber.Text;
            model.RecivedMaterialDate = DateEditDeliveryNoteDate.Date;

            int supplierID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupSupplier));
            if (model.SupplierID != null)
                model.SupplierID = clientRepo.GetClientByID(supplierID, model.SupplierID.Session);
            else
                model.SupplierID = clientRepo.GetClientByID(supplierID);

            int locationID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupLocation));
            if (model.LocationID != null)
                model.LocationID = locationRepo.GetLocationByID(locationID, model.LocationID.Session);
            else
                model.LocationID = locationRepo.GetLocationByID(locationID);

            model.Notes = MemoNotes.Text;

            /*if (!String.IsNullOrEmpty(model.Picture))
            {
                //UploadProfile.ProfileImage.Src = model.Picture.Replace(AppDomain.CurrentDomain.BaseDirectory, "\\");
            }*/

            if (GetDeliveryNoteProvider().GetDeliveryNoteStatus() == Enums.DeliveryNoteStatus.Error && !String.IsNullOrEmpty(processError))
                model.ProcessError = processError;

            model.DeliveryNoteStatusID = deliveryNoteRepo.GetDeliveryNoteStatusByCode(GetDeliveryNoteProvider().GetDeliveryNoteStatus(), model.Session);

            deliveryNoteID = deliveryNoteRepo.SaveDeliveryNote(model, PrincipalHelper.GetUserID());

            return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ProcessUserAction(false);
        }

        private bool DeleteObject()
        {
            return deliveryNoteRepo.DeleteDeliveryNote(deliveryNoteID);
        }

        #region Common

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool exitFormPage = true, bool showProcessingNotification = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = deliveryNoteID.ToString() }
            };

            if (isIDDeleted)
                redirectString = "DeliveryNoteTable.aspx";
            else if (!exitFormPage)
            {
                queryStrings.Insert(0, new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                redirectString = GenerateURI("DeliveryNoteForm.aspx", queryStrings);
            }
            else if (exitFormPage && showProcessingNotification)
            {
                queryStrings.Insert(0, new QueryStrings() { Attribute = Enums.QueryStringName.notifyProcessing.ToString(), Value = "true" });
                redirectString = GenerateURI("DeliveryNoteTable.aspx", queryStrings);
            }
            else
                redirectString = GenerateURI("DeliveryNoteTable.aspx", queryStrings);

            List<Enums.DeliveryNoteSession> list = Enum.GetValues(typeof(Enums.DeliveryNoteSession)).Cast<Enums.DeliveryNoteSession>().ToList();
            ClearAllSessions(list, redirectString);
        }

        private void ProcessUserAction(bool exitFormPage = true)
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (userAction)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing, exitFormPage);
            }
        }
        #endregion

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewDeliveryNoteItem.DataBind();
            }
            else
            {
                object user = ASPxGridViewDeliveryNoteItem.GetRowValues(ASPxGridViewDeliveryNoteItem.FocusedRowIndex, "DeliveryNoteItemID");
                bool openPopup = SetSessionsAndOpenPopUp(e.Parameter, Enums.EmployeeSession.UserID, user);
                AddValueToSession(Enums.EmployeeSession.EmployeeID, deliveryNoteID);
                if (openPopup)
                    PopupControlUsers.ShowOnPageLoad = true;
            }
        }

        protected void UploadProfile_ImageUpdated(object sender, EventArgs e)
        {
            if (model == null && userAction == (int)Enums.UserAction.Add)
                model = new DeliveryNote(session);
            else if (model == null)
                model = GetDeliveryNoteProvider().GetDeliveryNoteModel();

            //model.Picture = (sender as ImageUploadWidget).ImageFullFileName;
            GetDeliveryNoteProvider().SetDeliveryNoteModel(model);
        }


        protected void PopupControlUsers_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.EmployeeSession.EmployeeID);
            RemoveSession(Enums.EmployeeSession.UserID);
            RemoveSession(Enums.EmployeeSession.UserModel);
        }

        #region Attachments

        protected void DocumentUpload_PopulateAttachments(object sender, EventArgs e)
        {
            if (model != null && !String.IsNullOrEmpty(model.XMLFilePath))
            {
                List<DocumentEntity> list = new List<DocumentEntity>();
                DocumentEntity document = null;
                string[] split = model.XMLFilePath.Split('|');
                string resultExtension = "";
                foreach (var item in split)
                {
                    string[] fileData = item.Split(';');
                    document = new DocumentEntity();
                    document.Url = fileData[0];
                    document.Name = fileData[1];

                    resultExtension = Path.GetExtension(fileData[1]);
                    if (resultExtension.Equals(".png") || resultExtension.Equals(".jpg") || resultExtension.Equals(".jpeg"))
                        document.isImage = true;
                    else if (resultExtension.ToLowerInvariant().Equals(".xml"))
                        document.isXML = true;

                    list.Add(document);
                }
                (sender as UploadAttachment).files = list;
                //HtmlGenericControl control = (HtmlGenericControl)attachmentsItem.FindControl("attachmentsBadge");
                //control.InnerText = list.Count.ToString();
            }
            (sender as UploadAttachment).ActiveDropZoneID = "active-drop-zone";
        }

        protected void DocumentUpload_UploadComplete(object sender, EventArgs e)
        {
            model = GetDeliveryNoteProvider().GetDeliveryNoteModel();
            if (model != null)
            {
                string pipe = "";
                if (!String.IsNullOrEmpty(model.XMLFilePath))
                    pipe = "|";

                deliveryNoteRepo = new DeliveryNoteRepository(model.Session);
                model.XMLFilePath += pipe + (sender as UploadAttachment).currentFile.Url + ";" + (sender as UploadAttachment).currentFile.Name;
                deliveryNoteRepo.SaveDeliveryNote(model, PrincipalHelper.GetUserID());
                GetDeliveryNoteProvider().SetDeliveryNoteModel(model);

                //TODO: parse XML


                //HtmlGenericControl control = (HtmlGenericControl)attachmentsItem.FindControl("attachmentsBadge");
                //control.InnerText = model.Priloge.Split('|').Length.ToString();
            }
        }

        protected void DocumentUpload_DeleteAttachments(object sender, EventArgs e)
        {
            model = GetDeliveryNoteProvider().GetDeliveryNoteModel();
            if (model != null)
            {
                int hasPipe = 0;
                string fileToDelete = (sender as UploadAttachment).currentFile.Name;
                DocumentEntity obj = GetAttachmentFromDB(fileToDelete);

                if (obj != null)
                {
                    string item = obj.Url + ";" + obj.Name;
                    string strPhysicalFolder = Server.MapPath(obj.Url);
                    if (File.Exists(strPhysicalFolder))
                        File.Delete(strPhysicalFolder);

                    if (model.XMLFilePath.Contains("|"))
                        hasPipe = 1;
                    else
                        hasPipe = 0;

                    model.XMLFilePath = model.XMLFilePath.Remove(model.XMLFilePath.IndexOf(item) - hasPipe, item.Length + hasPipe);
                    deliveryNoteRepo = new DeliveryNoteRepository(model.Session);
                    deliveryNoteRepo.SaveDeliveryNote(model, PrincipalHelper.GetUserID());
                }

            }
        }

        protected void DocumentUpload_DownloadAttachments(object sender, EventArgs e)
        {
            model = GetDeliveryNoteProvider().GetDeliveryNoteModel();
            if (model != null)
            {
                string fileName = (sender as UploadAttachment).currentFile.Name;
                DocumentEntity obj = GetAttachmentFromDB(fileName);

                AddValueToSession(Enums.CommonSession.DownloadDocument, obj);
                //Response.Redirect(Request.RawUrl);
                ASPxWebControl.RedirectOnCallback(Request.RawUrl);
            }
        }

        private DocumentEntity GetAttachmentFromDB(string fileName)
        {
            model = GetDeliveryNoteProvider().GetDeliveryNoteModel();
            if (model != null)
            {
                string[] split = model.XMLFilePath.Split('|');
                foreach (var item in split)
                {
                    string[] fileSplit = item.Split(';');
                    if (fileSplit[1].Equals(fileName))
                    {
                        return new DocumentEntity { Url = fileSplit[0], Name = fileSplit[1] };
                    }
                }
            }
            return null;
        }

        private void WriteDocumentToResponse(byte[] documentData, string format, bool isInline, string fileName)
        {
            string contentType = "application/pdf";

            if (format == "png")
                contentType = "image/png";
            else if (format == "jpg" || format == "jpeg")
                contentType = "image/jpeg";

            string disposition = (isInline) ? "inline" : "attachment";

            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
            Response.BinaryWrite(documentData);
            Response.End();
            //Response.Flush(); // Sends all currently buffered output to the client.
            //Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }
        #endregion

        private void GridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "IsBuyer") return;

            if (Convert.ToBoolean(e.Value))
                e.DisplayText = "DA";
            else
                e.DisplayText = "NE";
        }

        protected void btnProcessXMLFile_Click(object sender, EventArgs e)
        {
            if (model == null) model = GetDeliveryNoteProvider().GetDeliveryNoteModel();

            if (model != null && GetDeliveryNoteProvider().GetDeliveryNoteStatus() == Enums.DeliveryNoteStatus.Not_Processed)
            {
                string[] split = model.XMLFilePath.Split('|');
                DocumentEntity document = null;
                foreach (var item in split)
                {
                    string[] fileData = item.Split(';');
                    document = new DocumentEntity();
                    document.Url = fileData[0];
                    document.Name = fileData[1];
                }
                string physicalPath = Server.MapPath(document.Url);

                //TODO: Shrani DeliveryNote in nastavi na status In_Process
                GetDeliveryNoteProvider().SetDeliveryNoteStatus(Enums.DeliveryNoteStatus.In_Process);
                AddOrEditEntityObject((userAction == (int)Enums.UserAction.Add ? true : false));
                int userId = PrincipalHelper.GetUserID();
                Task.Run(() =>
                {
                    ParseXML(physicalPath, userId);
                    //TODO obvestiti uproabnika o daljšem obdobju parsanja...glej status.
                });

            }
            this.Master.NavigationBarMain.DataBind();
            ClearSessionsAndRedirect(false, true, true);
        }

        private void ParseXML(string xmlPath, int userID)
        {
            try
            {
                XDocument doc = XDocument.Load(new StreamReader(xmlPath, Encoding.UTF8));
                int locationID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupLocation));
                int supplierID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupSupplier));
                var rootName = doc.Root.Name;
                List<SummaryItemModel> topLevelList = new List<SummaryItemModel>();
                List<Item> atomes = new List<Item>();
                bool isRepacking = false;

                if (rootName == Enums.XMLTagName.Posiljka.ToString())
                {
                    var products = doc.Root.Element(Enums.XMLTagName.Vsebina.ToString()).Descendants(Enums.XMLTagName.Ident.ToString()).ToList();
                    topLevelList = FindTopLevelSummaryItems(products);
                    atomes = SetHierarchyOfAtomes(products, topLevelList);
                }
                else
                {
                    var obj = doc.Root.Element(Enums.XMLTagName.SummaryItems.ToString());
                    List<SummaryItemModel> list = GetSummaryItems(obj);
                    var repacking = list.Where(si => si.ProducerProductName == "Repacking").FirstOrDefault();
                    isRepacking = repacking == null ? false : true;

                    topLevelList = FindTopLevelSummaryItems(list, doc.Root.Element(Enums.XMLTagName.Units.ToString()), isRepacking);
                    atomes = SetHierarchyOfAtomes(doc.Root.Element(Enums.XMLTagName.Units.ToString()), list);
                }

                deliveryNoteRepo.SaveSummaryToDeliveryNoteItem(topLevelList, deliveryNoteID, locationID, supplierID, atomes, userID);

                deliveryNoteRepo.SaveInventoryDeliveries(atomes, deliveryNoteID, locationID, userID, isRepacking);
                //GetDeliveryNoteProvider().SetDeliveryNoteStatus(Enums.DeliveryNoteStatus.Completed);
                ASPxGridViewDeliveryNoteItem.DataBind();
            }
            catch (Exception ex)
            {
                processError = ex.Message;
                GetDeliveryNoteProvider().SetDeliveryNoteStatus(Enums.DeliveryNoteStatus.Error);
                AddOrEditEntityObject((userAction == (int)Enums.UserAction.Add ? true : false));
                throw new Exception(ex.Message, ex);
            }
        }

        private List<SummaryItemModel> GetSummaryItems(XElement summaryItemsTemplate)
        {
            List<SummaryItemModel> list = summaryItemsTemplate.Elements(Enums.XMLTagName.SummaryItem.ToString()).Select(x => new SummaryItemModel
            {
                PSN = x.Attribute(Enums.XMLTagAttributeName.PSN.ToString()) != null ? x.Attribute(Enums.XMLTagAttributeName.PSN.ToString()).Value : "",
                SID = x.Attribute(Enums.XMLTagAttributeName.SID.ToString()) != null ? x.Attribute(Enums.XMLTagAttributeName.SID.ToString()).Value : "",
                ProducerProductCode = x.Element(Enums.XMLTagName.ProducerProductCode.ToString()) != null ? x.Element(Enums.XMLTagName.ProducerProductCode.ToString()).Value : "",
                ProducerProductName = x.Element(Enums.XMLTagName.ProducerProductName.ToString()).Value,
                ItemQuantity = CommonMethods.ParseInt(x.Element(Enums.XMLTagName.ItemQuantity.ToString()) != null ? x.Element(Enums.XMLTagName.ItemQuantity.ToString()).Value : null),
                CountOfTradeUnits = CommonMethods.ParseInt(x.Element(Enums.XMLTagName.CountOfTradeUnits.ToString()) != null ? x.Element(Enums.XMLTagName.CountOfTradeUnits.ToString()).Value : "0"),
                PackagingLevel = x.Element(Enums.XMLTagName.PackagingLevel.ToString()).Value,
                ProductionDate = DateTime.Parse(x.Element(Enums.XMLTagName.ProductionDate.ToString()) != null ? x.Element(Enums.XMLTagName.ProductionDate.ToString()).Value : DateTime.MinValue.ToString()),
                NEW = x.Element(Enums.XMLTagName.NEW.ToString()) != null ? x.Element(Enums.XMLTagName.NEW.ToString()).Value : "",
                Length = CommonMethods.ParseDecimal(x.Element(Enums.XMLTagName.Length.ToString()) != null ? x.Element(Enums.XMLTagName.Length.ToString()).Value : "0"),
                UnitOfMeasure = x.Element(Enums.XMLTagName.UnitOfMeasure.ToString()) != null ? x.Element(Enums.XMLTagName.UnitOfMeasure.ToString()).Value : ""
            }).ToList();



            return list;
        }

        private List<SummaryItemModel> FindTopLevelSummaryItems(List<SummaryItemModel> summaryItems, XElement rootUnits, bool isDeloveryNoteRepacking)
        {
            string unitsNode = Enums.XMLTagName.Units.ToString();
            string rootNode = Enums.XMLTagName.Shipment.ToString();
            int unitsCount = 0;
            List<SummaryItemModel> topLevelItems = new List<SummaryItemModel>();

            
            if (!isDeloveryNoteRepacking)
            {
                foreach (var item in summaryItems)
                {
                    XElement element = rootUnits.Descendants().Where(x => item.PackagingLevel != "00" && (x.Attribute(Enums.XMLTagAttributeName.SID.ToString()) != null ? x.Attribute(Enums.XMLTagAttributeName.SID.ToString()).Value == item.SID : false)).FirstOrDefault();
                    XElement searchElement = element;
                    if (searchElement != null)
                    {
                        while (true)
                        {
                            if (searchElement.Name == unitsNode)
                                unitsCount++;

                            if (searchElement.Name == rootNode)
                                break;

                            searchElement = searchElement.Parent;
                        }

                        if (unitsCount == 1)
                        {
                            int elementCount = rootUnits.Descendants().Where(x => item.PackagingLevel != "00" && (x.Attribute(Enums.XMLTagAttributeName.SID.ToString()) != null ? x.Attribute(Enums.XMLTagAttributeName.SID.ToString()).Value == item.SID : false)).Count();
                            item.ProductItemCount = item.ItemQuantity * elementCount;
                            topLevelItems.Add(item);
                        }

                        unitsCount = 0;
                    }
                }
            }
            else
            {
                var repacking = summaryItems.Where(si => si.ProducerProductName == "Repacking").FirstOrDefault();
                int maxNum = 0;
                string maxPackagingLevel = "00";
                //želimo pridobiti drugi največu packaging level, da bomo lahko zračunali količine in preverili če je v vsem repackingu isti izdelek
                foreach (var item in summaryItems)
                {
                    //izpustimo repacking summaryItem
                    if (item.SID != repacking.SID)
                    {
                        var level = CommonMethods.ParseInt(item.PackagingLevel);
                        if (level > maxNum)
                        {
                            maxNum = level;
                            maxPackagingLevel = item.PackagingLevel;
                        }
                    }
                }

                //pridobimo vse summaryItems ki imajo drugi največji packaging level
                var packagingLevelSummaryItems = summaryItems.Where(si => si.PackagingLevel == maxPackagingLevel).ToList();
                //pridobimo ime izdelka (vzamemo naziv do prvega presledka)
                string productName = packagingLevelSummaryItems.First().ProducerProductName.Substring(0, packagingLevelSummaryItems.First().ProducerProductName.IndexOf(" "));
                //pridobimo število summary itemsov ki vsebujejo to ime izdelka
                int summaryItemsCount = packagingLevelSummaryItems.Where(si => si.ProducerProductName.Contains(productName)).Count();

                //če je summaryItemsCount enako število objektov v packagingLevelSummaryItems potem vemo da je v Paketu Repacking samo en izdelek
                if (summaryItemsCount == packagingLevelSummaryItems.Count)
                {
                    //TODO: izračunamo količino (ItemQuantity * število paketov - packagingLevelSummaryItems.Count)
                    //V deliveryNoteItem shranimo samo en zapis ki je Repacking
                    repacking.ProducerProductName = packagingLevelSummaryItems.First().ProducerProductName;
                    repacking.ProductItemCount = packagingLevelSummaryItems.Sum(pis => pis.ItemQuantity);
                    repacking.Notes = "Repacking dobavnica z enakim izdelkom";
                    topLevelItems.Add(repacking);
                }
                else
                {
                    //Obstaja več izdelkov v istem paketu (Repacking)
                    //V deliveryNoteItem shranimo toliko zapisev kot je različnih izdelkov. UID bodo vseboavli enako, razlikovalo se bo samo naziv izdelka, količina,...
                    //pridobimo seznam različnih izdelkov
                    var distinctProducts = packagingLevelSummaryItems.GroupBy(p => p.ProducerProductName.Substring(0, p.ProducerProductName.IndexOf(" "))).ToList();
                    int productCount = 1;
                    foreach (var item in distinctProducts)
                    {
                        var newTopLevelItem = new SummaryItemModel();
                        newTopLevelItem.CountOfTradeUnits = item.Count();
                        newTopLevelItem.PSN = item.Select(i=>i.PSN).First();
                        newTopLevelItem.SID = item.Select(i => i.SID).First();
                        newTopLevelItem.ItemQuantity = 0;
                        newTopLevelItem.Notes = String.Format("Repacking dobavnica z različnimi izdelki - ({0}/{1})", productCount, distinctProducts.Count);
                        newTopLevelItem.PackagingLevel = repacking.PackagingLevel;
                        newTopLevelItem.ProducerProductCode = repacking.ProducerProductCode;
                        newTopLevelItem.ProducerProductName = item.Key;
                        newTopLevelItem.UnitOfMeasure = repacking.UnitOfMeasure;
                        newTopLevelItem.ProductItemCount = item.Sum(i => i.ItemQuantity);
                        productCount++;

                        topLevelItems.Add(newTopLevelItem);
                    }
                }
            }

            return topLevelItems;
        }

        private List<Item> SetHierarchyOfAtomes(XElement rootUnits, List<SummaryItemModel> summaryItems)
        {
            List<Item> updatedLeafs = new List<Item>();
            string currentAtomeSID = "", previousAtomeSID = "";
            SummaryItemModel atomeItem = null;

            //Pridobimo vse liste (atome, artikle) xml dokumenta.
            var leafs = (from l in rootUnits.Descendants()
                         where !l.Elements().Any()
                         select new Item
                         {
                             atomeElement = l,
                             UID = l.Attribute(Enums.XMLTagAttributeName.UID.ToString())?.Value,
                             Parents = new List<Item>(),
                             PackagesUIDs = "",
                             PackagesSIDs = ""
                         }).ToList();

            //Sestavimo hirearhijo atoma
            foreach (var item in leafs)
            {
                XElement atomeElem = item.atomeElement;
                Item obj = ConstructAtomeHierarchyUID(item);
                obj.atomeElement = atomeElem;
                if (!String.IsNullOrEmpty(obj.PackagesUIDs))
                    obj.PackagesUIDs = obj.PackagesUIDs.Remove(obj.PackagesUIDs.LastIndexOf(','));

                obj.PackagesSIDs = ConstructAtomeHierarchySID(obj.atomeElement, "");

                if (!String.IsNullOrEmpty(obj.PackagesSIDs))
                    obj.PackagesSIDs = obj.PackagesSIDs.Remove(item.PackagesSIDs.LastIndexOf(','));

                if (atomeElem.Attribute(Enums.XMLTagAttributeName.SID.ToString()) != null)
                {
                    currentAtomeSID = atomeElem.Attribute(Enums.XMLTagAttributeName.SID.ToString()).Value;

                    if (currentAtomeSID != previousAtomeSID)
                        atomeItem = summaryItems.Where(a => a.SID == currentAtomeSID).FirstOrDefault();

                    obj.Quantity = atomeItem.UnitOfMeasure.ToLower().Contains("kgm") ? CommonMethods.ParseDecimal(atomeItem.NEW.Replace(".", ",")) : atomeItem.Length;
                    obj.MeasuringUnitCode = atomeItem.UnitOfMeasure;
                    previousAtomeSID = currentAtomeSID;
                }


                updatedLeafs.Add(obj);
            }

            return updatedLeafs;
        }

        private Item ConstructAtomeHierarchyUID(Item item)
        {
            if (item.atomeElement.Attribute(Enums.XMLTagAttributeName.UID.ToString()) != null)
            {
                item.PackagesUIDs = item.PackagesUIDs.Insert(0, item.atomeElement.Attribute(Enums.XMLTagAttributeName.UID.ToString()).Value + CommonMethods.PackageDelimiter + " ");

                item.Parents.Add(new Item { UID = item.atomeElement.Attribute(Enums.XMLTagAttributeName.UID.ToString()).Value });
                item.atomeElement = item.atomeElement.Parent;
                return ConstructAtomeHierarchyUID(item);
            }
            else if (!item.atomeElement.Name.LocalName.Equals(Enums.XMLTagName.Shipment.ToString()))
            {
                item.atomeElement = item.atomeElement.Parent;
                return ConstructAtomeHierarchyUID(item);
            }

            return item;
        }

        private string ConstructAtomeHierarchySID(XElement element, string packages)
        {
            string packagesSIDs = packages;

            if (element.Attribute(Enums.XMLTagAttributeName.SID.ToString()) != null)
            {
                packagesSIDs = packagesSIDs.Insert(0, element.Attribute(Enums.XMLTagAttributeName.SID.ToString()).Value + CommonMethods.PackageDelimiter + " ");
                return ConstructAtomeHierarchySID(element.Parent, packagesSIDs);
            }
            else if (!element.Name.LocalName.Equals(Enums.XMLTagName.Shipment.ToString()))
                return ConstructAtomeHierarchySID(element.Parent, packagesSIDs);

            return packagesSIDs;
        }

        protected void ASPxGridViewDeliveryNoteItem_DataBound(object sender, EventArgs e)
        {
            //EnableButtonBasedOnGridRows(ASPxGridViewDeliveryNoteItem, btnAdd, btnEdit, btnDelete);
            HtmlGenericControl control = (HtmlGenericControl)deliveryNoteItem.FindControl("deliveryNoteProductBadge");
            control.InnerText = (sender as ASPxGridView).VisibleRowCount.ToString();
        }

        private List<SummaryItemModel> FindTopLevelSummaryItems(List<XElement> products)
        {
            List<SummaryItemModel> summaryList = new List<SummaryItemModel>();

            foreach (var item in products)
            {
                var obj = item.Descendants(Enums.XMLTagName.Serijske.ToString()).Descendants(Enums.XMLTagName.Serijske_podnivo.ToString()).FirstOrDefault();//če obstaja tag Serijski_podnivo potem preštejemo koliko je elementov notri
                SummaryItemModel summary = new SummaryItemModel();
                summary.CountOfTradeUnits = item.Descendants(Enums.XMLTagName.Serijske.ToString()).Count();
                summary.ItemQuantity = obj != null ? item.Descendants(Enums.XMLTagName.Serijske.ToString()).Descendants(Enums.XMLTagName.Serijske_podnivo.ToString()).Count() : 1;

                summary.IsTopLevelElement = true;
                summary.ProducerProductCode = item.Descendants(Enums.XMLTagName.Sifra_ident.ToString()).FirstOrDefault().Value;
                summary.ProducerProductName = item.Descendants(Enums.XMLTagName.Naziv_ident.ToString()).FirstOrDefault().Value;
                summary.UnitOfMeasure = item.Descendants(Enums.XMLTagName.EM_ident.ToString()).FirstOrDefault().Value;
                summary.SID = item.Descendants(Enums.XMLTagName.Sifra_ident.ToString()).FirstOrDefault().Value;

                summaryList.Add(summary);
            }

            return summaryList;
        }

        private List<Item> SetHierarchyOfAtomes(List<XElement> idents, List<SummaryItemModel> summaryItems)
        {
            List<Item> updatedLeafs = new List<Item>();

            SummaryItemModel atomeItem = null;

            foreach (var item in idents)
            {
                bool sublevel = true;
                decimal identQuantity = CommonMethods.ParseDecimal(item.Descendants(Enums.XMLTagName.Kolicina_ident.ToString()).FirstOrDefault().Value);
                var atomes = item.Descendants(Enums.XMLTagName.Serijske_podnivo.ToString()).ToList();

                if (atomes.Count == 0)
                {
                    sublevel = false;
                    atomes = item.Descendants(Enums.XMLTagName.Serijske.ToString()).ToList();
                }

                foreach (var atome in atomes)
                {
                    Item obj = new Item();
                    obj.atomeElement = atome;
                    obj.MeasuringUnitCode = item.Descendants(Enums.XMLTagName.EM_ident.ToString()).FirstOrDefault().Value;
                    obj.PackagesUIDs = ConstructPackagesUIDsHierarchy(obj);
                    obj.Quantity = identQuantity / atomes.Count();
                    obj.UID = sublevel ? atome.Descendants(Enums.XMLTagName.SP_Barkoda_serijske.ToString()).FirstOrDefault().Value : atome.Descendants(Enums.XMLTagName.Barkoda_serijske.ToString()).FirstOrDefault().Value;
                    obj.PackagesSIDs = item.Descendants(Enums.XMLTagName.Sifra_ident.ToString()).FirstOrDefault().Value;

                    updatedLeafs.Add(obj);
                }
            }

            return updatedLeafs;
        }

        private string ConstructPackagesUIDsHierarchy(Item item)
        {
            var element = item.atomeElement;
            string uids = "";
            while (true)
            {
                if (element.Name == Enums.XMLTagName.Ident.ToString())
                    break;

                var codeElement = item.atomeElement.Descendants(Enums.XMLTagName.SP_Barkoda_serijske.ToString()).FirstOrDefault();
                uids += (codeElement != null ? codeElement.Value : item.atomeElement.Descendants(Enums.XMLTagName.Barkoda_serijske.ToString()).FirstOrDefault().Value) + ", ";

                element = element.Parent;
            }

            return uids;
        }
    }
}