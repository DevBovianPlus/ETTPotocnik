﻿<%@ Page Title="Kategorija" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Client_popup.aspx.cs" Inherits="ETT_Web.CodeList.Client_popup" %>

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
        <div class="col-0 pr-0" style="margin-right: 26px;">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="NAZIV : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="clientTxtName"
                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="150" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                <ClientSideEvents Init="SetFocus" />
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row m-0 align-items-center pb-2 d-none">
        <div class="col-0 pr-0" style="margin-right: 30px;">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DRŽAVA : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtCountry" ClientInstanceName="clientTxtCountry"
                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 12px;">
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="TELEFON : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col-8 no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtPhone" ClientInstanceName="clientTxtPhone"
                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 26px;">
            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="EMAIL : " Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col-8 no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtEmail" ClientInstanceName="clientTxtEmail"
                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right:10px;">
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
