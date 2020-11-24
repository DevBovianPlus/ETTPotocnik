<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageUploadWidget.ascx.cs" Inherits="ETT_Web.Widgets.ImageUploadWidget" %>

<script type="text/javascript">

    $(document).ready(function () {
        /*alert($('#externalDropZone img').attr('src').length > 7);*/
        if ($('.external-drop-zone img').attr('src').length > 7)
            setElementVisible(".clearImg", true);
    });

    function onUploadControlFileUploadComplete(s, e) {
        
        var profileImage = '.external-drop-zone img';//'ctl00_MainContentHolder_UserDataCallbackPanel_Test_uploadedImage';//'#externalDropZone img';
        if (e.isValid)
            $(profileImage).attr('src', e.callbackData); //document.getElementById(profileImage).src = "/UploadControl/UploadImages/" + e.callbackData;
        setElementVisible(profileImage, e.isValid);
        setElementVisible(".clearImg", e.isValid);
    }
    function onImageLoad() {
        //var externalDropZone = document.getElementById("externalDropZone");
        var uploadedImage = document.getElementById("uploadedImage");
        $('.external-drop-zone img').css('left', ($('.external-drop-zone').width() - $('.external-drop-zone img').width()) / 2 + "px");
        $('.external-drop-zone img').css('top', ($('.external-drop-zone').height() - $('.external-drop-zone img').height()) / 2 + "px");
        setElementVisible("#dragZone", false);
    }
    function setElementVisible(elementId, visible) {
        //document.getElementById(elementId).className = visible ? "" : "hidden";
        if (visible)
            $(elementId).show();
        else
            $(elementId).hide();
    }

    function onImageClear() {
        $('.external-drop-zone img').attr('src', '');
        setElementVisible('.external-drop-zone img', false);
        setElementVisible("#dragZone", true);
        setElementVisible(".clearImg", false);
        clientClearImageCallback.PerformCallback("ClearImage");
    }
</script>

<div id="externalDropZone" class="external-drop-zone" runat="server" style="margin: 0 auto;">
    <div id="dragZone">
        <span class="dragZoneText">Povleci profilno sliko v okvir</span>
    </div>
    <img id="uploadedImage" src="#" alt="" runat="server" />
    <div id="dropZone" class="hidden">
        <span class="dropZoneText">Drop an image here</span>
    </div>
</div>
<div style="display: block; width: 100%; overflow: hidden">
    <dx:ASPxUploadControl ID="UploadControl" runat="server" UploadMode="Auto" Width="100%"
        ClientInstanceName="clientUploadControl" AutoStartUpload="true" ShowProgressPanel="true" DialogTriggerID="externalDropZone"
        OnFileUploadComplete="UploadControl_FileUploadComplete" Theme="Moderno">
        <AdvancedModeSettings EnableDragAndDrop="true" EnableFileList="false" EnableMultiSelect="false" ExternalDropZoneID="externalDropZone"
            DropZoneText="" />
        <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
        <BrowseButton Text="Dodaj sliko..." />
        <DropZoneStyle CssClass="uploadControlDropZone" />
        <ProgressBarStyle CssClass="uploadControlProgressBar" />
        <ClientSideEvents
            DropZoneEnter="function(s, e) { if(e.dropZone.id == 'externalDropZone') setElementVisible('dropZone', true); }"
            DropZoneLeave="function(s, e) { if(e.dropZone.id == 'externalDropZone') setElementVisible('dropZone', false); }"
            FileUploadComplete="onUploadControlFileUploadComplete"></ClientSideEvents>
    </dx:ASPxUploadControl>
    <div class="clearImg" onclick="onImageClear()" title="Pobriši sliko" style="display:none;"></div>
    <dx:ASPxCallback ID="ClearImageCallback" runat="server" ClientInstanceName="clientClearImageCallback"
        OnCallback="ClearImageCallback_Callback" />
</div>
