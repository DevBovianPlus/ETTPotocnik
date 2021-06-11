<%@ Page Title="Kategorije" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="CategorieTable.aspx.cs" Inherits="ETT_Web.CodeList.CategorieTable" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function CallbackPanel_EndCallback(s, e) {
        }

        function HandleUserAction(s, e) {
            //vrne vrednost ki predstavlja akcijo uporabnika (add, edit, delete)
            var actionParameter = HandleUserActionsOnTabs(gridCategorie, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                callbackPanel.PerformCallback(actionParameter);
            }
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Categorie':
                            popupControCategorie.Hide();
                            callbackPanel.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'Categorie':
                            popupControCategorie.Hide();
                    }
                    break;
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
        ClientInstanceName="callbackPanel">
        <ClientSideEvents EndCallback="CallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewCategorie" Width="100%" runat="server" KeyFieldName="CategorieID" DataSourceID="XpoDSCategorie"
                    CssClass="gridview-no-header-padding" EnableRowsCache="false" AutoGenerateColumns="False" ClientInstanceName="gridCategorie"
                    OnDataBound="ASPxGridViewCategorie_DataBound">
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
                    <SettingsText EmptyDataRow="Trenutno ni podatka o kategorijah. Dodaj novo." />

                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name" AllowTextTruncationInAdaptiveMode="true" MinWidth="230" MaxWidth="400" Width="30%" />
                        <dx:GridViewDataTextColumn Caption="Koda" FieldName="Code" AdaptivePriority="1" MinWidth="150" MaxWidth="250" Width="20%" />
                        <dx:GridViewDataTextColumn Caption="Opombe" FieldName="Notes" AdaptivePriority="2" MinWidth="400" MaxWidth="500" Width="50%" />
                    </Columns>
                </dx:ASPxGridView>

                <dx:ASPxPopupControl ID="PopupControlCategorie" runat="server" ContentUrl="Categorie_popup.aspx"
                    ClientInstanceName="popupControCategorie" Modal="True" HeaderText="KATEGORIJA"
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

                <div class="row m-0 mt-2">
                    <div class="col-sm-9">
                        <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                            Height="25" Width="50" ClientInstanceName="btnDelete">
                            <ClientSideEvents Click="HandleUserAction" />
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/trash.png" UrlHottracked="../Images/trashHover.png" />
                        </dx:ASPxButton>
                    </div>
                    <div class="col-sm-3 text-right">
                        <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                            Height="25" Width="90" ClientInstanceName="btnAdd">
                            <ClientSideEvents Click="HandleUserAction" />
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                            Height="25" Width="90" ClientInstanceName="btnEdit">
                            <ClientSideEvents Click="HandleUserAction" />
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/edit.png" UrlHottracked="../Images/editHover.png" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:XpoDataSource ID="XpoDSCategorie" runat="server" ServerMode="true"
        DefaultSorting="CategorieID DESC" TypeName="ETT_DAL.ETTPotocnik.Categorie">
    </dx:XpoDataSource>
</asp:Content>
