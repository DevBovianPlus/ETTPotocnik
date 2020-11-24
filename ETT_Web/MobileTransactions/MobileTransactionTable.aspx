<%@ Page Title="Mobilne transakcije" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="MobileTransactionTable.aspx.cs" Inherits="ETT_Web.MobileTransactions.MobileTransactionTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CallbackPanelMobileTransaction_EndCallback(s, e) {
        }

        function HandleUserAction(s, e) {
            //vrne vrednost ki predstavlja akcijo uporabnika (add, edit, delete)
            var actionParameter = HandleUserActionsOnTabs(gridMobileTransaction, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                CallbackPanelMobileTransaction.PerformCallback(actionParameter);
            }
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'MobileTransaction':
                            PopupControlMobileTransaction.Hide();
                            CallbackPanelMobileTransaction.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'MobileTransaction':
                            PopupControlMobileTransaction.Hide();
                    }
                    break;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelMobileTransaction" runat="server" OnCallback="CallbackPanelMobileTransaction_Callback" ClientInstanceName="CallbackPanelMobileTransaction">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="CallbackPanelMobileTransaction_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewMobileTransaction" Width="100%" runat="server" KeyFieldName="MobileTransactionID" DataSourceID="XpoDSMobileTransaction"
                    CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridMobileTransaction" OnCustomColumnDisplayText="ASPxGridViewMobileTransaction_CustomColumnDisplayText"
                    OnDataBound="ASPxGridViewMobileTransaction_DataBound ">
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
                    <SettingsText EmptyDataRow="Trenutno ni podatka o mobilnih transakcijah. Dodaj novo." />

                    <Columns>
                        

                        <dx:GridViewDataTextColumn Caption="Prenešeno" FieldName="InventoryDeliveriesLocationID.NeedsMatching"  MinWidth="70" MaxWidth="250" Width="2%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn Caption="Datum" FieldName="tsInsert" AllowTextTruncationInAdaptiveMode="true" MinWidth="80" MaxWidth="200" Width="5%">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy HH:mm:ss" />
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Uporabnik" MinWidth="100" MaxWidth="200" Width="5%"
                            FieldName="UserFirstnameAndLastname" UnboundType="String" UnboundExpression="[InventoryDeliveriesLocationID.UserID.EmployeeID.Firstname] + ' ' + [InventoryDeliveriesLocationID.UserID.EmployeeID.Lastname]">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Iz lokacije" FieldName="InventoryDeliveriesLocationID.LocationFromID.Name"  MinWidth="200" MaxWidth="250" Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Na lokacijo" FieldName="InventoryDeliveriesLocationID.LocationToID.Name"  MinWidth="200" MaxWidth="250" Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="SupplierID.Name"  MinWidth="200" MaxWidth="250" Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Artikel" FieldName="ProductID.Name"  MinWidth="200" MaxWidth="250" Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Koda artikla" FieldName="UIDCode" MinWidth="150" MaxWidth="250" Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>

                <dx:ASPxPopupControl ID="PopupControlMobileTransaction" runat="server" ContentUrl="MobileTransaction_popup.aspx"
                    ClientInstanceName="PopupControlMobileTransaction" Modal="True" HeaderText="MOBILNA TRANSAKCIJA"
                    CloseAction="CloseButton" Width="740px" Height="450px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="PopupControlMobileTransaction_WindowCallback">
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
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/trash.png" UrlHottracked="../Images/trashHover.png" />
                            <ClientSideEvents Click="HandleUserAction" />
                        </dx:ASPxButton>
                    </div>
                    <div class="col-sm-3 text-right">
                        <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                            Height="25" Width="90" ClientInstanceName="btnAdd" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                            <ClientSideEvents Click="HandleUserAction" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                            Height="25" Width="90" ClientInstanceName="btnEdit" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/edit.png" UrlHottracked="../Images/editHover.png" />
                            <ClientSideEvents Click="HandleUserAction" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:XpoDataSource ID="XpoDSMobileTransaction" runat="server" ServerMode="true"
        DefaultSorting="MobileTransactionID DESC" TypeName="ETT_DAL.ETTPotocnik.MobileTransaction">
    </dx:XpoDataSource>
</asp:Content>
