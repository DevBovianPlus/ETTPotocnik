<%@ Page Title="Elektronske dobavnice" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeBehind="ProductOverview.aspx.cs" Inherits="ETT_Web.DeliveryNotes.ProductOverview" %>

<%@ Register Assembly="DevExpress.Xpo.v19.1, Version=19.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ Register TagPrefix="widget" TagName="AttachmentUpload" Src="~/Widgets/UploadAttachment.ascx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var isRequestInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtDeliveryNoteNumber];
            var dateEditItems = [dateEditDeliveryNoteDate];
            var lookupItmes = [lookUpSupplier, lookUpLocation];
            process = InputFieldsValidation(lookupItmes, inputItems, dateEditItems, null, null, null);

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

        function HandleUserAction(s, e) {
            //vrne vrednost ki predstavlja akcijo uporabnika (add, edit, delete)
            var actionParameter = HandleUserActionsOnTabs(gridDeliveryNoteItem, btnAdd, btnEdit, btnDelete, s);

            if (actionParameter != "") {
                callbackPanel.PerformCallback(actionParameter);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="nav nav-tabs">
        <li class="nav-item" runat="server" id="productOverviewItem">
            <a class="nav-link" data-toggle="tab" href="#ProductOverview"><span runat="server" id="productOverviewBadge" class="badge badge-pill badge-info">0</span> Artikli</a>
        </li>
    </ul>

    <div class="tab-content border mb-3">

        <div id="ProductOverview" class="container-fluid tab-pane fade">
            <div class="card">
                <div class="card-header" style="background-color: #FAFCFE">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6>Pregled artiklov</h6>
                        <a data-toggle="collapse" href="#collapseProductOverview" aria-expanded="true" aria-controls="collapseProductOverview"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                    </div>
                </div>
                <div class="collapse show" id="collapseProductOverview">
                    <div class="card-body p-0" style="background-color: #eef2f5d6;">
                        <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" Width="100%" OnCallback="CallbackPanel_Callback"
                            ClientInstanceName="callbackPanel">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <div class="container p-2" runat="server" id="packagingConatiner">

                                        <div class="card mb-2 hidden">
                                            <div class="card-body">
                                                <h4 class="card-title">Vsebina Zunanje pakiranje 5103180525000025:</h4>
                                                <div class="card bg-light">
                                                    <div class="card-body">
                                                        <ul class="list-group list-group-flush">
                                                            <li class="list-group-item list-group-item-action text-primary">
                                                                <i class='far fa-arrow-alt-circle-right mr-1 text-secondary'></i>Artikel: 5100180525000049 | SID: I00-0001
                                                                <div class="row ml-4">
                                                                    <div class="col-lg-4"><em><strong class="text-info">Iz: Skladišče Huda jama</strong></em></div>
                                                                    <div class="col-lg-4"><em><strong class="text-info">Na: Delovišče Ušenišče</strong></em></div>
                                                                    <div class="col-lg-4"><em><strong class="text-info">Ob: 2016-04-26 06:23:40 (Slavko Vodovnik)</strong></em></div>
                                                                </div>
                                                            </li>
                                                            <li class="list-group-item list-group-item-action">
                                                                <i class='far fa-arrow-alt-circle-right mr-1'></i>Artikel: 5100180525000050 | SID: I00-0001
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class="mt-2"><small>Skupaj Artikel: 2</small></div>
                                            </div>
                                        </div>

                                        <div class="card mb-2 hidden">
                                            <div class="card-body">
                                                <h4 class="card-title">Vsebina Zunanje pakiranje 5103180525000025:</h4>
                                                <div class="card bg-light">
                                                    <div class="card-body">
                                                        <ul class="list-group list-group-flush">
                                                            <li class="list-group-item list-group-item-action mb-2">
                                                                <i class='far fa-arrow-alt-circle-right mr-1'></i>Artikel: 5100180525000049 | SID: I00-0001
                                                            </li>
                                                            <li class="list-group-item list-group-item-action mb-2">
                                                                <i class='far fa-arrow-alt-circle-right mr-1'></i>Artikel: 5100180525000050 | SID: I00-0001
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class="mt-2"><small>Skupaj Artikel: 2</small></div>
                                            </div>
                                        </div>


                                        <div class="card mb-2 hidden">
                                            <div class="card-body">
                                                <h4 class="card-title">Vsebina Paleta 4504190206000004:</h4>
                                                <div class="card bg-light">
                                                    <ul class="list-group list-group-flush">
                                                        <li class="list-group-item list-group-item-action mb-2">
                                                            <div class="card border-secondary">
                                                                <div class="card-body">
                                                                    <h4 class="card-title">Vsebina Zunanje pakiranje U03-0014 (UID: 4503190206000046)</h4>
                                                                    <div class="card bg-light">
                                                                        <div class="card-body">
                                                                            <ul class="list-group list-group-flush">
                                                                                <li class="list-group-item list-group-item-action mb-2">
                                                                                    <i class='far fa-arrow-alt-circle-right mr-1'></i>Artikel: 5100180525000049 | SID: I00-0001
                                                                                </li>
                                                                                <li class="list-group-item list-group-item-action mb-2">
                                                                                    <i class='far fa-arrow-alt-circle-right mr-1'></i>Artikel: 5100180525000050 | SID: I00-0001
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>
                                                                    <div class="mt-2"><small>Skupaj Artikel: 2</small></div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li class="list-group-item list-group-item-action mb-2">
                                                            <div class="card border-secondary">
                                                                <div class="card-body">
                                                                    <h4 class="card-title">Vsebina Zunanje pakiranje U03-0014 (UID: 4503190206000047)</h4>
                                                                    <div class="card bg-light">
                                                                        <div class="card-body">
                                                                            <ul class="list-group list-group-flush">
                                                                                <li class="list-group-item list-group-item-action">
                                                                                    <i class='far fa-arrow-alt-circle-right mr-1'></i>Artikel: 5100180525000049 | SID: I00-0001
                                                                                </li>
                                                                                <li class="list-group-item list-group-item-action">
                                                                                    <i class='far fa-arrow-alt-circle-right mr-1'></i>Artikel: 5100180525000050 | SID: I00-0001
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>
                                                                    <div class="mt-2"><small>Skupaj Artikel: 2</small></div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
