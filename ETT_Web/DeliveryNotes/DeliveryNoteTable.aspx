<%@ Page Title="Elektronske dobavnice" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="DeliveryNoteTable.aspx.cs" Inherits="ETT_Web.DeliveryNotes.DeliveryNoteTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var notifyProcessing = GetUrlQueryStrings()['notifyProcessing'];
             
            if (notifyProcessing) {
                $("#notifyProcessingModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.notifyProcessing;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
        });


        function RowDoubleClick(s, e) {
             gridDeliveryNote.GetRowValues(gridDeliveryNote.GetFocusedRowIndex(), 'DeliveryNoteID', OnGetRowValues);
            
        }

        function OnGetRowValues(value) {
            gridDeliveryNote.PerformCallback("DoubleClick;" + value);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxGridView ID="ASPxGridViewDeliveryNote" Width="100%" runat="server" KeyFieldName="DeliveryNoteID" DataSourceID="XpoDSDeliveryNote"
        CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridDeliveryNote"
        OnDataBound="ASPxGridViewDeliveryNote_DataBound" OnCustomCallback="ASPxGridViewDeliveryNote_CustomCallback" OnHtmlRowPrepared="ASPxGridViewDeliveryNote_HtmlRowPrepared">
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o dobavnicah. Dodaj novo." />

        <Columns>
            <dx:GridViewDataDateColumn Caption="Datum dobavnice" FieldName="DeliveryNoteDate" AllowTextTruncationInAdaptiveMode="true" MinWidth="100" MaxWidth="200" Width="10%">
                <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Štev. dobavnice" FieldName="DeliveryNoteNumber" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="10%" />
            <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="SupplierID.Name" AdaptivePriority="2" MinWidth="200" MaxWidth="250" Width="20%" />
            <dx:GridViewDataTextColumn Caption="Prevzem v skladišče" FieldName="LocationID.Name" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="20%" />
            <dx:GridViewDataHyperLinkColumn Caption="Datoteka" FieldName="XMLFilePath" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%">
                <PropertiesHyperLinkEdit NavigateUrlFormatString="{0}" TextFormatString="Datoteka naložena" />
            </dx:GridViewDataHyperLinkColumn>
            <dx:GridViewDataTextColumn Caption="Status" FieldName="DeliveryNoteStatusID.Name" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="15%" />
            <dx:GridViewDataTextColumn Caption="Status" FieldName="DeliveryNoteStatusID.Code" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="15%" Visible="false" />
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
    <dx:XpoDataSource ID="XpoDSDeliveryNote" runat="server" ServerMode="true"
        DefaultSorting="DeliveryNoteID DESC" TypeName="ETT_DAL.ETTPotocnik.DeliveryNote">
    </dx:XpoDataSource>

    <!-- Session end Modal -->
    <div id="notifyProcessingModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: yellow; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <div class="w-100"><i class="material-icons" style="font-size: 48px; color: orange">warning</i></div>
                    <button type="button" class="close m-0 p-0" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body text-center">
                    <h3>Elektronska dobavnica v obdelavi!</h3>
                    <p>Elektronsko dobavnico ste uspšeno poslali v obdelavo. Zaradi same velikosti lahko obdelava traja dlje časa. Ko se obdelava zaključi se spremeni status na dobavnici.</p>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
