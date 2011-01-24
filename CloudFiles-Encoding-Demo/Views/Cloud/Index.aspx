<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">Upload</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Cloud Upload</h2>
    
    <p>Use the following form to upload your files to the cloud.</p>
    
    <div id="fileUploadHolderSingle" style="width: 300px; height: 80px;">
        <div id="uploaderSingle"></div>
    </div>
    
    <div id="cloudProgress" style="margin: 0 0 25px 0;">
        <label>Cloud Upload Progress:</label>
        <span id="progressMeter"></span>
    </div>
        
    <div id="fileUploadHolderMulti" style="width: 600px; height: 300px;">
        <div id="uploaderMulti"></div>
    </div>
    
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="ScriptSection" runat="server">

    <script type="text/javascript">
        var global = this;
        $(document).ready(function() {

            var uploaderSingle = createSilverlight(
                "uploaderSingle",
                "fileUploadHolderSingle",
                "/ClientBin/CreateThe.MediaBlog.UI.SilverLight.xap",
                "UploadHandlerName=Cloud/UploadSingle,HttpUploader=true,MaxFileSizeKB=,MaxUploads=1,FileFilter=,CustomParam=yourparameters,DefaultColor=White,mode=single,LinkColor=#8c7c3d",
                "singleUploaderLoaded"
                );

            var uploaderMulti = createSilverlight(
                "uploaderMulti",
                "fileUploadHolderMulti",
                "/ClientBin/CreateThe.MediaBlog.UI.SilverLight.xap",
                "UploadHandlerName=Cloud/Upload,HttpUploader=true,MaxFileSizeKB=,MaxUploads=6,FileFilter=,CustomParam=yourparameters,DefaultColor=White,mode=multi,LinkColor=#8c7c3d",
                "multiUploaderLoaded"
                );

            // Handles ajaj response for progress data
            var logProgress = function(data) {
            
                // If complete, update UI
                if (data.Progress == 100) {
                    $('#progressMeter').html("Complete");
                }
                
                // Else if in process, keep pinging
                else if (data.Progress >= 0) {
                    $('#progressMeter').html(data.Progress + "%");
                    setTimeout(function() {
                        global.pingCloudUpload();
                    }, 100);
                }
                else {
                    // Invalid progress #, must not be available
                }
            };

            // Ping the cloud service to get progress
            global.pingCloudUpload = function() {
                return;

                $.ajax({
                    url: "/cloud/getprogress",
                    success: logProgress,
                    error: function(req, txt, err) { if (console) { console.log(txt); } },
                    dataType: "json"
                });

            };
        });
    </script>
    
</asp:Content>