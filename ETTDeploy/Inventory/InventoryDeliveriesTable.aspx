<%@ Page Title="Iskanje artiklov" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="InventoryDeliveriesTable.aspx.cs" Inherits="ETT_Web.Inventory.InventoryDeliveriesTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function HandleUserAction(s, e) {
            LoadingPanel.Show();
            //vrne vrednost ki predstavlja akcijo uporabnika (add, edit, delete)
            var actionParameter = HandleUserActionsOnTabs(gridInventoryDeliveries, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                gridInventoryDeliveries.PerformCallback(actionParameter);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <dx:ASPxGridView ID="ASPxGridViewInventoryDeliveries"  runat="server" KeyFieldName="InventoryDeliveriesID" DataSourceID="XpoDSInventoryDeliveries"
        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridInventoryDeliveries"
        OnDataBound="ASPxGridViewInventoryDeliveries_DataBound" OnCustomCallback="ASPxGridViewInventoryDeliveries_CustomCallback">
        <ClientSideEvents RowDblClick="HandleUserAction" />
        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
            AllowHideDataCellsByColumnMinWidth="true">
        </SettingsAdaptivity>
        <SettingsBehavior AllowEllipsisInText="true" />
        <Paddings Padding="0" />
        <Settings ShowVerticalScrollBar="True"
            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="600"
            ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
        <SettingsPager PageSize="50" ShowNumericButtons="true">
            <PageSizeItemSettings Visible="true" Items="200,250,300,500,700,1000" Caption="Zapisi na stran : " AllItemText="Vsi">
            </PageSizeItemSettings>
            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
        </SettingsPager>
        <SettingsBehavior AllowFocusedRow="true" />
        <Styles>
            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
            <Cell Wrap="False" />
        </Styles>
        <SettingsText EmptyDataRow="Trenutno ni podatka o artiklih." />

        <Columns>
            <dx:GridViewDataDateColumn Caption="Datum" FieldName="tsInsert" AllowTextTruncationInAdaptiveMode="true" MinWidth="100" MaxWidth="200" Width="15%">
                <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy hh:mm:ss" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Komercialno ime eksploziva" FieldName="InventoryStockID.ProductID.Name" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="30%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="UID koda" FieldName="AtomeUID250" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="30%" >
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Packages" FieldName="PackagesUIDs" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="30%" >
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Koda proizvajalca" FieldName="SupplierProductCode" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="20%" >
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Zadnja lokacija" FieldName="LastLocationID.Name" AdaptivePriority="2" MinWidth="200" MaxWidth="250" Width="20%" >
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Datum zadnje spremembe" FieldName="tsUpdate" AllowTextTruncationInAdaptiveMode="true" MinWidth="100" MaxWidth="200" Width="15%">
                <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy  hh:mm:ss" />
            </dx:GridViewDataDateColumn>
        </Columns>
    </dx:ASPxGridView>

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
                Height="25" Width="90" ClientInstanceName="btnAdd">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                <ClientSideEvents Click="HandleUserAction" />
            </dx:ASPxButton>

            <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                Height="25" Width="90" ClientInstanceName="btnEdit">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <Image Url="../Images/edit.png" UrlHottracked="../Images/editHover.png" />
                <ClientSideEvents Click="HandleUserAction" />
            </dx:ASPxButton>
        </div>
    </div>

    <dx:XpoDataSource ID="XpoDSInventoryDeliveries" runat="server" ServerMode="true" 
        DefaultSorting="InventoryDeliveriesID DESC" TypeName="ETT_DAL.ETTPotocnik.InventoryDeliveries">
    </dx:XpoDataSource>
</asp:Content>
