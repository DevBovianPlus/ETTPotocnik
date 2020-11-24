<%@ Page Title="Kategorija" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Users_popup.aspx.cs" Inherits="ETT_Web.Employees.Users_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtUsername, clientTxtPassword];
            var lookupItems = [lookUpEmployee, lookUpRole];

            process = InputFieldsValidation(lookupItems, inputItems, null, null, null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;
        }


        function showHidePassword() {
            var x = document.getElementById("ContentPlaceHolder1_txtPassword_I");

            if (x.type === "password") {
                x.type = "text";
                $('.icon-show-hide-pass').css("background-image", "url('/Images/hidePass.png')");
            } else {
                x.type = "password";
                $('.icon-show-hide-pass').css("background-image", "url('/Images/showPass.png')");
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 10px;">
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="ZAPOSLEN : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col-5 no-padding-left">
            <dx:ASPxGridLookup ID="GridLookupEmployee" runat="server" ClientInstanceName="lookUpEmployee"
                KeyFieldName="EmployeeID" TextFormatString="{0} {1}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" IncrementalFilteringMode="Contains" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter">
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
                <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Zaposleni" SwitchToModalAtWindowInnerWidth="650" />
                <ClientSideEvents Init="SetFocus" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname"
                        ReadOnly="true" MinWidth="230" MaxWidth="300" Width="30%">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Priimek"
                        FieldName="Lastname" Width="30%" AdaptivePriority="2" MinWidth="200" MaxWidth="300">
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

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 19px;">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="UPO. IME : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col-5 no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtUsername" ClientInstanceName="clientTxtUsername"
                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="30" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 35px;">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="GESLO : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtPassword" ClientInstanceName="clientTxtPassword"
                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="30" AutoCompleteType="Disabled" Password="true">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
        <div class="col no-padding-left">
            <div class="icon-show-hide-pass" onclick="showHidePassword()"></div>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 32px;">
            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="VLOGA : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col-5 no-padding-left">
            <dx:ASPxGridLookup ID="GridLookupRole" runat="server" ClientInstanceName="lookUpRole"
                KeyFieldName="RoleID" TextFormatString="{0}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSRole" IncrementalFilteringMode="Contains" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter">
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
                <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Vloga" SwitchToModalAtWindowInnerWidth="650" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                        ReadOnly="true" MinWidth="230" MaxWidth="400" Width="40%">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Opis"
                        FieldName="Description" Width="60%" AdaptivePriority="2" MinWidth="200" MaxWidth="300">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 28px;">
            <dx:ASPxLabel ID="ASPxLabel15" runat="server" Font-Size="12px" Text="DOSTOP : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col pl-0">
            <dx:ASPxCheckBox ID="chbxGrantAccess" runat="server" ToggleSwitchDisplayMode="Always"></dx:ASPxCheckBox>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 28px;">
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="OPOMBE : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="3" MaxLength="150" CssClass="text-box-input" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxMemo>
        </div>
    </div>

    <div class="row m-0 text-right">
        <div class="col-12">
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnConfirmPopup" runat="server" Text="Potrdi" AutoPostBack="false"
                    ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopup_Click" UseSubmitBehavior="false"
                    Width="100px">
                    <ClientSideEvents Click="CheckFieldValidation" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnCancelPopup" runat="server" Text="Prekliči" AutoPostBack="false"
                    ClientInstanceName="clientBtnCancel" OnClick="btnCancelPopup_Click" UseSubmitBehavior="false"
                    Width="100px">
                    <Image Url="../Images/cancelPopUp.png" UrlHottracked="../Images/cancelHover.png" Width="24"></Image>
                </dx:ASPxButton>
            </span>
        </div>
    </div>

    <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="true"
        DefaultSorting="EmployeeID" TypeName="ETT_DAL.ETTPotocnik.Employee">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSRole" runat="server" ServerMode="true"
        DefaultSorting="RoleID" TypeName="ETT_DAL.ETTPotocnik.Role">
    </dx:XpoDataSource>

</asp:Content>
