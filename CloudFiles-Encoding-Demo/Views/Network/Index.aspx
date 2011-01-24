<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">Upload</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Network Upload</h2>
    
    <p>Use the following form to upload your files to a server.</p>
    
    <div id="fileUploadHolderSingle" style="width: 300px; height: 80px;">
        <div id="uploaderSingle"></div>
    </div>
    
    <div id="fileUploadHolderMulti" style="width: 600px; height: 300px;">
        <div id="uploaderMulti"></div>
    </div>
    
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="ScriptSection" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
        
            var uploaderSingle = createSilverlight(
                "uploaderSingle",
                "fileUploadHolderSingle",
                "/ClientBin/CreateThe.MediaBlog.UI.SilverLight.xap",
                "UploadHandlerName=Network/UploadAdvancedSingle,HttpUploader=true,MaxFileSizeKB=,MaxUploads=1,FileFilter=,CustomParam=yourparameters,DefaultColor=White,mode=single,LinkColor=#8c7c3d",
                "singleUploaderLoaded"
                );

            var uploaderMulti = createSilverlight(
                "uploaderMulti",
                "fileUploadHolderMulti",
                "/ClientBin/CreateThe.MediaBlog.UI.SilverLight.xap",
                "UploadHandlerName=Network/UploadAdvanced,HttpUploader=true,MaxFileSizeKB=,MaxUploads=6,FileFilter=,CustomParam=yourparameters,DefaultColor=White,mode=multi,LinkColor=#8c7c3d",
                "multiUploaderLoaded"
                );
        });
    </script>
    
</asp:Content>