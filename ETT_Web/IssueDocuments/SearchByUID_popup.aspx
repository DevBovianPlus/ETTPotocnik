<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="SearchByUID_popup.aspx.cs" Inherits="ETT_Web.IssueDocuments.SearchByUID_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var isRequestInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [txtUID250];
            var lookupItmes = [];
            process = InputFieldsValidation(lookupItmes, inputItems, null, null, null, null);

            if (s.GetText() == 'Izbriši')
                process = true;

            if (process && !isRequestInitiated) {
                LoadingPanel.Show();
                isRequestInitiated = true;
                e.processOnServer = true;
            }
            else
                e.processOnServer = false;
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxCallbackPanel ID="CallbakcPanel" runat="server" ClientInstanceName="CallbackPanel" OnCallback="CallbakcPanel_Callback">
        <PanelCollection>
            <dx:PanelContent>

                <div class="card">
                    <div class="card-body p-0">
                        <div class="row m-0 pb-3 pt-3">
                            <div class="col-lg-2">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="UID : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col-8 p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtUID250" ClientInstanceName="txtUID250"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="20" AutoCompleteType="Disabled"
                                            ClientEnabled="false" BackColor="LightGray">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col">
                                <dx:ASPxGridView ID="ASPxGridViewInventoryDeliveries" runat="server" KeyFieldName="InventoryDeliveriesID" DataSourceID="XpoDSInventoryDeliveries"
                                    CssClass="gridview-no-header-padding mw-100" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridInventoryDeliveries"
                                    OnDataBound="ASPxGridViewInventoryDeliveries_DataBound" Width="100%">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                                        AllowHideDataCellsByColumnMinWidth="true">
                                    </SettingsAdaptivity>
                                    <SettingsBehavior AllowEllipsisInText="true" />
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="250"
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="200,250,300" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectSingleRowOnly="true" />
                                    <Styles>
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                        <Cell Wrap="False" />
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o artiklih." />

                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="60px" />
                                        <dx:GridViewDataDateColumn Caption="Datum" FieldName="tsInsert" AllowTextTruncationInAdaptiveMode="true" MinWidth="100" MaxWidth="100" Width="15%">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Caption="Artikel" FieldName="InventoryStockID.ProductID.Name" AdaptivePriority="1" MinWidth="100" MaxWidth="150" Width="20%" />
                                        <dx:GridViewDataTextColumn Caption="UID koda" FieldName="AtomeUID250" AdaptivePriority="1" MinWidth="100" MaxWidth="150" Width="20%" />
                                        <%--<dx:GridViewDataTextColumn Caption="Koda proizvajalca" FieldName="SupplierProductCode" AdaptivePriority="1" MinWidth="100" MaxWidth="200" Width="30%" />--%>
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pt-2 justify-content-end">
                    <div class="col-6 text-right">
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                                Height="25" Width="110" ClientInstanceName="btnConfirm" UseSubmitBehavior="false" OnClick="btnConfirm_Click">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../Images/btnAddPopup.png" UrlHottracked="../Images/btnAddPopupHover.png" />
                                <ClientSideEvents Click="CheckFieldValidation" />
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                                Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../Images/cancelPopUp.png" UrlHottracked="../Images/cancelPopUp.png" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:XpoDataSource ID="XpoDSInventoryDeliveries" runat="server" ServerMode="true"
        DefaultSorting="InventoryDeliveriesID DESC" TypeName="ETT_DAL.ETTPotocnik.InventoryDeliveries" Criteria="Contains([PackagesUIDs], ?)">
        <CriteriaParameters>
            <asp:SessionParameter Name="SearchUIDValue" SessionField="SearchUIDValue" />
        </CriteriaParameters>
    </dx:XpoDataSource>

</asp:Content>
