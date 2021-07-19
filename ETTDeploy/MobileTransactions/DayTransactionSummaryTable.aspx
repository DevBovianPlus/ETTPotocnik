<%@ Page Title="Dnevne transakcije - Sumarno" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="DayTransactionSummaryTable.aspx.cs" Inherits="ETT_Web.MobileTransactions.DayTransactionSummaryTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        function CallbackPanelSumarryTransaction_EndCallback(s, e) {
            LoadingPanel.Hide();
        }

        function AcceptFilterSum_Click(s, e) {
            LoadingPanel.Show();
            CallbackPanelSumarryTransaction.PerformCallback("AcceptFilter");
        }



        function ShowModal(title, message, successModal) {
            $('.modal-title').empty();
            $('.modal-title').append(title);

            $('.modal-body-desc').empty();
            $('.modal-body-desc').append(message);

            if (successModal)
                $('#modalSuccess').modal("show");
            else
                $('#modal').modal("show");
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="col-4">
        <div class="row m-12 align-items-center">
            <div class="col-0 pr-0">
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="Od: " Font-Bold="true"></dx:ASPxLabel>
            </div>
            <div class="col pl-4">
                <dx:ASPxDateEdit ID="dtDayOfTranasationOd" runat="server" EditFormat="Date" Width="100%"
                    CssClass="text-box-input date-edit-padding" Font-Size="13px"
                    ClientInstanceName="dateEditDayOfTranasation">
                    <FocusedStyle CssClass="focus-text-box-input" />
                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                    <DropDownButton Visible="true"></DropDownButton>
                    <ClientSideEvents Init="SetFocus" />
                </dx:ASPxDateEdit>
            </div>
            <div class="col-0 pr-0">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="Do: " Font-Bold="true"></dx:ASPxLabel>
            </div>
            <div class="col pl-4">
                <dx:ASPxDateEdit ID="dtDayOfTranasationDo" runat="server" EditFormat="Date" Width="100%" DisplayFormatString="dd.MM.yyyy"
                    CssClass="text-box-input date-edit-padding" Font-Size="13px"
                    ClientInstanceName="dateEditDayOfTranasation">
                    <FocusedStyle CssClass="focus-text-box-input" />
                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                    <DropDownButton Visible="true"></DropDownButton>
                    <ClientSideEvents Init="SetFocus" />
                </dx:ASPxDateEdit>
            </div>
            <div class="col pl-0">
                <dx:ASPxButton ID="btnPotrdi" runat="server" Text="Potrdi" AutoPostBack="false"
                    Height="25" Width="50" ClientInstanceName="btnPotrdi" OnClick="btnPotrdi_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../Images/search.png" UrlHottracked="../Images/searchHover.png" />
                    <ClientSideEvents Click="AcceptFilterSum_Click" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>

    <dx:ASPxCallbackPanel ID="CallbackPanelSumarryTransaction" runat="server" ClientInstanceName="CallbackPanelSumarryTransaction" OnCallback="CallbackPanelSumarryTransaction_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="CallbackPanelSumarryTransaction_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewSumarryTransaction" Width="100%" runat="server" KeyFieldName="SumarryTransactionID"
                    CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridSumarryTransaction"
                    OnDataBinding="ASPxGridViewSumarryTransaction_DataBinding" OnDataBound="ASPxGridViewSumarryTransaction_DataBound">
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                        AllowHideDataCellsByColumnMinWidth="true">
                    </SettingsAdaptivity>
                    <SettingsBehavior AllowEllipsisInText="true" />
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
                        VerticalScrollableHeight="600"
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
                        <dx:GridViewDataTextColumn FieldName="RowCnt" Visible="true" Caption="#" CellStyle-HorizontalAlign="Center" AllowTextTruncationInAdaptiveMode="true" MinWidth="80" MaxWidth="80" Width="1%">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Datum" FieldName="DateSum" AllowTextTruncationInAdaptiveMode="true" MinWidth="80" MaxWidth="200" Width="2%">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Iz lokacije" FieldName="IzLokacije" MinWidth="200" MaxWidth="250" Width="4%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Na lokacijo" FieldName="NaLokacijo" MinWidth="200" MaxWidth="250" Width="4%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Komercialno ime eksploziva" FieldName="Produkt" MinWidth="200" MaxWidth="250" Width="10%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Količina v kom" FieldName="QuantitySum" MinWidth="200" MaxWidth="250" Width="2%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Količina v kg" FieldName="QuantitySumKg" MinWidth="200" MaxWidth="250" Width="2%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>



    <div id="modal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: yellow; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <div class="w-100"><i class="material-icons" style="font-size: 48px; color: orange">warning</i></div>
                    <button type="button" class="close m-0 p-0" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body text-center">
                    <h3 class="modal-title"></h3>
                    <p class="modal-body-desc"></p>
                </div>
            </div>

        </div>
    </div>

    <div id="modalSuccess" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: lightgrey; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <div class="w-100"><i class="far fa-thumbs-up" style="font-size: 48px; color: limegreen"></i></div>
                    <button type="button" class="close m-0 p-0" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body text-center">
                    <h3 class="modal-title"></h3>
                    <p class="modal-body-desc"></p>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
