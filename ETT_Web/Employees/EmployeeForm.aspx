<%@ Page Title="Zaposleni" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="EmployeeForm.aspx.cs" Inherits="ETT_Web.Employees.EmployeeForm" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ Register TagPrefix="widget" TagName="ImageUpload" Src="../Widgets/ImageUploadWidget.ascx" %>

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
            var actionParameter = HandleUserActionsOnTabs(gridUsers, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                callbackPanel.PerformCallback(actionParameter);
            }
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
        <li class="nav-item" runat="server" id="userCredentialsItem">
            <a class="nav-link" data-toggle="tab" href="#UserCredentials"><span runat="server" id="userCredentialsBadge" class="badge badge-pill badge-info">0</span> Dostopni podatki</a>
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
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="IME : *" Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="clientTxtName"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="70" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <ClientSideEvents Init="SetFocus" />
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0 mr-3">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="PRIIMEK : " Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxTextBox runat="server" ID="txtLastName" ClientInstanceName="clientTxtLastName"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="80" AutoCompleteType="Disabled">
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
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="DATUM ROJ. : " Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxDateEdit ID="DateEditBirthDate" runat="server" EditFormat="Date" Width="100%"
                                                    CssClass="text-box-input date-edit-padding" Font-Size="13px">
                                                    <FocusedStyle CssClass="focus-text-box-input" />
                                                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                    <DropDownButton Visible="true"></DropDownButton>
                                                </dx:ASPxDateEdit>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6 mb-2 mb-lg-0">
                                        <div class="row m-0 align-items-center justify-content-center">
                                            <div class="col-0 p-0" style="margin-right: 15px;">
                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="NASLOV : " Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxTextBox runat="server" ID="txtAddress" ClientInstanceName="clientTxtAddress"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="15" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row m-0 pb-3">
                                    <div class="col-lg-6 mb-2 mb-lg-0">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0" style="margin-right: 48px;">
                                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="POŠTA : " Font-Bold="true"></dx:ASPxLabel>
                                            </div>
                                            <div class="col p-0">
                                                <dx:ASPxTextBox runat="server" ID="txtPostcode" ClientInstanceName="clientTxtPostcode"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6">
                                        <div class="row m-0 align-items-center justify-content-end">
                                            <div class="col-0 p-0" style="margin-right: 33px;">
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
                                    <div class="col-lg-6 mb-2 mb-lg-0">
                                        <div class="row m-0 align-items-center justify-content-center">
                                            <div class="col-0 p-0" style="margin-right: 50px;">
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

                                    <div class="col-lg-6">
                                        <div class="row m-0 align-items-center justify-content-end">
                                            <div class="col-0 p-0 " style="margin-right: 11px;">
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
                                    <div class="col-lg-12">
                                        <div class="row m-0 align-items-center">
                                            <div class="col-0 p-0" style="margin-right:35px;">
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

                            </div>

                            <div class="col-lg-4">
                                <widget:ImageUpload ID="UploadProfile" runat="server" OnImageUpdated="UploadProfile_ImageUpdated" ImageType="Profile" />
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

        <div id="UserCredentials" class="container-fluid tab-pane fade">
            <div class="card">
                <div class="card-header" style="background-color: #FAFCFE">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6>Dostopni podatki</h6>
                        <a data-toggle="collapse" href="#collapseUserCredentials" aria-expanded="true" aria-controls="collapseUserCredentials"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                    </div>
                </div>
                <div class="collapse show" id="collapseUserCredentials">
                    <div class="card-body p-0" style="background-color: #eef2f5d6;">
                        <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
                            ClientInstanceName="callbackPanel">
                            <PanelCollection>
                                <dx:PanelContent>

                                    <dx:ASPxGridView ID="ASPxGridViewUsers" Width="100%" runat="server" KeyFieldName="UserID" DataSourceID="XpoDSUsers"
                                        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridUsers"
                                        OnDataBound="ASPxGridViewUsers_DataBound">
                                        <ClientSideEvents RowDblClick="HandleUserAction" />
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                                            AllowHideDataCellsByColumnMinWidth="true">
                                        </SettingsAdaptivity>
                                        <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Auto" />
                                        <SettingsBehavior AllowEllipsisInText="true" />
                                        <Paddings Padding="0" />
                                        <SettingsPager PageSize="50" >
                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                        </SettingsPager>
                                        <SettingsBehavior AllowFocusedRow="true" />
                                        <Styles>
                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                            <Cell Wrap="False" />
                                        </Styles>
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o dostopu. Dodaj novega." />

                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Upo. ime" FieldName="Username" AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="400" Width="30%" />
                                            <dx:GridViewDataTextColumn Caption="Vloga" FieldName="RoleID.Name" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="20%" />
                                            <dx:GridViewDataCheckColumn Caption="ETT dostop" FieldName="GrantAppAccess" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
                                            <dx:GridViewDataTextColumn Caption="Opombe" FieldName="Notes" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="40%" />
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

                        <dx:XpoDataSource ID="XpoDSUsers" runat="server" ServerMode="true"
                            DefaultSorting="UserID DESC" TypeName="ETT_DAL.ETTPotocnik.Users" Criteria="[EmployeeID] = ?">
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
