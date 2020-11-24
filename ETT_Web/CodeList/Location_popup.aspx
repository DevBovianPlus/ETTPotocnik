<%@ Page Title="Lokacija" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Location_popup.aspx.cs" Inherits="ETT_Web.CodeList.Location_popup" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtName];

            process = InputFieldsValidation(null, inputItems, null, null, null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 21px;">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="NAZIV : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="clientTxtName"
                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="150" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                <ClientSideEvents Init="SetFocus" />
            </dx:ASPxTextBox>
        </div>
    </div>
    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 16px;">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="JE KUPEC : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxCheckBox ID="chbxIsBuyer" runat="server" ToggleSwitchDisplayMode="Always">
            </dx:ASPxCheckBox>
        </div>
    </div>
     <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 16px;">
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="JE SKLADIŠČE : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxCheckBox ID="CheckBoxIsWarehouse" runat="server" ToggleSwitchDisplayMode="Always">
            </dx:ASPxCheckBox>
        </div>
    </div>
    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0 mr-3">
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="OPOMBE : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="3" MaxLength="300" CssClass="text-box-input" AutoCompleteType="Disabled">
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
</asp:Content>
