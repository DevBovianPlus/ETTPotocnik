<%@ Page Title="Kategorija" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Product_popup.aspx.cs" Inherits="ETT_Web.CodeList.Product_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtName, clientTxtSupplierCode];
            var lookupItems = [lookUpSupplier, lookUpCategory, lookUpMeasuringUnit];

            process = InputFieldsValidation(lookupItems, inputItems, null, null, null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Client':
                            popupControlSupplier.Hide();
                            lookUpSupplier.GetGridView().Refresh();
                            break;
                        case 'Categorie':
                            popupControlCategorie.Hide();
                            lookUpCategory.GetGridView().Refresh();
                            break;
                        case 'MeasuringUnit':
                            popupControlMeasuringUnit.Hide();
                            lookUpMeasuringUnit.GetGridView().Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'Client':
                            popupControlSupplier.Hide();
                            break;
                        case 'Categorie':
                            popupControlCategorie.Hide();
                            break;
                        case 'MeasuringUnit':
                            popupControlMeasuringUnit.Hide();
                            break;
                    }
                    break;
            }
        }

        function CallbackPanel_EndCallback(s, e) {
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
        ClientInstanceName="callbackPanel">
        <ClientSideEvents EndCallback="CallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 75px;">
                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="PSN: *" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col-5 no-padding-left">
                        <dx:ASPxTextBox runat="server" ID="txtPSN" ClientInstanceName="clientTxtPSN"
                            CssClass="text-box-input" Font-Size="13px" Width="40%" MaxLength="150" AutoCompleteType="Disabled">
                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        </dx:ASPxTextBox>
                    </div>
                </div>

                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 61px;">
                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="K.IME EKSPLOZIVA : *" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col no-padding-left">
                        <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="clientTxtName"
                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="150" AutoCompleteType="Disabled">
                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                            <ClientSideEvents Init="SetFocus" />
                        </dx:ASPxTextBox>
                    </div>
                </div>

                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 61px;">
                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Size="12px" Text="FAKTOR: *" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col no-padding-left">
                        <dx:ASPxTextBox runat="server" ID="txtFaktor" ClientInstanceName="clientTxtFaktor"
                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="150" AutoCompleteType="Disabled">
                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                            <ClientSideEvents Init="SetFocus" />
                        </dx:ASPxTextBox>
                    </div>
                </div>

                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 28px;">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DOBAVITELJ : *" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col-5 no-padding-left">
                        <dx:ASPxGridLookup ID="GridLookupSupplier" runat="server" ClientInstanceName="lookUpSupplier"
                            KeyFieldName="ClientID" TextFormatString="{0}" CssClass="text-box-input"
                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                            OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSSupplier" IncrementalFilteringMode="Contains" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter">
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
                    <div class="col-1 pl-0">
                        <dx:ASPxPopupControl ID="PopupControlSupplier" runat="server" ContentUrl="Client_popup.aspx"
                            ClientInstanceName="popupControlSupplier" Modal="True" HeaderText="DOBAVITELJ"
                            CloseAction="CloseButton" Width="720px" Height="320px" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" ShowShadow="true"
                            OnWindowCallback="PopupControlSupplier_WindowCallback">
                            <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="800" VerticalAlign="WindowCenter" MinHeight="320px" MaxWidth="680px" MaxHeight="320px" />
                            <ContentStyle BackColor="#F7F7F7">
                                <Paddings Padding="0px"></Paddings>
                            </ContentStyle>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="btnCreateSupplier" runat="server" RenderMode="Link" AutoPostBack="false" UseSubmitBehavior="false" ToolTip="Dodaj dobavitelja">
                            <Image Url="../Images/create.png" UrlHottracked="../Images/createHover.png" />
                            <ClientSideEvents Click="function(s,e){ callbackPanel.PerformCallback('CreateSupplier'); }" />
                        </dx:ASPxButton>
                    </div>
                </div>

                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 32px;">
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="KODA DOB. : *" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col-5 no-padding-left">
                        <dx:ASPxTextBox runat="server" ID="txtSupplierCode" ClientInstanceName="clientTxtSupplierCode"
                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="150" AutoCompleteType="Disabled">
                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        </dx:ASPxTextBox>
                    </div>
                </div>

                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 28px;">
                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="TIP EKSPLOZIVA : *" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col-5 no-padding-left">
                        <dx:ASPxGridLookup ID="GridLookupCategory" runat="server" ClientInstanceName="lookUpCategory"
                            KeyFieldName="CategorieID" TextFormatString="{0}" CssClass="text-box-input"
                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                            OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSCategory" IncrementalFilteringMode="Contains" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter">
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
                            <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Kategorije" SwitchToModalAtWindowInnerWidth="650" />
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                                    ReadOnly="true" MinWidth="230" MaxWidth="400" Width="40%">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Opombe"
                                    FieldName="Notes" Width="60%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridLookup>
                    </div>
                    <div class="col-1 pl-0">
                        <dx:ASPxPopupControl ID="PopupControlCategorie" runat="server" ContentUrl="Categorie_popup.aspx"
                            ClientInstanceName="popupControlCategorie" Modal="True" HeaderText="KATEGORIJA"
                            CloseAction="CloseButton" Width="680px" Height="285px" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                            AllowResize="true" ShowShadow="true"
                            OnWindowCallback="PopupControlCategorie_WindowCallback">
                            <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="800" VerticalAlign="WindowCenter" MinHeight="285px" MaxWidth="680px" MaxHeight="285px" />
                            <ContentStyle BackColor="#F7F7F7">
                                <Paddings Padding="0px"></Paddings>
                            </ContentStyle>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="btnCreateCategory" runat="server" RenderMode="Link" AutoPostBack="false" UseSubmitBehavior="false" ToolTip="Dodaj kategorijo">
                            <Image Url="../Images/create.png" UrlHottracked="../Images/createHover.png" />
                            <ClientSideEvents Click="function(s,e){ callbackPanel.PerformCallback('CreateCategory'); }" />
                        </dx:ASPxButton>
                    </div>
                </div>

                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 10px;">
                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="MERSKA ENOTA : *" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col-5 no-padding-left">
                        <dx:ASPxGridLookup ID="GridLookupMeasuringUnit" runat="server" ClientInstanceName="lookUpMeasuringUnit"
                            KeyFieldName="MeasuringUnitID" TextFormatString="{0}" CssClass="text-box-input"
                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                            OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSMeasuringUnit" IncrementalFilteringMode="Contains" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter">
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
                            <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Kategorije" SwitchToModalAtWindowInnerWidth="650" />
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                                    ReadOnly="true" MinWidth="230" MaxWidth="400" Width="40%">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Simbol" FieldName="Symbol"
                                    ReadOnly="true" MinWidth="150" MaxWidth="180" Width="20%">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Opombe"
                                    FieldName="Notes" Width="40%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridLookup>
                    </div>
                    <div class="col-1 pl-0">
                        <dx:ASPxPopupControl ID="PopupControlMeasuringUnit" runat="server" ContentUrl="MeasuringUnit_popup.aspx"
                            ClientInstanceName="popupControlMeasuringUnit" Modal="True" HeaderText="MERSKE ENOTE"
                            CloseAction="CloseButton" Width="680px" Height="285px" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                            AllowResize="true" ShowShadow="true"
                            OnWindowCallback="PopupControlMeasuringUnit_WindowCallback">
                            <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="800" VerticalAlign="WindowCenter" MinHeight="285px" MaxWidth="680px" MaxHeight="285px" />
                            <ContentStyle BackColor="#F7F7F7">
                                <Paddings Padding="0px"></Paddings>
                            </ContentStyle>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="btnCreateMeasuringUnit" runat="server" RenderMode="Link" AutoPostBack="false" UseSubmitBehavior="false" ToolTip="Dodaj mersko enoto">
                            <Image Url="../Images/create.png" UrlHottracked="../Images/createHover.png" />
                            <ClientSideEvents Click="function(s,e){ callbackPanel.PerformCallback('CreateMesuringUnit'); }" />
                        </dx:ASPxButton>
                    </div>
                </div>

                <div class="row m-0 d-flex align-items-center pb-2">
                    <div class="col-0 pr-0" style="margin-right: 58px">
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="OPOMBE : " Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <div class="col no-padding-left">
                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="3" MaxLength="300" CssClass="text-box-input" AutoCompleteType="Disabled">
                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        </dx:ASPxMemo>
                    </div>
                </div>

                <div class="row m-0 text-right">
                    <div class="col-12">
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnConfirmPopup" runat="server" Text="Potrdi" AutoPostBack="false"
                                ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopup_Click" UseSubmitBehavior="false"
                                Width="100px">
                                <ClientSideEvents Click="CheckFieldValidation" />
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnCancelPopup" runat="server" Text="Prekliči" AutoPostBack="false"
                                ClientInstanceName="clientBtnCancel" OnClick="btnCancelPopup_Click" UseSubmitBehavior="false"
                                Width="100px">
                                <Image Url="../Images/cancelPopUp.png" UrlHottracked="../Images/cancelHover.png" Width="24"></Image>
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

    <dx:XpoDataSource ID="XpoDSCategory" runat="server" ServerMode="true"
        DefaultSorting="CategorieID" TypeName="ETT_DAL.ETTPotocnik.Categorie">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSMeasuringUnit" runat="server" ServerMode="true"
        DefaultSorting="MeasuringUnitID" TypeName="ETT_DAL.ETTPotocnik.MeasuringUnit">
    </dx:XpoDataSource>

</asp:Content>
