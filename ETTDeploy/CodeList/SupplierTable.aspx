<%@ Page Title="Partnerji" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="SupplierTable.aspx.cs" Inherits="ETT_Web.CodeList.SupplierTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function RowDoubleClick(s, e) {
             gridSupplier.GetRowValues(gridSupplier.GetFocusedRowIndex(), 'ClientID', OnGetRowValues);
            
        }

        function OnGetRowValues(value) {
            gridSupplier.PerformCallback("DoubleClick;" + value);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxGridView ID="ASPxGridViewSupplier" Width="100%" runat="server" KeyFieldName="ClientID" DataSourceID="XpoDSSupplier"
        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridSupplier"
        OnDataBound="ASPxGridViewSupplier_DataBound" OnCustomCallback="ASPxGridViewSupplier_CustomCallback">
        <ClientSideEvents RowDblClick="RowDoubleClick" />
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o dobaviteljih. Dodaj novega." />

        <Columns>
            <dx:GridViewDataTextColumn Caption="Naziv podjetja" FieldName="Name" AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="400" Width="20%" />
            <dx:GridViewDataTextColumn Caption="Naslov" FieldName="Address" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Poštna štev." FieldName="Postcode" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Kraj" FieldName="PostName" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Poštna štev." FieldName="Postcode" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Država" FieldName="Country" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Telefon" FieldName="Phone" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Faks" FieldName="FAX" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
        </Columns>
    </dx:ASPxGridView>

    <div class="row m-0 mt-2">
        <div class="col-sm-9">
            <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                Height="25" Width="50" ClientInstanceName="btnDelete" OnClick="btnDelete_Click">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <Image Url="../Images/trash.png" UrlHottracked="../Images/trashHover.png" />
                <ClientSideEvents Click="btnDelete_Click" />
            </dx:ASPxButton>
        </div>
        <div class="col-sm-3 text-right">
            <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                Height="25" Width="90" ClientInstanceName="btnAdd" OnClick="btnAdd_Click">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                <ClientSideEvents Click="btnAdd_Click" />
            </dx:ASPxButton>

            <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                Height="25" Width="90" ClientInstanceName="btnEdit" OnClick="btnEdit_Click">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <Image Url="../Images/edit.png" UrlHottracked="../Images/editHover.png" />
                <ClientSideEvents Click="btnEdit_Click" />
            </dx:ASPxButton>
        </div>
    </div>
    <dx:XpoDataSource ID="XpoDSSupplier" runat="server" ServerMode="true"
        DefaultSorting="ClientID DESC" TypeName="ETT_DAL.ETTPotocnik.Client" Criteria="[ClientTypeID] = 1">
    </dx:XpoDataSource>
</asp:Content>
