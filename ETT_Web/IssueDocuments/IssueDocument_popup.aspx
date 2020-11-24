<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="IssueDocument_popup.aspx.cs" Inherits="ETT_Web.IssueDocuments.IssueDocument_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var isRequestInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [txtName, txtUID250, txtQuantity];
            var lookupItmes = [lookUpSupplier];
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

        function btnSearch_Click(s, e) {
            CallbackPanel.PerformCallback("SearchByUID");
        }

        function OnClosePopUpHandler(command, sender, result) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'SearchByUID':
                            PopupControlSearchInventory.Hide();
                            CallbackPanel.PerformCallback("FillIssueDocument;" + result);
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'SearchByUID':
                            PopupControlSearchInventory.Hide();
                    }
                    break;
            }
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
                                    <div class="col-0 p-0" style="margin-right: 60px">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="UID : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col-4 p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtUID250" ClientInstanceName="txtUID250"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" BackColor="LightGray" ClientEnabled="false">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div class="col-6 p-0 pl-2">
                                        <div class="row m-0 align-items-center justify-content-end">
                                            <div class="col-6 p-0 pl-2">
                                                <dx:ASPxTextBox runat="server" ID="txtUIDSearchString" ClientInstanceName="txtUIDSearchString"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" NullText="Iskalni niz....">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div class="col-1 p-0 pl-1">
                                                <dx:ASPxButton ID="btnSearch" runat="server" Text="" AutoPostBack="false"
                                                    Height="32" ClientInstanceName="btnSearch" UseSubmitBehavior="false">
                                                    <Paddings Padding="0" />
                                                    <Image Url="../Images/search.png" UrlHottracked="../Images/searchHover.png" UrlDisabled="../Images/searchDisabled.png" Width="24" />
                                                    <ClientSideEvents Click="btnSearch_Click" />
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row m-0 pb-3">
                            <div class="col-lg-6">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 47px">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="NAZIV : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="txtName"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="400" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0 mr-3">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DOBAVITELJ : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col-5 p-0">
                                        <dx:ASPxGridLookup ID="GridLookupSupplier" runat="server" ClientInstanceName="lookUpSupplier" PopupHorizontalAlign="WindowCenter"
                                            PopupVerticalAlign="WindowCenter"
                                            KeyFieldName="ClientID" TextFormatString="{0}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                                            OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSSupplier" IncrementalFilteringMode="Contains">
                                            <ClearButton DisplayMode="OnHover" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <GridViewStyles>
                                                <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                            </GridViewStyles>
                                            <GridViewProperties>
                                                <SettingsBehavior EnableRowHotTrack="True" AllowEllipsisInText="true" AllowDragDrop="false" />
                                                <SettingsPager ShowSeparators="True" NumericButtonCount="3" EnableAdaptivity="true" />
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowVerticalScrollBar="True"
                                                    ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                            </GridViewProperties>
                                            <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Dobavitelji" SwitchToModalAtWindowInnerWidth="650" />
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                                                    ReadOnly="true" MinWidth="230" MaxWidth="400" Width="40%">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Država"
                                                    FieldName="Country" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Telefon"
                                                    FieldName="Phone" Width="20%" AdaptivePriority="1" MinWidth="200" MaxWidth="250">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Email"
                                                    FieldName="Email" Width="20%" AdaptivePriority="0" MinWidth="450" MaxWidth="250">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 28px;">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="KOLIČINA : *" Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col-4 p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtQuantity" ClientInstanceName="txtQuantity"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="300" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-12">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 41px;">
                                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="OPOMBE : " Font-Bold="true"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="14" MaxLength="2000" CssClass="text-box-input" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <dx:ASPxPopupControl ID="PopupControlSearchInventory" runat="server" ContentUrl="SearchByUID_popup.aspx"
                            ClientInstanceName="PopupControlSearchInventory" Modal="True" HeaderText="ISKANJE PO UID"
                            CloseAction="CloseButton" Width="900px" Height="600px" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                            AllowResize="true" ShowShadow="true"
                            OnWindowCallback="PopupControlSearchInventory_WindowCallback">
                            <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="800" VerticalAlign="WindowCenter" MinHeight="285px" MaxWidth="680px" MaxHeight="285px" />
                            <ContentStyle BackColor="#F7F7F7">
                                <Paddings Padding="0px"></Paddings>
                            </ContentStyle>
                        </dx:ASPxPopupControl>
                    </div>
                </div>

                <div class="row m-0 pt-2 justify-content-end">
                    <div class="col-6 text-right">
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnConfirm" runat="server" Text="Shrani" AutoPostBack="false"
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

    <dx:XpoDataSource ID="XpoDSSupplier" runat="server" ServerMode="true"
        DefaultSorting="ClientID" TypeName="ETT_DAL.ETTPotocnik.Client" Criteria="[ClientTypeID] = 1">
    </dx:XpoDataSource>

</asp:Content>
