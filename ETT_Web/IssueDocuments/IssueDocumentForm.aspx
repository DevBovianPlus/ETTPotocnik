<%@ Page Title="Izdajnica" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="IssueDocumentForm.aspx.cs" Inherits="ETT_Web.IssueDocuments.IssueDocumentForm" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ Register TagPrefix="widget" TagName="AttachmentUpload" Src="~/Widgets/UploadAttachment.ascx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var isRequestInitiated = false;
        var confirmCompletingIssueDocument = false;

        $(document).ready(function () {
            
            $("#submitIssue").on('click', function () {
                HideModal();
                LoadingPanel.Show();
                confirmCompletingIssueDocument = true;
                btnCompleteIssueDocument.DoClick();
            });
        });


        function CheckFieldValidation(s, e) {
            var process = false;
            process = ValidateInputFields();

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

        function OnClosePopUpHandler(command, sender, usersCount) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'IssueDocumentPosition':
                            PopupControlIssueDocumentPos.Hide();
                            gridIssueDocumentPosition.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'IssueDocumentPosition':
                            PopupControlIssueDocumentPos.Hide();
                    }
                    break;
            }
        }

        function HandleUserPopupAction(s, e) {
            var parameter = HandleUserActionsOnTabs(gridIssueDocumentPosition, btnAdd, btnEdit, btnDelete, s);
            if (parameter != "" && ValidateInputFields())
                CallbackPanel.PerformCallback("UserAction;" + parameter);
        }

        function ValidateInputFields() {
            var process = false;
            var inputItems = [txtName, txtInvoiceNumber];
            var dateEditItems = [DateEditIssueDocumentDate];
            var lookupItmes = [lookUpBuyer];
            process = InputFieldsValidation(lookupItmes, inputItems, dateEditItems, null, null, null);

            return process;
        }

        function btnCompleteIssueDocument_Click(s, e) {
            if (ValidateInputFields() && !isRequestInitiated && confirmCompletingIssueDocument) {
                LoadingPanel.Show();
                isRequestInitiated = true;
                e.processOnServer = true;
                confirmCompletingIssueDocument = false;
            }
            else if (!confirmCompletingIssueDocument) {
                ShowModal("Želite prenesti izdajnico?", "S potrditvijo boste potrdili in zaključili izdajnico!");
                e.processOnServer = false;
            }
            else
                e.processOnServer = false;
        }

        function ShowModal(title, message) {

            $('#ConfirmModalTitle').empty();
            $('#ConfirmModalTitle').append(title);
            $('#ConfirmModalBody').empty();
            $('#ConfirmModalBody').append(message);

            $('#ConfirmModal').modal('show');
        }

        function HideModal() {
            $('#ConfirmModal').modal('hide');
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="nav nav-tabs d-none">
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
                            <div class="col-lg-2">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="ŠTEV. IZDAJE : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtIssueDocumentNumber" ClientInstanceName="txtIssueDocumentNumber"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="20" AutoCompleteType="Disabled"
                                            BackColor="LightGray" ClientEnabled="false" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-7">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="STATUS : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col-3 p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtIssueStatus" ClientInstanceName="txtIssueStatus"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="300" AutoCompleteType="Disabled"
                                            BackColor="LightGray" ClientEnabled="false" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-end">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DATUM IZDAJE : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxDateEdit ID="DateEditIssueDocumentDate" runat="server" EditFormat="Date" Width="100%"
                                            CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                            ClientInstanceName="DateEditIssueDocumentDate">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                            <ClientSideEvents Init="SetFocus" />
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0" style="margin-right: 40px">
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="NAZIV : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="txtName"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="300" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 38px;">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="KUPEC : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxGridLookup ID="GridLookupBuyer" runat="server" ClientInstanceName="lookUpBuyer"
                                            KeyFieldName="ClientID" TextFormatString="{0}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                                            OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSBuyer" IncrementalFilteringMode="Contains">
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

                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="INTERNA DOB. : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtInternalDocument" ClientInstanceName="txtInternalDocument"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="300" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="ŠTEV. RAČUN : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtInvoiceNumber" ClientInstanceName="txtInvoiceNumber"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="300" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-12">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 33px;">
                                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="OPOMBE : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="3" MaxLength="2000" CssClass="text-box-input" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-12">
                                <h5 class="font-italic"><i class='fas fa-table'></i>&nbsp; Artikli na izdajnici</h5>
                                <hr class="mb-4 w-100">

                                <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
                                    ClientInstanceName="CallbackPanel">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxGridView ID="ASPxGridViewIssueDocumentPosition" Width="100%" runat="server" KeyFieldName="IssueDocumentPositionID" DataSourceID="XpoDSIssueDocumentPosition"
                                                CssClass="gridview-no-header-padding mw-100" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridIssueDocumentPosition"
                                                OnDataBound="ASPxGridViewIssueDocumentPosition_DataBound">
                                                <ClientSideEvents RowDblClick="HandleUserPopupAction" />
                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                                                    AllowHideDataCellsByColumnMinWidth="true">
                                                </SettingsAdaptivity>
                                                <Settings ShowVerticalScrollBar="True"
                                                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300"
                                                    ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="true" />
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
                                                <SettingsText EmptyDataRow="Trenutno ni podatka o artiklih na izdajnici. Dodaj novega." />

                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="SupplierID.Name" AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="400" Width="30%" />
                                                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name" AdaptivePriority="2" MinWidth="100" MaxWidth="150" Width="30%" />
                                                    <dx:GridViewDataTextColumn Caption="UID" FieldName="UID250" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="20%" />
                                                    <dx:GridViewDataTextColumn Caption="Količina" FieldName="Quantity" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="20%" />
                                                </Columns>

                                                <TotalSummary>
                                                    <dx:ASPxSummaryItem FieldName="Quantity" SummaryType="Sum" DisplayFormat="N2" Tag="Vsota količin" />
                                                </TotalSummary>
                                            </dx:ASPxGridView>

                                            <dx:ASPxPopupControl ID="PopupControlIssueDocumentPos" runat="server" ContentUrl="IssueDocument_popup.aspx"
                                                ClientInstanceName="PopupControlIssueDocumentPos" Modal="True" HeaderText="POZICIJE IZDAJNICE"
                                                CloseAction="CloseButton" Width="950px" Height="650px" PopupHorizontalAlign="WindowCenter"
                                                PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                                AllowResize="true" ShowShadow="true"
                                                OnWindowCallback="PopupControlIssueDocumentPos_WindowCallback">
                                                <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                                <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="800" VerticalAlign="WindowCenter" MinHeight="285px" MaxWidth="680px" MaxHeight="285px" />
                                                <ContentStyle BackColor="#F7F7F7">
                                                    <Paddings Padding="0px"></Paddings>
                                                </ContentStyle>
                                            </dx:ASPxPopupControl>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </div>
                        </div>

                        <div class="row m-0 pt-2">
                            <div class="col-6">
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

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnCompleteIssueDocument" runat="server" Text="Zaključi izdajnico" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="btnCompleteIssueDocument" UseSubmitBehavior="false" ClientVisible="false" OnClick="btnCompleteIssueDocument_Click">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/completeIssue.png" UrlHottracked="../Images/completeIssueHover.png" />
                                        <ClientSideEvents Click="btnCompleteIssueDocument_Click" />
                                    </dx:ASPxButton>
                                </span>
                            </div>
                            <div class="col-6 text-right">
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="btnAdd" UseSubmitBehavior="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/btnAddPopup.png" UrlHottracked="../Images/btnAddPopupHover.png" />
                                        <ClientSideEvents Click="HandleUserPopupAction" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                                        Height="25" Width="110" ClientInstanceName="btnEdit" UseSubmitBehavior="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/btnEditPopup.png" UrlHottracked="../Images/btnEditPopupHover.png" />
                                        <ClientSideEvents Click="HandleUserPopupAction" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                                        Height="25" Width="110" UseSubmitBehavior="false" ClientInstanceName="btnDelete">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/btnRemovePopup.png" UrlHottracked="../Images/btnRemovePopupHover.png" />
                                        <ClientSideEvents Click="HandleUserPopupAction" />
                                    </dx:ASPxButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <dx:XpoDataSource ID="XpoDSIssueDocumentPosition" runat="server" ServerMode="true"
            DefaultSorting="IssueDocumentPositionID DESC" TypeName="ETT_DAL.ETTPotocnik.IssueDocumentPosition" Criteria="[IssueDocumentID] = ?">
            <CriteriaParameters>
                <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
            </CriteriaParameters>
        </dx:XpoDataSource>

        <dx:XpoDataSource ID="XpoDSBuyer" runat="server" ServerMode="true"
            DefaultSorting="ClientID" TypeName="ETT_DAL.ETTPotocnik.Client" Criteria="[ClientTypeID] = 2">
        </dx:XpoDataSource>
        <dx:XpoDataSource ID="XpoDSLocation" runat="server" ServerMode="true"
            DefaultSorting="LocationID" TypeName="ETT_DAL.ETTPotocnik.Location">
        </dx:XpoDataSource>
    </div>

    <div class="modal fade" id="ConfirmModal" tabindex="-1" role="dialog" aria-hidden="true"
        data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header justify-content-center bg-warning">
                    <i class="far fa-question-circle" style="color: white; font-size: 70px;"></i>
                    <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>--%>
                </div>
                <div class="modal-body text-center">
                    <h5 class="modal-title" id="ConfirmModalTitle"></h5>
                    <p id="ConfirmModalBody"></p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" id="submitIssue" class="btn btn-success">Da, želim zaključiti</button>
                    <button type="button" id="Cancel" class="btn btn-secondary" data-dismiss="modal">Prekliči</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
