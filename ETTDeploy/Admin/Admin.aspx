<%@ Page Title="Administracija" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="ETT_Web.Admin.Admin" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function CallbackPanel_EndCallback(s, e) {
        }

        function HandleUserAction(s, e) {
            //vrne vrednost ki predstavlja akcijo uporabnika (add, edit, delete)
            var actionParameter = HandleUserActionsOnTabs(gridUsers, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                callbackPanel.PerformCallback(actionParameter);
            }
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'User':
                            popupControlUser.Hide();
                            callbackPanel.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'User':
                            popupControlUser.Hide();
                    }
                    break;
            }
        }

        function FilterHistory_CheckedChanged(s, e) {
            if (s.GetChecked()) {
                $('#periodFilter').animate({
                    opacity: 1,
                    height: "toggle"
                }, 250);
            }
            else {
                $('#periodFilter').animate({
                    opacity: 0,
                    height: "toggle"
                }, 250);
            }
        }

        function dateEdit_Init(s, e) {
            s.SetDate(new Date());
        }

        function btnRefreshGrid_Click(s, e) {
            callbackPanel.PerformCallback('FilerByPeriod');
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row m-0 pb-3 pt-3">
        <div class="col-4">
            <div class="row m-0 align-items-center">
                <div class="col-12 pr-0 mr-3 pb-2">
                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="Mobilne transakcije" Font-Bold="true"></dx:ASPxLabel>
                </div>
                <div class="col-12 pl-0">
                    <dx:ASPxButton ID="btnMatchMobileTransactions" runat="server" Text="Razknjiži transkacije" OnClick="btnMatchMobileTransactions_Click" 
                        AutoPostBack="false" UseSubmitBehavior="false"></dx:ASPxButton>
                </div>
            </div>
        </div>
        <div class="col-4">
            <div class="row m-0 align-items-center">
                <div class="col-12 pr-0 mr-3 pb-2">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="Pobriši podvojene transakcije" Font-Bold="true"></dx:ASPxLabel>
                </div>
                <div class="col-12 pl-0">
                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Briši transkacije" OnClick="btnDeleteTransactions_Click" 
                        AutoPostBack="false" UseSubmitBehavior="false"></dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
