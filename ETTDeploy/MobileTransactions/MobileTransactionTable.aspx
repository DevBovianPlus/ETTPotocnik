<%@ Page Title="Mobilne transakcije" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="MobileTransactionTable.aspx.cs" Inherits="ETT_Web.MobileTransactions.MobileTransactionTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CallbackPanelMobileTransaction_EndCallback(s, e) {
            LoadingPanel.Hide();

            if (s.cpErrorDifferentBuyers != "" && s.cpErrorDifferentBuyers != undefined) {
                ShowModal('Preveč kupcev!', 'Na posamezni izdajnici je lahko samo en kupec!')
                delete (s.cpErrorDifferentBuyers);
            }
            else if (s.cpErrorIssueDocumentCreated != "" && s.cpErrorIssueDocumentCreated != undefined) {
                ShowModal('Izdajnica ustvarjena!', 'Uspešno se ustvarili izdajnico za izbran material', true);
                delete (s.cpErrorIssueDocumentCreated);
            }
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

        function IsBuyer_SelectionChanged(s, e) {
            if (gridMobileTransaction.GetSelectedRowCount() > 0)
                btnTransferToIssueDocument.SetVisible(true);
            else
                btnTransferToIssueDocument.SetVisible(false);
        }

        function TransferMobileTransactionsToIssueDocument_Click(s, e) {
            LoadingPanel.Show();
            CallbackPanelMobileTransaction.PerformCallback("TransferToIssueDocument");
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

        function FilterAllTransaction_CheckedChanged(s, e) {
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelMobileTransaction" runat="server" OnCallback="CallbackPanelMobileTransaction_Callback" ClientInstanceName="CallbackPanelMobileTransaction">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="CallbackPanelMobileTransaction_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="col-2">
                    <div class="row m-0 align-items-center">
                        <div class="col-0 pr-0">
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="Prikaži vse  : " Font-Bold="true"></dx:ASPxLabel>
                        </div>
                        <div class="col pl-0">
                            <dx:ASPxCheckBox ID="chkShowTransactionVse" runat="server">
                            </dx:ASPxCheckBox>
                        </div>
                        <div class="col pl-0">
                            <dx:ASPxButton ID="btnIzberiVse" runat="server" Text="Potrdi" AutoPostBack="false"
                                Height="25" Width="50" ClientInstanceName="btnIzberiVse" OnClick="btnIzberiVse_Click">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../Images/search.png" UrlHottracked="../Images/searchHover.png" />

                            </dx:ASPxButton>
                        </div>
                    </div>
                </div>
                <dx:ASPxGridView ID="ASPxGridViewMobileTransaction" Width="100%" runat="server" KeyFieldName="MobileTransactionID"
                    CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridMobileTransaction" OnCustomColumnDisplayText="ASPxGridViewMobileTransaction_CustomColumnDisplayText"
                    OnDataBound="ASPxGridViewMobileTransaction_DataBound" OnCommandButtonInitialize="ASPxGridViewMobileTransaction_CommandButtonInitialize" OnDataBinding="ASPxGridViewMobileTransaction_DataBinding">
                    <ClientSideEvents RowDblClick="HandleUserAction" SelectionChanged="IsBuyer_SelectionChanged" />
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
                        AllowHideDataCellsByColumnMinWidth="true">
                    </SettingsAdaptivity>
                    <SettingsBehavior AllowEllipsisInText="true" />
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="600"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                    <SettingsPager PageSize="100" ShowNumericButtons="true">
                        <PageSizeItemSettings Visible="true" Items="100,200,500" Caption="Zapisi na stran : " AllItemText="Vsi">
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

                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Caption="Izberi" Width="45px" SelectAllCheckboxMode="AllPages" />

                        <dx:GridViewDataTextColumn Caption="Prenešeno" FieldName="NeedMatching" MinWidth="70" MaxWidth="100" Width="1%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RowCnt" Visible="true" Width="70px" Caption="#" CellStyle-HorizontalAlign="Center">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Datum" FieldName="tsInsert" AllowTextTruncationInAdaptiveMode="true" MinWidth="80" MaxWidth="200" Width="5%">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy HH:mm:ss" />
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Uporabnik" FieldName="Uporabnik" MinWidth="150" MaxWidth="150" Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Iz lokacije" FieldName="IzLokacije" MinWidth="200" MaxWidth="250" Width="4%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Na lokacijo" FieldName="NaLokacijo" MinWidth="200" MaxWidth="250" Width="4%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="Dobavitelj" MinWidth="200" MaxWidth="250" Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Komercialno ime eksploziva" FieldName="Produkt" MinWidth="200" MaxWidth="250" Width="6%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Koda artikla" FieldName="UIDCode" MinWidth="150" MaxWidth="200" Width="3%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="IsBuyer" Visible="false" MinWidth="200" MaxWidth="250" Width="5%">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LocationToClientID" Visible="false" MinWidth="200" MaxWidth="250" Width="5%">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="InventoryDeliveriesLocationID" Visible="false" MinWidth="200" MaxWidth="250" Width="5%">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Datum zadnje spremembe" FieldName="tsUpdate" AllowTextTruncationInAdaptiveMode="true" MinWidth="100" MaxWidth="200" Width="5%">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy  hh:mm:ss" />
                        </dx:GridViewDataDateColumn>
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
                        <dx:ASPxButton ID="btnTransferToIssueDocument" runat="server" Text="Prenesi na izdajnico" AutoPostBack="false"
                            Height="25" Width="50" ClientInstanceName="btnTransferToIssueDocument" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/issueDocumentTransfer.png" UrlHottracked="../Images/issueDocumentTransferHover.png" />
                            <ClientSideEvents Click="TransferMobileTransactionsToIssueDocument_Click" />
                        </dx:ASPxButton>

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
        DefaultSorting="InventoryDeliveriesLocationID.LocationToID.IsBuyer DESC" TypeName="ETT_DAL.ETTPotocnik.MobileTransaction">
    </dx:XpoDataSource>

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
