<%@ Page Title="Pregled zaloge" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="InventoryStockTable.aspx.cs" Inherits="ETT_Web.Inventory.InventoryStockTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CallbackPanelInventoryStock_EndCallback(s, e) {
        }

        function HandleUserAction(s, e) {
            //vrne vrednost ki predstavlja akcijo uporabnika (add, edit, delete)
            var actionParameter = HandleUserActionsOnTabs(gridInventoryStock, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                CallbackPanelInventoryStock.PerformCallback(actionParameter);
            }
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'InventoryStock':
                            popupControMeasuringUnit.Hide();
                            CallbackPanelInventoryStock.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'InventoryStock':
                            popupControMeasuringUnit.Hide();
                    }
                    break;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelInventoryStock" runat="server" OnCallback="CallbackPanelInventoryStock_Callback" ClientInstanceName="CallbackPanelInventoryStock">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="CallbackPanelInventoryStock_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewInventoryStock" Width="100%" runat="server" KeyFieldName="InventoryStockID" DataSourceID="XpoDSInventoryStock"
                    CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridInventoryStock"
                    OnDataBound="ASPxGridViewInventoryStock_DataBound">
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
                    <SettingsText EmptyDataRow="Trenutno ni podatka o zalogah artiklov. Dodaj novo." />

                    <Columns>
                        <dx:GridViewDataDateColumn Caption="Datum" FieldName="tsInsert" AllowTextTruncationInAdaptiveMode="true" MinWidth="100" MaxWidth="200" Width="12%" Visible="false">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="PSN" FieldName="ProductID.PSN" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="7%" />
                        <dx:GridViewDataTextColumn Caption="Komercialno ime eksploziva" FieldName="ProductID.Name" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="35%" />
                        <dx:GridViewDataTextColumn Caption="Lokacija" FieldName="LocationID.Name" AdaptivePriority="2" MinWidth="200" MaxWidth="250" Width="20%" />
                        <dx:GridViewDataTextColumn Caption="Količina" FieldName="Quantity" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="12%" 
                            PropertiesTextEdit-DisplayFormatString="N2" />
                        <dx:GridViewDataTextColumn Caption="Merska enota" FieldName="ProductID.MeasuringUnitID.Name" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="10%" />
                        <dx:GridViewDataTextColumn Caption="Količina (kos)" FieldName="QuantityPcs" AdaptivePriority="2" MinWidth="150" MaxWidth="250" Width="12%" />
                    </Columns>
                </dx:ASPxGridView>

                <dx:ASPxPopupControl ID="PopupControlInventoryStock" runat="server" ContentUrl="InventoryStock_popup.aspx"
                    ClientInstanceName="popupControMeasuringUnit" Modal="True" HeaderText="PREGLED ZALOGE"
                    CloseAction="CloseButton" Width="740px" Height="330px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="PopupControlInventoryStock_WindowCallback">
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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:XpoDataSource ID="XpoDSInventoryStock" runat="server" ServerMode="true"
        DefaultSorting="InventoryStockID DESC" TypeName="ETT_DAL.ETTPotocnik.InventoryStock">
    </dx:XpoDataSource>
</asp:Content>
