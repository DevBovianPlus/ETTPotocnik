<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadAttachment.ascx.cs" Inherits="ETT_Web.Widgets.UploadAttachment" %>

<script type="text/javascript">
    var documentContainerID = '<%= this.WebsiteDocumentContainerID%>';
    $(document).ready(function () {
        $('[data-toggle="popover"]').popover({ html: true });

        $('#modal-btn-preview').click(function () {
            var urlPreview = $('#modal-btn-preview').data('preview');
            window.open(urlPreview, '_blank');
            $('#attachmentModal').modal('hide');
        });

        $('#modal-btn-download').click(function () {
            var downloadDocument = $('#modal-btn-download').data('download');
            LoadingPanel.Show();
            clientCallbackUploadControl.PerformCallback('Download;' + downloadDocument);
            $('#attachmentModal').modal('hide');
        });

        $('#modal-btn-delete').click(function () {
            var deleteDocument = $('#modal-btn-delete').data('delete');
            LoadingPanel.Show();
            $(".uploaded-link[data-document='" + deleteDocument + "']").remove();
            clientCallbackUploadControl.PerformCallback('Delete;' + deleteDocument);
            $('#attachmentModal').modal('hide');
        });
    });

    function onUploadControlFileUploadComplete(s, e) {
        var obj = jQuery.parseJSON(e.callbackData);
        if (obj != null) {

            $('#' + documentContainerID).append("<a class='uploaded-link' href='#' data-preview='" + obj.Url + "' data-document='" + obj.Name + "'>" +
                "<div class='uploaded-doc" + (obj.isImage ? " uploaded-image'" : obj.isXML ? " uploaded-xml'" : "'") + "><span title=" + obj.Name + ">" + obj.Name + "</span></div></a>");

            /*$('#' + documentContainerID).append("<a class='uploaded-link' href='#' data-toggle='popover'" +
                " data-placement='top top-right' data-html='true' data-trigger='focus' title='Možnosti' data-content='" +
                "<a class=\"documentOpen\" href=\"" + obj.Url + "\" target=\"_blank\">Odpri</a>" +
                "<div class=\"documentDelete\" data-document=\"" + obj.Name + "\">Izbriši</div>" +
                "<div class=\"documentDownload\" data-document=\"" + obj.Name + "\">Prenesi</div>'>" +
                "<div class='uploaded-doc" + (obj.isImage ? " uploaded-image'" : "'") + "><span title=" + obj.Name + ">" + obj.Name + "</span></div></a>");*/
           // $('[data-toggle="popover"]').popover({ html: true });
        }
    }

    function setElementVisible(elementId, visible) {
        //document.getElementById(elementId).className = visible ? "" : "hidden";
        if (visible)
            $(elementId).removeClass("hidden");
        else
            $(elementId).addClass("hidden");
    }
    function OnDropZoneEnter(s, e) {
        var dropZone = clientHfDropZone.Get('DropZone');
        if (e.dropZone.id == dropZone)
            setElementVisible('#' + documentContainerID, true);
    }
    function OnDropZoneLeave(s, e) {
        var dropZone = clientHfDropZone.Get('DropZone');
        if (e.dropZone.id == dropZone)
            setElementVisible('#' + documentContainerID, false);
    }

    $(document).on('click', '.documentDelete', function (e) {
        var obj = $(this).data("document");
        var item = $(this).parent().parent().parent();
        $(this).closest('.popover').prev().remove();

        clientCallbackUploadControl.PerformCallback('Delete;' + obj);
    });

    $(document).on('click', '.uploaded-link', function (e) {
        var obj = $(this).data("document");
        var previewUrl = $(this).data("preview");

        $('#modal-btn-preview').data('preview', previewUrl);
        $('#modal-btn-download').data('download', obj);
        $('#modal-btn-delete').data('delete', obj);

        $('#attachmentModal').modal('show');
        

        $('#attachmentModalTitle').empty();
        $('#attachmentModalTitle').append(obj);
        $('#attachmentModalBody').empty();
        $('#attachmentModalBody').append("Dokument " + obj + " lahko pregledate, prenesete ali zbrišete iz dobavnice.");
    });

    $(document).on('click', '.documentDownload', function (e) {
        var obj = $(this).data("document");

        clientCallbackUploadControl.PerformCallback('Download;' + obj);
    });

    function UploadControl_EndCallback(s, e) {
        LoadingPanel.Hide();
    }
</script>

<div id="externalDropZone" class="external-drop-zone hidden" runat="server">
    <div id="dragZone">
        <span class="dragZoneText">Povleci profilno sliko v okvir</span>
    </div>
    <div id="dropZone" class="hidden">
        <span class="dropZoneText">Drop an image here</span>
    </div>

</div>
<div style="display: block; width: 100%; overflow: hidden">
    <dx:ASPxHiddenField ID="hfDropZone" ClientInstanceName="clientHfDropZone" runat="server" />
    <dx:ASPxUploadControl ID="UploadControl" runat="server" UploadMode="Auto" Width="100%"
        ClientInstanceName="clientUploadControl" AutoStartUpload="true" ShowProgressPanel="true" DialogTriggerID="externalDropZone"
        OnFileUploadComplete="UploadControl_FileUploadComplete">
        <AdvancedModeSettings EnableDragAndDrop="true" EnableFileList="false" EnableMultiSelect="true" ExternalDropZoneID="externalDropZone"
            DropZoneText="" />
        <ValidationSettings MaxFileSize="4194304" NotAllowedFileExtensionErrorText="Datoteka ni primerna!" ErrorStyle-CssClass="validationMessage" />
        <BrowseButton Text="Dodaj dokument..." />
        <DropZoneStyle CssClass="uploadControlDropZone" />
        <ProgressBarStyle CssClass="uploadControlProgressBar" />
        <ClientSideEvents
            DropZoneEnter="OnDropZoneEnter"
            DropZoneLeave="OnDropZoneLeave"
            FileUploadComplete="onUploadControlFileUploadComplete"></ClientSideEvents>
    </dx:ASPxUploadControl>
    <dx:ASPxCallback ID="CallbackUploadControl" runat="server" ClientInstanceName="clientCallbackUploadControl"
        OnCallback="CallbackUploadControl_Callback">
        <ClientSideEvents EndCallback="UploadControl_EndCallback" />
    </dx:ASPxCallback>
</div>
<div id="documentContainer" runat="server" class="container container-doc"></div>

<div class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false" id="attachmentModal">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog modal-sm vertical-align-center">
                <div class="modal-content">
                    <div class="modal-header kvp-model-header">
                        <h4 class="modal-title ellipsis" id="attachmentModalTitle" style="color: white;"></h4>
                        <button type="button" class="close" data-dismiss="modal" style="color: white; opacity: 0.8">&times;</button>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row d-flex align-items-center">
                            <div class="col-3">
                                <i class="fa fa-balance-scale" style="font-size: 30px; color: #3C8DBC;"></i>
                            </div>
                            <div class="col-9 text-right">
                                <p id="attachmentModalBody"></p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" id="modal-btn-preview">Predogled</button>
                        <button type="button" class="btn btn-info" id="modal-btn-download">Prenesi</button>
                        <button type="button" class="btn btn-danger" id="modal-btn-delete">Izbriši</button>
                        <%--<button type="button" class="btn btn-danger" data-dismiss="modal">Zapri</button>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
