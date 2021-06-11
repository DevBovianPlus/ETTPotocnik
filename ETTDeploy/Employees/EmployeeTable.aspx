<%@ Page Title="Zaposleni" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="EmployeeTable.aspx.cs" Inherits="ETT_Web.Employees.EmployeeTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function RowDoubleClick(s, e) {
             gridEmployee.GetRowValues(gridEmployee.GetFocusedRowIndex(), 'EmployeeID', OnGetRowValues);
            
        }

        function OnGetRowValues(value) {
            gridEmployee.PerformCallback("DoubleClick;" + value);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxGridView ID="ASPxGridViewEmployee" Width="100%" runat="server" KeyFieldName="EmployeeID" DataSourceID="XpoDSEmployee"
        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridEmployee"
        OnDataBound="ASPxGridViewEmployee_DataBound" OnCustomCallback="ASPxGridViewEmployee_CustomCallback">
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o zaposlenih. Dodaj novega." />

        <Columns>
            <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="300" Width="15%" />
            <dx:GridViewDataTextColumn Caption="Priimek" FieldName="Lastname" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="15%" />
            <dx:GridViewDataDateColumn Caption="Datum roj." FieldName="BirthDate" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" PropertiesDateEdit-DisplayFormatString="dd. MMMM yyyy" />
            <dx:GridViewDataTextColumn Caption="Naslov" FieldName="Address" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="15%" />
            <dx:GridViewDataTextColumn Caption="Kraj" FieldName="Post" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Poštna štev." FieldName="Postcode" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Telefon" FieldName="Phone" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="15%" />
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
    <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="true"
        DefaultSorting="EmployeeID" TypeName="ETT_DAL.ETTPotocnik.Employee">
    </dx:XpoDataSource>
</asp:Content>
