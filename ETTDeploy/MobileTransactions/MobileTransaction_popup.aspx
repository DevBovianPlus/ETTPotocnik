<%@ Page Title="Pregled zaloge" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="MobileTransaction_popup.aspx.cs" Inherits="ETT_Web.MobileTransactions.MobileTransaction_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtQuantity];
            var lookupItems = [lookUpProduct, lookUpLocation];
            process = InputFieldsValidation(lookupItems, inputItems, null, null, null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 20px;">
            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="UPORABNIK : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxGridLookup ID="GridLookupUser" runat="server" ClientInstanceName="lookUpUser"
                KeyFieldName="UserID" TextFormatString="{0} {1}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSUser" IncrementalFilteringMode="Contains" PopupHorizontalAlign="WindowCenter"
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
                <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Zaposleni" SwitchToModalAtWindowInnerWidth="650" />
                <ClientSideEvents Init="SetFocus" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Ime" FieldName="EmployeeID.Firstname"
                        ReadOnly="true" MinWidth="230" MaxWidth="300" Width="30%">
                    </dx:GridViewDataTextColumn>

                    <%--<dx:GridViewDataTextColumn Caption="EmployeeID.Lastname"
                        FieldName="Lastname" Width="30%" AdaptivePriority="2" MinWidth="200" MaxWidth="300">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="EmployeeID.Phone"
                        FieldName="Phone" Width="20%" AdaptivePriority="1" MinWidth="200" MaxWidth="250">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="EmployeeID.Email"
                        FieldName="Email" Width="20%" AdaptivePriority="0" MinWidth="450" MaxWidth="250">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>--%>
                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 13px;">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="KODA : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtScannedCode" ClientInstanceName="clientTxtScannedCode"
                CssClass="text-box-input" Font-Size="13px" Width="30%" MaxLength="10" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 20px;">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="ARTIKEL : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxGridLookup ID="GridLookupProduct" runat="server" ClientInstanceName="lookUpProduct"
                KeyFieldName="ProductID" TextFormatString="{0}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSProduct" IncrementalFilteringMode="Contains"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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
                <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Artikli" SwitchToModalAtWindowInnerWidth="650" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                        ReadOnly="true" MinWidth="230" MaxWidth="400" Width="40%">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Merska enota"
                        FieldName="MeasuringUnitID.Name" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Kategorija"
                        FieldName="CategoryID.Name" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Proizvajalec"
                        FieldName="SupplierID.Name" Width="20%" AdaptivePriority="2" MinWidth="200" MaxWidth="250">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 28px;">
            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="DOBAVITELJ : *" Font-Bold="true"></dx:ASPxLabel>
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
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 13px;">
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="IZ LOKACIJE : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxGridLookup ID="GridLookupLocationFrom" runat="server" ClientInstanceName="lookUpLocationFrom"
                KeyFieldName="LocationID" TextFormatString="{0}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSLocation" IncrementalFilteringMode="Contains"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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
                <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Lokacije" SwitchToModalAtWindowInnerWidth="650" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                        ReadOnly="true" MinWidth="230" MaxWidth="400" Width="60%">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Opombe"
                        FieldName="Notes" Width="40%" AdaptivePriority="1" MinWidth="200" MaxWidth="250">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>

    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0" style="margin-right: 13px;">
            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="NA LOKACIJO : *" Font-Bold="true"></dx:ASPxLabel>
        </div>
        <div class="col no-padding-left">
            <dx:ASPxGridLookup ID="GridLookupLocationTo" runat="server" ClientInstanceName="lookUpLocationTo"
                KeyFieldName="LocationID" TextFormatString="{0}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSLocation" IncrementalFilteringMode="Contains"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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
                <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Lokacije" SwitchToModalAtWindowInnerWidth="650" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name"
                        ReadOnly="true" MinWidth="230" MaxWidth="400" Width="60%">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Opombe"
                        FieldName="Notes" Width="40%" AdaptivePriority="1" MinWidth="200" MaxWidth="250">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>


    <div class="row m-0 d-flex align-items-center pb-2">
        <div class="col-0 pr-0 mr-3">
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

    <dx:XpoDataSource ID="XpoDSProduct" runat="server" ServerMode="true"
        DefaultSorting="ProductID" TypeName="ETT_DAL.ETTPotocnik.Product">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSLocation" runat="server" ServerMode="true"
        DefaultSorting="LocationID" TypeName="ETT_DAL.ETTPotocnik.Location">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSUser" runat="server" ServerMode="true"
        DefaultSorting="UserID" TypeName="ETT_DAL.ETTPotocnik.Users">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSSupplier" runat="server" ServerMode="true"
        DefaultSorting="ClientID" TypeName="ETT_DAL.ETTPotocnik.Client" Criteria="[ClientTypeID] = 1">
    </dx:XpoDataSource>

</asp:Content>
