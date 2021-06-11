<%@ Page Title="Nadzorna plošča" Language="C#" MasterPageFile="~/MasterPages/Welcome.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ETT_Web.Home" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolderMain_FormLayoutWrap').keypress(function (event) {
                var key = event.which;
                if (key == 13) {
                    CauseValidation(this, event);
                    clientUsername.GetInputElement().blur();
                    clientPass.GetInputElement().blur();
                    return false;
                }
            });

            var unhandledException = GetUrlQueryStrings()['unhandledExp'];
            var sessionEnd = GetUrlQueryStrings()['sessionExpired'];
            var messageType = GetUrlQueryStrings()['messageType'];

            if (unhandledException) {
                $("#unhandledExpModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.unhandledExp;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
            else if (sessionEnd) {
                $("#sessionEndModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.sessionExpired;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
            else if (messageType !== undefined) {
                var value = "";

                switch (messageType) {
                    case "1":
                        var resource = "";<%--'<%= KVP_Obrazci.Resources.HandledException.res_01 %>';--%>
                        value = resource;
                        break;
                    case "2":
                        value = ""; <%-- '<%= KVP_Obrazci.Resources.HandledException.res_02 %>';--%>
                        break;
                    default:
                        break;
                }

                $("#handledExpBodyText").append(value);
                $("#handledExpModal").modal("show");

                //we delete messageType query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.messageType;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }

        });

        function CauseValidation(s, e) {
            var procees = false;
            var inputItems = [clientUsername, clientPass];

            procees = InputFieldsValidation(null, inputItems, null, null);

            if (procees) {
                clientLoadingPanel.Show();
                clientLoginCallback.PerformCallback("SignInUserCredentials");
            }
        }

        function EndLoginCallback(s, e) {
            clientLoadingPanel.Hide();

            clientUsername.SetText("");
            clientPass.SetText("");

            if (s.cpResult != "" && s.cpResult !== undefined) {
                clientErrorLabel.SetText(s.cpResult);
                delete (s.cpResult);
            }
            else
                window.location.assign('Home.aspx');//"../Default.aspx"
        }
        function ClearText(s, e) {
            clientErrorLabel.SetText("");

            $(clientUsername.GetInputElement()).parent().parent().parent().removeClass("focus-text-box-input-error");
            $(clientPass.GetInputElement()).parent().parent().parent().removeClass("focus-text-box-input-error")
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="FormLayoutWrap" runat="server" style="display: flex; width: 30%; margin: 0 auto; overflow: hidden; padding: 10px; border: 1px solid #e1e1e1; border-radius: 3px; box-shadow: 5px 10px 18px #e1e1e1; background-color: white; margin-top: 30px;">
        <dx:ASPxFormLayout ID="ASPxFormLayoutLogin" runat="server">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="500" />
            <Items>
                <dx:LayoutGroup Name="LOGIN" GroupBoxDecoration="HeadingLine" Caption="Prijava" UseDefaultPaddings="false">
                    <Items>
                        <dx:LayoutItem Caption="Error label caption" Name="ErrorLabelCaption" ShowCaption="False"
                            CaptionSettings-VerticalAlign="Middle">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxLabel ID="ErrorLabel" runat="server" Text="" ForeColor="Red"
                                        ClientInstanceName="clientErrorLabel">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Uporabniško ime" Name="Username" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="20px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="txtUsername" runat="server"
                                        CssClass="text-box-input" ClientInstanceName="clientUsername"
                                        AutoCompleteType="Disabled">
                                        <FocusedStyle CssClass="focus-text-box-input" />
                                        <ClientSideEvents Init="SetFocus" GotFocus="ClearText" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Geslo" Name="Password" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="10px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="txtPassword" runat="server"
                                        CssClass="text-box-input" Password="true" ClientInstanceName="clientPass">
                                        <ClientSideEvents GotFocus="ClearText" />
                                        <FocusedStyle CssClass="focus-text-box-input" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Zapomni si geslo" Name="RememberMe" Paddings-PaddingTop="10px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="rememberMeCheckBox" runat="server" ToggleSwitchDisplayMode="Always" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Name="SignUp" ShowCaption="False" Paddings-PaddingTop="20px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <div class="row">
                                        <div class="col-xs-12 float-right">
                                            <dx:ASPxButton ID="ASPxButton8" runat="server" Text="PRIJAVA" Width="100"
                                                AutoPostBack="false" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="CauseValidation" />
                                                <Image Url="Images/lock2.png" UrlHottracked="Images/lock2Hoover.png" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>

                                    <dx:ASPxCallback ID="LoginCallback" runat="server" OnCallback="LoginCallback_Callback"
                                        ClientInstanceName="clientLoginCallback">
                                        <ClientSideEvents EndCallback="EndLoginCallback" />
                                    </dx:ASPxCallback>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
        <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="clientLoadingPanel" runat="server" Modal="true">
        </dx:ASPxLoadingPanel>
    </div>

    <div id="HomeContent" runat="server">
        <div class="row m-0 h-100">
            <div class="col-sm-9 p-0">
                <div class="card d-none">
                    <div class="card-header" style="background-color: #FAFCFE">
                        <div class="d-flex justify-content-between align-items-center">
                            <h5>OSNOVNI PODATKI</h5>
                            <a data-toggle="collapse" href="#collapseBasicData" aria-expanded="true" aria-controls="collapseBasicData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                        </div>
                    </div>
                    <div class="collapse show" id="collapseBasicData">
                        <div class="card-body" style="background-color: #eef2f5d6;">
                            <div class="row">
                                <div class="col-md-2 card p-0 card-link" style="height:150px;" onclick="javascript:alert('sdf');">
                                    <div class="card-body">
                                        <div class="text-right"><i class="fas fa-users" style="font-size:40px"></i></div>
                                        <div class="card-title"><h2>20</h2></div>
                                        <div class="card-subtitle mb-2 text-muted"><h5>Minerji</h5></div>
                                        <div class="progress">
                                            <div class="progress-bar bg-info progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%"></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-2 card p-0 card-link" style="height:150px;" onclick="javascript:alert('sdf');">
                                    <div class="card-body">
                                        <div class="text-right"><i class="fas fa-users" style="font-size:40px"></i></div>
                                        <div class="card-title"><h2>20</h2></div>
                                        <div class="card-subtitle mb-2 text-muted"><h5>Minerji</h5></div>
                                        <div class="progress">
                                            <div class="progress-bar bg-info progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%"></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-2 card p-0 card-link" style="height:150px;" onclick="javascript:alert('sdf');">
                                    <div class="card-body">
                                        <div class="text-right"><i class="fas fa-users" style="font-size:40px"></i></div>
                                        <div class="card-title"><h2>20</h2></div>
                                        <div class="card-subtitle mb-2 text-muted"><h5>Minerji</h5></div>
                                        <div class="progress">
                                            <div class="progress-bar bg-info progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%"></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-2 card p-0 card-link" style="height:150px;" onclick="javascript:alert('sdf');">
                                    <div class="card-body">
                                        <div class="text-right"><i class="fas fa-users" style="font-size:40px"></i></div>
                                        <div class="card-title"><h2>20</h2></div>
                                        <div class="card-subtitle mb-2 text-muted"><h5>Minerji</h5></div>
                                        <div class="progress">
                                            <div class="progress-bar bg-info progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%"></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-2 card p-0 card-link" style="height:150px;" onclick="javascript:alert('sdf');">
                                    <div class="card-body">
                                        <div class="text-right"><i class="fas fa-users" style="font-size:40px"></i></div>
                                        <div class="card-title"><h2>20</h2></div>
                                        <div class="card-subtitle mb-2 text-muted"><h5>Minerji</h5></div>
                                        <div class="progress">
                                            <div class="progress-bar bg-info progress-bar-animated" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card d-none">
                    <div class="card-header" style="background-color: #FAFCFE">
                        <div class="d-flex justify-content-between align-items-center">
                            <h5>Grafi</h5>
                            <a data-toggle="collapse" href="#collapseGraphData" aria-expanded="true" aria-controls="collapseGraphData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                        </div>
                    </div>
                    <div class="collapse show" id="collapseGraphData">
                        <div class="card-body" style="background-color: #eef2f5d6;">asddfsf</div>
                    </div>
                </div>
            </div>
            <div class="col-sm-3 border-left p-0 d-none" style="background-color: #FFF;">
                <div class="d-flex p-3 justify-content-between">
                    <div class="p-2">Zapri</div>
                    <div class="p-2">
                        <a href="#" class="p-3" style="border: 1px solid #DAE0E4; border-radius: 20px; text-decoration: none;">NOVO OPRAVILO</a>
                    </div>
                </div>
                <div class="p-3"></div>
                <h6 class="pl-4">PRIHAJAJOČA OPRAVILA</h6>
                <hr />
                <div>
                    <ol>
                        <li>Lorem ipsum</li>
                        <li>Lorem ipsum</li>
                        <li>Lorem ipsum</li>
                        <li>Lorem ipsum</li>
                        <li>Lorem ipsum</li>
                    </ol>
                </div>
                <div class="p-3"></div>
                <h6 class="pl-4">NEDAVNE AKTIVNOSTI</h6>
                <hr />
                <div>
                    <ol>
                        <li>Prijava, 10:20:15</li>
                        <li>Odjava, 10:50:15</li>
                        <li>Dodajanje, 9:23:21</li>
                        <li>Brisanje, 9:20:02</li>
                        <li>Spreminjanje, 9:25:20</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>

        <!-- Unhandled exception Modal -->
    <div id="unhandledExpModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: red; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">error_outline</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Napaka!</h3>
                    <p>Sistem je naletel na napako. Naša ekipa razvijalcev je že dobila obvestilo o napaki in je v čakalni vrsti za odpravljanje. Za to se vam iskreno opravičujemo in vas lepo pozdravljamo.</p>
                </div>
            </div>

        </div>
    </div>

    <!-- Session end Modal -->
    <div id="sessionEndModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: yellow; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">warning</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Potek seje!</h3>
                    <p>Zaradi neaktivnosti vas je sistem samodejno odjavil.</p>
                </div>
            </div>

        </div>
    </div>

    <!-- Handled exception Modal -->
    <div id="handledExpModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: #bce8f1; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">error_outline</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Opozorilo!</h3>
                    <p id="handledExpBodyText"></p>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
