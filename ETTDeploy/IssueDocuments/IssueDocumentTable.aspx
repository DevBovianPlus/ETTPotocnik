<%@ Page Title="Izdajnice" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="IssueDocumentTable.aspx.cs" Inherits="ETT_Web.IssueDocuments.IssueDocumentTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function RowDoubleClick(s, e) {
             gridIssueDocument.GetRowValues(gridIssueDocument.GetFocusedRowIndex(), 'IssueDocumentID', OnGetRowValues);
            
        }

        function OnGetRowValues(value) {
            gridIssueDocument.PerformCallback("DoubleClick;" + value);
        }

        function btnPrintSelected_Click(s, e)
        {
            gridIssueDocument.GetRowValues(gridIssueDocument.GetFocusedRowIndex(), 'IssueDocumentID;IssueStatus.Code', OnGetPrintRowValues);
        }

        function OnGetPrintRowValues(value) {
            var status = '<%= ETT_Utilities.Common.Enums.IssueDocumentStatus.ZAKLJUCENO.ToString() %>';
            if(value[1] === status)
                gridIssueDocument.PerformCallback("PrintDocument;" + value[0]);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxGridView ID="ASPxGridViewIssueDocument" Width="100%" runat="server" KeyFieldName="IssueDocumentID" DataSourceID="XpoDSIssueDocument"
        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridIssueDocument"
        OnDataBound="ASPxGridViewIssueDocument_DataBound" OnCustomCallback="ASPxGridViewIssueDocument_CustomCallback">
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o izdajnicah. Dodaj novo." />

        <Columns>
            <dx:GridViewDataDateColumn Caption="Datum izdajnice" FieldName="IssueDate" AllowTextTruncationInAdaptiveMode="true" MinWidth="100" MaxWidth="200" Width="10%">
                <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Štev. izdajnice" FieldName="IssueNumber" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Status" FieldName="IssueStatus.Name" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Status" FieldName="IssueStatus.Code" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" Visible="false" />
            <dx:GridViewDataTextColumn Caption="Kupec" FieldName="BuyerID.Name" AdaptivePriority="2" MinWidth="200" MaxWidth="250" Width="20%" />
            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="20%" />
            <dx:GridViewDataTextColumn Caption="Interni dokument" FieldName="InternalDocument" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Štev. računa" FieldName="InvoiceNumber" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Dovoljenje za prodajo" FieldName="PermissionDoc" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
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
            <dx:ASPxButton ID="btnPrintSelected" runat="server" Text="Natisni" AutoPostBack="false"
                Height="25" Width="90" ClientInstanceName="btnPrintSelected">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <Image Url="../Images/print.png" UrlHottracked="../Images/printHover.png" />
                <ClientSideEvents Click="btnPrintSelected_Click" />
            </dx:ASPxButton>

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
    <dx:XpoDataSource ID="XpoDSIssueDocument" runat="server" ServerMode="true"
        DefaultSorting="IssueDocumentID DESC" TypeName="ETT_DAL.ETTPotocnik.IssueDocument">
    </dx:XpoDataSource>
</asp:Content>
