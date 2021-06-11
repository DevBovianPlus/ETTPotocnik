<%@ Page Title="Partnerji" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="SupplierForm.aspx.cs" Inherits="ETT_Web.CodeList.SupplierForm" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var isRequestInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtName];

            process = InputFieldsValidation(null, inputItems, null, null, null, null);

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
            var actionParameter = HandleUserActionsOnTabs(gridContactPerson, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                callbackPanel.PerformCallback(actionParameter);
            }
        }

        function OnClosePopUpHandler(command, sender, contactsCount) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'ContactPerson':
                            popupControlContactPerson.Hide();

                            if (contactsCount > 0) {
                                $('#ContentPlaceHolderMain_contactPersonBadge').empty();
                                $('#ContentPlaceHolderMain_contactPersonBadge').append(contactsCount);

                            }

                            callbackPanel.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'ContactPerson':
                            popupControlContactPerson.Hide();
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
        <li class="nav-item" runat="server" id="contactPersonItem">
            <a class="nav-link" data-toggle="tab" href="#ContactPerson"><span runat="server" id="contactPersonBadge" class="badge badge-pill badge-info">0</span> Kontaktne osebe</a>
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
                            <div class="col-lg-3">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 pr-0" style="margin-right: 28px;">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="TIP STRANKE : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col pl-0">
                                        <dx:ASPxTextBox runat="server" ID="txtClientType" ClientInstanceName="clientClientType"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="100" AutoCompleteType="Disabled" ReadOnly="true" BackColor="LightGray">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-5 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 55px;">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="NAZIV : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="clientTxtName"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="150" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <ClientSideEvents Init="SetFocus" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-7">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="NAZIV DOLGI : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtLongName" ClientInstanceName="clientTxtLongName"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="250" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 54px;">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="NASLOV : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtAddress" ClientInstanceName="clientTxtAddress"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0" style="margin-right: 21px;">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="POŠTNA ŠTEV. : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtPostalcode" ClientInstanceName="clientTxtPostalcode"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="15" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4">
                                <div class="row m-0 align-items-center justify-content-end">
                                    <div class="col-0 p-0" style="margin-right: 66px;">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="KRAJ : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtCity" ClientInstanceName="clientTxtCity"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 54px;">
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="DRŽAVA : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtCountry" ClientInstanceName="clientTxtCountry"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0" style="margin-right: 67px;">
                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Size="12px" Text="EMAIL : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtEmail" ClientInstanceName="clientTxtEmail"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4">
                                <div class="row m-0 align-items-center justify-content-end">
                                    <div class="col-0 p-0 " style="margin-right: 45px;">
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Font-Size="12px" Text="TELEFON : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtPhone" ClientInstanceName="clientTxtPhone"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 80px">
                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Size="12px" Text="FAX : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtFax" ClientInstanceName="clientTxtFax"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0" style="margin-right: 81px;">
                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" Font-Size="12px" Text="TRR : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtBankAccount" ClientInstanceName="clientTxtBankAccount"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="40" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4">
                                <div class="row m-0 align-items-center justify-content-end">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" Font-Size="12px" Text="DAVČNA ŠTEV. : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtTaxNumber" ClientInstanceName="clientTxtTaxNumber"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" Font-Size="12px" Text="MATIČNA ŠTEV. : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtRegistrationNumber" ClientInstanceName="clientTxtRegistrationNumber"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center justify-content-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Font-Size="12px" Text="IDENTIFIKACIJA : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtIdentificationNumber" ClientInstanceName="clientTxtIdentificationNumber"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="40" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-3">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 pr-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel15" runat="server" Font-Size="12px" Text="DAVČNI ZAVEZANEC : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col pl-0">
                                        <dx:ASPxCheckBox ID="chbxTaxPayer" runat="server" ToggleSwitchDisplayMode="Always"></dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-3">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel16" runat="server" Font-Size="12px" Text="ČLAN EU : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxCheckBox ID="chbxEUMember" runat="server" ToggleSwitchDisplayMode="Always">
                                        </dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-12">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="OPOMBE : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="4" MaxLength="200" CssClass="text-box-input" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pt-5">
                            <div class="col-12 text-right">
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

        <div id="ContactPerson" class="container-fluid tab-pane fade">
            <div class="card">
                <div class="card-header" style="background-color: #FAFCFE">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6>Kontaktne osebe</h6>
                        <a data-toggle="collapse" href="#collapseContactPerson" aria-expanded="true" aria-controls="collapseBasicData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                    </div>
                </div>
                <div class="collapse show" id="collapseContactPerson">
                    <div class="card-body p-0" style="background-color: #eef2f5d6;">
                        <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
                            ClientInstanceName="callbackPanel">
                            <PanelCollection>
                                <dx:PanelContent>

                                    <dx:ASPxGridView ID="ASPxGridViewContactPerson" Width="100%" runat="server" KeyFieldName="ContactPersonID" DataSourceID="XpoDSContactPerson"
                                        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridContactPerson"
                                        OnDataBound="ASPxGridViewContactPerson_DataBound">
                                        <ClientSideEvents RowDblClick="HandleUserAction" />
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                                            AllowHideDataCellsByColumnMinWidth="true">
                                        </SettingsAdaptivity>
                                        <SettingsBehavior AllowEllipsisInText="true" />
                                        <Paddings Padding="0" />
                                        <Settings ShowVerticalScrollBar="True"
                                            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                            ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                        <SettingsPager PageSize="50" ShowNumericButtons="true">
                                            <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                            </PageSizeItemSettings>
                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                        </SettingsPager>
                                        <SettingsBehavior AllowFocusedRow="true" />
                                        <Styles>
                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                            <Cell Wrap="False" />
                                        </Styles>
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o kontaktnih osebah. Dodaj novo." />

                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name" AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="400" Width="30%" />
                                            <dx:GridViewDataTextColumn Caption="Telefon" FieldName="Phone" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Mobilna štev." FieldName="MobilePhone" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Fax" FieldName="FAX" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Opombe" FieldName="Notes" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="30%" />
                                        </Columns>
                                    </dx:ASPxGridView>

                                    <dx:ASPxPopupControl ID="PopupControlContactPerson" runat="server" ContentUrl="ContactPerson_popup.aspx"
                                        ClientInstanceName="popupControlContactPerson" Modal="True" HeaderText="KONTAKTNA OSEBA"
                                        CloseAction="CloseButton" Width="680px" Height="405px" PopupHorizontalAlign="WindowCenter"
                                        PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                        AllowResize="true" ShowShadow="true"
                                        OnWindowCallback="PopupControlContactPerson_WindowCallback">
                                        <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                        <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="800" VerticalAlign="WindowCenter" MinHeight="285px" MaxWidth="680px" MaxHeight="285px" />
                                        <ContentStyle BackColor="#F7F7F7">
                                            <Paddings Padding="0px"></Paddings>
                                        </ContentStyle>
                                    </dx:ASPxPopupControl>

                                    <div class="row m-0 mt-2">
                                        <div class="col-sm-9">
                                            <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                                                Height="25" Width="50" ClientInstanceName="btnDelete">
                                                <ClientSideEvents Click="HandleUserAction" />
                                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                                <Image Url="../Images/trash.png" UrlHottracked="../Images/trashHover.png" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-sm-3 text-right">
                                            <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                                                Height="25" Width="90" ClientInstanceName="btnAdd">
                                                <ClientSideEvents Click="HandleUserAction" />
                                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                                <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                                            </dx:ASPxButton>

                                            <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
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

                        <dx:XpoDataSource ID="XpoDSContactPerson" runat="server" ServerMode="true"
                            DefaultSorting="ContactPersonID DESC" TypeName="ETT_DAL.ETTPotocnik.ContactPerson" Criteria="[ClientID] = ?">
                            <CriteriaParameters>
                                <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
                            </CriteriaParameters>
                        </dx:XpoDataSource>

                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
