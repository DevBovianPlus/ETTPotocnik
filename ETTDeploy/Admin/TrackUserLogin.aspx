<%@ Page Title="Uporabniki" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="TrackUserLogin.aspx.cs" Inherits="ETT_Web.Admin.TrackUserLogin" %>

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
                <div class="col-0 pr-0 mr-3">
                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="PREGLEJ ZGODOVINO : " Font-Bold="true"></dx:ASPxLabel>
                </div>
                <div class="col pl-0">
                    <dx:ASPxCheckBox ID="FilterThroughHistoryCheckBox" runat="server" ToggleSwitchDisplayMode="Always">
                        <ClientSideEvents CheckedChanged="FilterHistory_CheckedChanged" />
                    </dx:ASPxCheckBox>
                </div>
            </div>
        </div>
        <div class="col-lg-7" id="periodFilter" style="display: none;">
            <div class="row m-0 align-items-center">
                <div class="col-0 pr-0 mr-3">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DATUM OD : " Font-Bold="true"></dx:ASPxLabel>
                </div>
                <div class="col-3 pl-0">
                    <dx:ASPxDateEdit ID="DateEditDateFrom" runat="server" EditFormat="Date" Width="100%"
                        CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="clientDateEditDateFrom">
                        <FocusedStyle CssClass="focus-text-box-input" />
                        <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                        <DropDownButton Visible="true"></DropDownButton>
                        <ClientSideEvents Init="dateEdit_Init" />
                    </dx:ASPxDateEdit>
                </div>
                <div class="col-0 pr-0 mr-3">
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="DATUM DO : " Font-Bold="true"></dx:ASPxLabel>
                </div>
                <div class="col-3 pl-0">
                    <dx:ASPxDateEdit ID="DateEditDateTo" runat="server" EditFormat="Date" Width="100%"
                        CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="clientDateEditDateTo">
                        <FocusedStyle CssClass="focus-text-box-input" />
                        <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                        <DropDownButton Visible="true"></DropDownButton>
                        <ClientSideEvents Init="dateEdit_Init" />
                    </dx:ASPxDateEdit>
                </div>
                <div class="col-2">
                    <dx:ASPxButton ID="btnRefreshGrid" runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                        ClientInstanceName="clientBtnRefreshGrid" Text="Osveži" Width="100px">
                        <Paddings PaddingLeft="25px" PaddingRight="25px" PaddingTop="0px" PaddingBottom="0px" />
                        <ClientSideEvents Click="btnRefreshGrid_Click" />
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
        ClientInstanceName="callbackPanel">
        <ClientSideEvents EndCallback="CallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewActiveUser" Width="100%" runat="server" KeyFieldName="ActiveUserID"
                    CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridActiveUser"
                    OnDataBinding="ASPxGridViewActiveUser_DataBinding" OnCustomColumnDisplayText="ASPxGridViewActiveUser_CustomColumnDisplayText"
                    OnHtmlDataCellPrepared="ASPxGridViewActiveUser_HtmlDataCellPrepared">
                    <ClientSideEvents RowDblClick="HandleUserAction" />
                   <%-- <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                        AllowHideDataCellsByColumnMinWidth="true">
                    </SettingsAdaptivity>--%>
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
                    <SettingsText EmptyDataRow="Trenutno ni podatka o vpisu uporabnikov." />

                    <Columns>
                        <dx:GridViewBandColumn Caption="Zaposlen">
                            <Columns>
                                <dx:GridViewDataColumn FieldName="UserID.EmployeeID.Firstname" Caption="Ime" ReadOnly="true" Visible="true" Width="120px" ExportWidth="90"
                                    AdaptivePriority="1" MinWidth="120" MaxWidth="200">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="UserID.EmployeeID.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" Width="160px" ExportWidth="120"
                                    AdaptivePriority="1" MinWidth="160" MaxWidth="250">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataColumn>
                            </Columns>
                        </dx:GridViewBandColumn>
                        <dx:GridViewDataTextColumn Caption="Upo. ime" FieldName="UserID.Username"  Width="15%" /><%-- AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="200" --%>
                        <dx:GridViewDataTextColumn Caption="Email" FieldName="UserID.EmployeeID.Email" Width="15%" /><%-- AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="200"  --%>
                        <dx:GridViewDataTextColumn Caption="Vloga" FieldName="UserID.RoleID.Name" Width="20%" /><%-- AdaptivePriority="1" MinWidth="150" MaxWidth="250"  --%>

                        <dx:GridViewDataDateColumn FieldName="LoginDate" Caption="Datum prijave" Width="15%"><%-- AdaptivePriority="1" MinWidth="150" MaxWidth="250"  --%>
                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn Caption="Prijavljen" FieldName="IsActive" Width="10%" /><%-- AdaptivePriority="1" MinWidth="150" MaxWidth="250"  --%>


                        <dx:GridViewDataDateColumn FieldName="LastRequestTS" Caption="Zadnja zahteva" Width="15%"><%-- AdaptivePriority="1" MinWidth="150" MaxWidth="250"  --%>
                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn Caption="Število zahtev" FieldName="RequestCount" Width="10%" /><%-- AdaptivePriority="1" MinWidth="150" MaxWidth="250"  --%>

                        <dx:GridViewDataTextColumn Caption="Čas seje (min)" FieldName="SessionExpireMin" Width="10%" /><%-- AdaptivePriority="1" MinWidth="150" MaxWidth="250"  --%>
                    </Columns>
                    <SettingsDetail ShowDetailRow="true" />
                    <Templates>
                        <DetailRow>
                            <dx:ASPxGridView ID="ASPxGridViewUserActivity" runat="server" KeyFieldName="UserActivityID"
                                Width="100%" EnablePagingGestures="False" OnBeforePerformDataSelect="ASPxGridViewUserActivity_BeforePerformDataSelect"
                                OnDataBinding="ASPxGridViewUserActivity_DataBinding">
                                <SettingsBehavior AllowEllipsisInText="true" />
                                <Paddings Padding="0" />
                                <Settings ShowVerticalScrollBar="True" VerticalScrollableHeight="400" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
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
                                <SettingsText EmptyDataRow="Trenutno ni podatka o aktivnostih uporabnika." />
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="Code" Caption="Koda" Width="20%" />
                                    <dx:GridViewDataColumn FieldName="Name" Caption="Naziv" Width="30%" />
                                    <dx:GridViewDataColumn FieldName="Notes" Caption="Opombe" Width="35%" />
                                     <dx:GridViewDataDateColumn FieldName="ts" Caption="Datum" Width="15%">
                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                                </Columns>
                                <SettingsPager EnableAdaptivity="true" />
                                <Styles Header-Wrap="True" />
                            </dx:ASPxGridView>
                        </DetailRow>
                    </Templates>
                </dx:ASPxGridView>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
