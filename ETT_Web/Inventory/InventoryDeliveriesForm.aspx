<%@ Page Title="Artikli po UID" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="InventoryDeliveriesForm.aspx.cs" Inherits="ETT_Web.Inventory.InventoryDeliveriesForm" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ Register TagPrefix="widget" TagName="AttachmentUpload" Src="~/Widgets/UploadAttachment.ascx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var isRequestInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtDeliveryNoteNumber];
            var dateEditItems = [dateEditDeliveryNoteDate];
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

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 55px;">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="ARTIKEL : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxGridLookup ID="GridLookupProduct" runat="server" ClientInstanceName="lookUpProduct"
                                            KeyFieldName="ProductID" TextFormatString="{0}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                                            OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSProduct" IncrementalFilteringMode="Contains"
                                            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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
                                            <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Artikli" SwitchToModalAtWindowInnerWidth="650" />
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                                                    ReadOnly="true" MinWidth="230" MaxWidth="400" Width="40%">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Merska enota"
                                                    FieldName="MeasuringUnitID.Name" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Kategorija"
                                                    FieldName="CategoryID.Name" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Proizvajalec"
                                                    FieldName="SupplierID.Name" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                            </Columns>
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="KODA PROIZVAJALCA : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtSupplierProductCode" ClientInstanceName="clientTxtSupplierProductCode"
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
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="UID koda : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtUIDAtomeCode" ClientInstanceName="clientTxtUIDAtomeCode"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="250" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="UID pakiranj : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtUIDPackaging" ClientInstanceName="clientTxtUIDPackaging"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="800" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
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
                                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="4" MaxLength="1250" CssClass="text-box-input" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-12">
                                <div class="jumbotron pb-0 mb-0" style="background-color: #e9ecef00">
                                    <h5 class="font-italic"><i class='fas fa-table'></i>Podatki o prejemu</h5>
                                    <hr class="mb-4 w-100">
                                    <div class="row m-0 pb-3">

                                        <div class="col-lg-4 mb-2 mb-lg-0">
                                            <div class="row m-0 align-items-center">
                                                <div class="col-0 p-0" style="margin-right: 14px;">
                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="PAKIRANJE : " Font-Bold="true"></dx:ASPxLabel>
                                                </div>
                                                <div class="col p-0">
                                                    <dx:ASPxTextBox runat="server" ID="txtPackaging" ClientInstanceName="clientTxtPackaging"
                                                        CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="250" AutoCompleteType="Disabled" ClientEnabled="false" BackColor="LightGray" Font-Bold="true">
                                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    </dx:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-4 mb-2 mb-lg-0">
                                            <div class="row m-0 align-items-center justify-content-center">
                                                <div class="col-0 p-0" style="margin-right: 15px;">
                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="DATUM DOBAVE : " Font-Bold="true"></dx:ASPxLabel>
                                                </div>
                                                <div class="col p-0">
                                                    <dx:ASPxDateEdit ID="DateEditDeliveryNoteDate" runat="server" EditFormat="Date" Width="100%"
                                                        CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                                        ClientInstanceName="dateEditDeliveryNoteDate" ClientEnabled="false" BackColor="LightGray" Font-Bold="true">
                                                        <FocusedStyle CssClass="focus-text-box-input" />
                                                        <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                        <DropDownButton Visible="true"></DropDownButton>
                                                        <ClientSideEvents Init="SetFocus" />
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-4 mb-2 mb-lg-0">
                                            <div class="row m-0 align-items-center justify-content-center">
                                                <div class="col-0 p-0" style="margin-right: 15px;">
                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="DOBAVA V : " Font-Bold="true"></dx:ASPxLabel>
                                                </div>
                                                <div class="col p-0">
                                                    <dx:ASPxGridLookup ID="GridLookupLocation" runat="server" ClientInstanceName="lookUpLocation"
                                                        KeyFieldName="LocationID" TextFormatString="{0}" CssClass="text-box-input" Font-Bold="true"
                                                        ClientEnabled="false" ForeColor="LightGray"
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
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-12">
                                <div class="jumbotron pb-0 mb-0" style="background-color: #e9ecef00">
                                    <h5 class="font-italic"><i class='fas fa-table'></i> Podatki o premikih</h5>
                                    <hr class="mb-4 w-100">
                                    <dx:ASPxGridView ID="ASPxGridViewInventoryDeliveriesLocation" Width="100%" runat="server" KeyFieldName="InventoryDeliveriesLocationID" DataSourceID="XpoDSInventoryDeliveriesLocation"
                                        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridInventoryDeliveriesLocation">
                                        <ClientSideEvents RowDblClick="HandleUserAction" />
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                                            AllowHideDataCellsByColumnMinWidth="true">
                                        </SettingsAdaptivity>
                                        <Settings ShowVerticalScrollBar="True"
                                            ShowFilterBar="Auto" ShowFilterRow="false" VerticalScrollableHeight="100"
                                            ShowFilterRowMenu="false" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
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
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o premikih artikla." />

                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="UID" FieldName="InventoryDeliveriesID.AtomeUID250" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" MaxWidth="200" Width="13%" />
                                            <dx:GridViewDataDateColumn Caption="Datum" FieldName="tsInsert" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="13%" PropertiesDateEdit-DisplayFormatString="dd. MMMM yyyy" />
                                            <dx:GridViewDataTextColumn Caption="Artikel" FieldName="InventoryDeliveriesID.InventoryStockID.ProductID.Name" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="30%" />
                                            <dx:GridViewDataTextColumn Caption="Iz lokacije" FieldName="LocationFromID.Name" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="20%" />
                                            <dx:GridViewDataTextColumn Caption="Na lokacijo" FieldName="LocationToID.Name" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="20%" />

                                            <dx:GridViewBandColumn Caption="Uporabnik">
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="UserID.EmployeeID.Firstname" Caption="Ime" Width="120px" ExportWidth="90"
                                                        AdaptivePriority="1" MinWidth="120" MaxWidth="200">
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="UserID.EmployeeID.Lastname" Caption="Priimek" Width="160px" ExportWidth="120"
                                                        AdaptivePriority="1" MinWidth="160" MaxWidth="250">
                                                    </dx:GridViewDataColumn>
                                                </Columns>
                                            </dx:GridViewBandColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </div>
                            </div>
                        </div>


                        <div class="row m-0 pt-5">
                            <div class="col-12 text-right">

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSave" runat="server" Text="Uveljavi spremembe" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="clientBtnSave" UseSubmitBehavior="false" OnClick="btnSave_Click"
                                        ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/btnSave.png" UrlHottracked="../Images/btnSaveHover.png" />
                                        <ClientSideEvents Click="CheckFieldValidation" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSaveChanges" runat="server" Text="Shrani" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="clientBtnSaveChanges" UseSubmitBehavior="false" OnClick="btnSaveChanges_Click"
                                        ClientVisible="false">
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

        <dx:XpoDataSource ID="XpoDSSupplier" runat="server" ServerMode="true"
            DefaultSorting="ClientID" TypeName="ETT_DAL.ETTPotocnik.Client" Criteria="[ClientTypeID] = 1">
        </dx:XpoDataSource>
        <dx:XpoDataSource ID="XpoDSLocation" runat="server" ServerMode="true"
            DefaultSorting="LocationID" TypeName="ETT_DAL.ETTPotocnik.Location">
        </dx:XpoDataSource>

        <dx:XpoDataSource ID="XpoDSProduct" runat="server" ServerMode="true"
            DefaultSorting="ProductID" TypeName="ETT_DAL.ETTPotocnik.Product">
        </dx:XpoDataSource>

        <dx:XpoDataSource ID="XpoDSInventoryDeliveriesLocation" runat="server" ServerMode="true"
            DefaultSorting="InventoryDeliveriesLocationID DESC" TypeName="ETT_DAL.ETTPotocnik.InventoryDeliveriesLocation" Criteria="[InventoryDeliveriesID] = ?">
            <CriteriaParameters>
                <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
            </CriteriaParameters>
        </dx:XpoDataSource>
    </div>

</asp:Content>
