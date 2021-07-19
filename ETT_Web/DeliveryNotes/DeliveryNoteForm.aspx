<%@ Page Title="Elektronske dobavnice" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="DeliveryNoteForm.aspx.cs" Inherits="ETT_Web.DeliveryNotes.DeliveryNoteForm" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ Register TagPrefix="widget" TagName="AttachmentUpload" Src="~/Widgets/UploadAttachment.ascx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var isRequestInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtDeliveryNoteNumber];
            var dateEditItems = [dateEditDeliveryNoteDate, dtPrejetjeMaterialaClient];
            var lookupItmes = [lookUpSupplier, lookUpLocation];
            process = InputFieldsValidation(lookupItmes, inputItems, dateEditItems, null, null, null);

            if (s.GetText() == 'Izbriši')
                process = true;

            if (process && !isRequestInitiated) {
                LoadingPanel.Show();
                isRequestInitiated = true;
                e.processOnServer = true;
            }
            else
                e.processOnServer = false;
        }

        function HandleUserAction(s, e) {
            //vrne vrednost ki predstavlja akcijo uporabnika (add, edit, delete)
            /* var actionParameter = HandleUserActionsOnTabs(gridDeliveryNoteItem, btnAdd, btnEdit, btnDelete, s);
 
             if (actionParameter != "") {
                 callbackPanel.PerformCallback(actionParameter);
             }*/

            gridDeliveryNoteItem.GetRowValues(gridDeliveryNoteItem.GetFocusedRowIndex(), 'DeliveryNoteItemID', OnGetRowValues);
        }

        function OnGetRowValues(value) {

            var queryString = '<%= ETT_Utilities.Common.Enums.QueryStringName.recordId.ToString() %>';
            var url = '/DeliveryNotes/ProductOverview.aspx?' + queryString + '=' + value;
            window.location.replace(url);
        }

        function OnClosePopUpHandler(command, sender, usersCount) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'User':
                            popupControlUsers.Hide();

                            if (usersCount > 0) {
                                $('#ContentPlaceHolderMain_userCredentialsBadge').empty();
                                $('#ContentPlaceHolderMain_userCredentialsBadge').append(usersCount);

                            }

                            callbackPanel.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'User':
                            popupControlUsers.Hide();
                    }
                    break;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="nav nav-tabs">
        <li class="nav-item">
            <a class="nav-link active" data-toggle="tab" href="#Basic">Osnovno</a>
        </li>
        <li class="nav-item" runat="server" id="deliveryNoteItem">
            <a class="nav-link" data-toggle="tab" href="#DeliveryNoteProduct"><span runat="server" id="deliveryNoteProductBadge" class="badge badge-pill badge-info">0</span> Artikli dobavnice</a>
        </li>
    </ul>

    <div class="tab-content border mb-3">

        <div id="Basic" class="container-fluid p-0 tab-pane active">
            <div class="card">
                <div class="card-header" style="background-color: #FAFCFE">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6>Osnovni podatki</h6>
                        <a data-toggle="collapse" href="#collapseBasicData" aria-expanded="true" aria-controls="collapseBasicData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                    </div>
                </div>
                <div class="collapse show" id="collapseBasicData">
                    <div class="card-body" style="background-color: #eef2f5d6;">

                        <div class="row m-0">
                            <div class="col-lg-8">

                                <div class="row m-0 pb-3">
                                    <div class="col-lg-6 mb-2 mb-lg-0">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0" style="margin-right: 55px;">
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DATUM DOBAVNICE : *" Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxDateEdit ID="DateEditDeliveryNoteDate" runat="server" EditFormat="Date" Width="100%"
                                                    CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                                    ClientInstanceName="dateEditDeliveryNoteDate">
                                                    <FocusedStyle CssClass="focus-text-box-input" />
                                                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                    <DropDownButton Visible="true"></DropDownButton>
                                                    <ClientSideEvents Init="SetFocus" />
                                                </dx:ASPxDateEdit>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0 mr-3">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="ŠTEVILKA DOBAVNICE : *" Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxTextBox runat="server" ID="txtDeliveryNoteNumber" ClientInstanceName="clientTxtDeliveryNoteNumber"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="20" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row m-0 pb-3">
                                    <div class="col-lg-6 mb-2 mb-lg-0">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0" style="margin-right: 14px;">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="DOBAVITELJ : *" Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxGridLookup ID="GridLookupSupplier" runat="server" ClientInstanceName="lookUpSupplier"
                                                    KeyFieldName="ClientID" TextFormatString="{0}" CssClass="text-box-input"
                                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                                                    OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSSupplier" IncrementalFilteringMode="Contains">
                                                    <ClearButton DisplayMode="OnHover" />
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <GridViewStyles>
                                                        <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                    </GridViewStyles>
                                                    <GridViewProperties>
                                                        <SettingsBehavior EnableRowHotTrack="True" AllowEllipsisInText="true" AllowDragDrop="false" />
                                                        <SettingsPager ShowSeparators="True" NumericButtonCount="3" EnableAdaptivity="true" />
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowVerticalScrollBar="True"
                                                            ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                    </GridViewProperties>
                                                    <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Dobavitelji" SwitchToModalAtWindowInnerWidth="650" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                                                            ReadOnly="true" MinWidth="230" MaxWidth="400" Width="40%">
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn Caption="Država"
                                                            FieldName="Country" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn Caption="Telefon"
                                                            FieldName="Phone" Width="20%" AdaptivePriority="1" MinWidth="200" MaxWidth="250">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn Caption="Email"
                                                            FieldName="Email" Width="20%" AdaptivePriority="0" MinWidth="450" MaxWidth="250">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                </dx:ASPxGridLookup>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6 mb-2 mb-lg-0">
                                        <div class="row m-0 align-items-center justify-content-center">
                                            <div class="col-0 p-0" style="margin-right: 15px;">
                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="LOKACIJA : *" Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxGridLookup ID="GridLookupLocation" runat="server" ClientInstanceName="lookUpLocation"
                                                    KeyFieldName="LocationID" TextFormatString="{0}" CssClass="text-box-input"
                                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                                                    OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSLocation"
                                                    IncrementalFilteringMode="Contains">
                                                    <ClearButton DisplayMode="OnHover" />
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <GridViewStyles>
                                                        <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                    </GridViewStyles>
                                                    <GridViewProperties>
                                                        <SettingsBehavior EnableRowHotTrack="True" AllowEllipsisInText="true" AllowDragDrop="false" />
                                                        <SettingsPager ShowSeparators="True" NumericButtonCount="3" EnableAdaptivity="true" />
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowVerticalScrollBar="True"
                                                            ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                    </GridViewProperties>
                                                    <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Lokacije" SwitchToModalAtWindowInnerWidth="650" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                                                            ReadOnly="true" MinWidth="230" MaxWidth="400" Width="50%">
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn Caption="Je kupec?"
                                                            FieldName="IsBuyer" Width="20%" AdaptivePriority="1" MinWidth="200" MaxWidth="250">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn Caption="Opombe"
                                                            FieldName="Notes" Width="30%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>

                                                    </Columns>
                                                </dx:ASPxGridLookup>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row m-0 pb-3">
                                    <div class="col-lg-12">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0" style="margin-right: 41px;">
                                                <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="OPOMBE : " Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="4" MaxLength="500" CssClass="text-box-input" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxMemo>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-lg-6 mb-2 mb-lg-0">
                                    <div class="row m-0 align-items-center">
                                        <div class="col-0 p-0" style="margin-right: 55px;">
                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="DATUM PREJETJA : *" Font-Bold="true"></dx:ASPxLabel>
                                        </div>
                                        <div class="col p-0">
                                            <dx:ASPxDateEdit ID="dtPrejetjeMateriala" runat="server" EditFormat="Date" Width="100%"
                                                CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                                ClientInstanceName="dtPrejetjeMaterialaClient">
                                                <FocusedStyle CssClass="focus-text-box-input" />
                                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                <DropDownButton Visible="true"></DropDownButton>
                                                <ClientSideEvents Init="SetFocus" />
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>
                                </div>

                                <div class="row m-0 pb-3">
                                    <div class="col-lg-12">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0" style="margin-right: 41px; margin-top: 20px">
                                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="Številka in datum dovoljenja za nakup, Ime pristojnega organa : " Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0" style="margin-right: 41px; margin-top: 20px">
                                                <dx:ASPxMemo ID="memDovoljenje" runat="server" Width="100%" Rows="2" MaxLength="500" CssClass="text-box-input" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxMemo>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row m-0 pb-3">
                                    <div class="col-lg-12">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0" style="margin-right: 41px; margin-top: 20px">
                                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="NAPAKA : " Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0" style="margin-right: 41px; margin-top: 20px">
                                                <dx:ASPxMemo ID="memError" runat="server" Width="100%" Rows="6" MaxLength="5000" CssClass="text-box-input" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxMemo>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="col-lg-4" id="active-drop-zone">
                                <widget:AttachmentUpload runat="server" ID="DocumentUpload" OnPopulateAttachments="DocumentUpload_PopulateAttachments" OnUploadComplete="DocumentUpload_UploadComplete"
                                    OnDeleteAttachments="DocumentUpload_DeleteAttachments" OnDownloadAttachments="DocumentUpload_DownloadAttachments"
                                    WebsiteDocumentContainerID="ContentPlaceHolderMain_DocumentUpload_documentContainer" AllowedFileExtensions=".xml" />
                            </div>
                        </div>

                        <div class="row m-0 pt-5">
                            <div class="col-12 text-right">
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnProcessXMLFile" runat="server" Text="Razčleni XML" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="clientBtnProcessXMLFile" UseSubmitBehavior="false" OnClick="btnProcessXMLFile_Click">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/xml_parse.png" UrlHottracked="../Images/xml_parse_hover.png" />
                                        <ClientSideEvents Click="CheckFieldValidation" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSave" runat="server" Text="Uveljavi spremembe" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="clientBtnSave" UseSubmitBehavior="false" OnClick="btnSave_Click">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/btnSave.png" UrlHottracked="../Images/btnSaveHover.png" />
                                        <ClientSideEvents Click="CheckFieldValidation" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSaveChanges" runat="server" Text="Shrani" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="clientBtnSaveChanges" UseSubmitBehavior="false" OnClick="btnSaveChanges_Click">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                                        <ClientSideEvents Click="CheckFieldValidation" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                                        Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/cancel.png" UrlHottracked="../Images/cancelHover.png" />
                                    </dx:ASPxButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="DeliveryNoteProduct" class="container-fluid tab-pane fade">
            <div class="card">
                <div class="card-header" style="background-color: #FAFCFE">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6>Artikli</h6>
                        <a data-toggle="collapse" href="#collapseDeliveryNoteProduct" aria-expanded="true" aria-controls="collapseDeliveryNoteProduct"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                    </div>
                </div>
                <div class="collapse show" id="collapseDeliveryNoteProduct">
                    <div class="card-body p-0" style="background-color: #eef2f5d6;">
                        <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
                            ClientInstanceName="callbackPanel">
                            <PanelCollection>
                                <dx:PanelContent>

                                    <dx:ASPxGridView ID="ASPxGridViewDeliveryNoteItem" Width="100%" runat="server" KeyFieldName="DeliveryNoteItemID" DataSourceID="XpoDSDeliveryNoteItem"
                                        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridDeliveryNoteItem"
                                        OnDataBound="ASPxGridViewDeliveryNoteItem_DataBound">
                                        <ClientSideEvents RowDblClick="HandleUserAction" />
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                                            AllowHideDataCellsByColumnMinWidth="true">
                                        </SettingsAdaptivity>
                                        <Settings ShowVerticalScrollBar="True"
                                            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                            ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                        <SettingsBehavior AllowEllipsisInText="true" />
                                        <Paddings Padding="0" />
                                        <SettingsPager PageSize="50">
                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                        </SettingsPager>
                                        <SettingsBehavior AllowFocusedRow="true" />
                                        <Styles>
                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                            <Cell Wrap="False" />
                                        </Styles>
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o artiklih na dobavnici. Dodaj novega." />

                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Koda proizvajalca" FieldName="SupplierProductCode" AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="400" Width="20%" />
                                            <dx:GridViewDataTextColumn Caption="Naziv izdelka" FieldName="SupplierProductName" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="20%" />
                                            <dx:GridViewDataTextColumn Caption="Pakiranje" FieldName="CountOfTradeUnits" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Merske enote" FieldName="MeasuringUnitID.Symbol" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Količina" FieldName="ItemQuantity" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="PSN" FieldName="PSN" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="SID" FieldName="SID" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Artikli skupaj" FieldName="ProductItemCount" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="10%" />
                                        </Columns>
                                    </dx:ASPxGridView>

                                    <dx:ASPxPopupControl ID="PopupControlUsers" runat="server" ContentUrl="Users_popup.aspx"
                                        ClientInstanceName="popupControlUsers" Modal="True" HeaderText="UPORABNIK"
                                        CloseAction="CloseButton" Width="680px" Height="405px" PopupHorizontalAlign="WindowCenter"
                                        PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                        AllowResize="true" ShowShadow="true"
                                        OnWindowCallback="PopupControlUsers_WindowCallback">
                                        <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                        <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="800" VerticalAlign="WindowCenter" MinHeight="285px" MaxWidth="680px" MaxHeight="285px" />
                                        <ContentStyle BackColor="#F7F7F7">
                                            <Paddings Padding="0px"></Paddings>
                                        </ContentStyle>
                                    </dx:ASPxPopupControl>

                                    <div class="row m-0 mt-2 pb-2">
                                        <div class="col-sm-12 text-right">
                                            <dx:ASPxButton ID="btnEdit" runat="server" Text="Preglej artikle" AutoPostBack="false"
                                                Height="25" Width="90" ClientInstanceName="btnEdit">
                                                <ClientSideEvents Click="HandleUserAction" />
                                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                                <Image Url="../Images/edit.png" UrlHottracked="../Images/editHover.png" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                        <dx:XpoDataSource ID="XpoDSDeliveryNoteItem" runat="server" ServerMode="true"
                            DefaultSorting="DeliveryNoteItemID DESC" TypeName="ETT_DAL.ETTPotocnik.DeliveryNoteItem" Criteria="[DeliveryNoteID] = ?">
                            <CriteriaParameters>
                                <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
                            </CriteriaParameters>
                        </dx:XpoDataSource>
                    </div>
                </div>
            </div>
        </div>

        <dx:XpoDataSource ID="XpoDSSupplier" runat="server" ServerMode="true"
            DefaultSorting="ClientID" TypeName="ETT_DAL.ETTPotocnik.Client" Criteria="[ClientTypeID] = 1">
        </dx:XpoDataSource>
        <dx:XpoDataSource ID="XpoDSLocation" runat="server" ServerMode="true"
            DefaultSorting="LocationID" TypeName="ETT_DAL.ETTPotocnik.Location" Criteria="[IsWarehouse] = 1">
        </dx:XpoDataSource>
    </div>

</asp:Content>
