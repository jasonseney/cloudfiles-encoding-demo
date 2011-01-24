<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CloudExamples.Models.FilesListViewModel>" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">File List</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>File List</h2>
    <p>Network Files:</p>
    <ul>
        <% foreach (var fileName in Model.NetworkFiles) { %>
                <li>
                    <%= fileName %>
                    <a target="_blank" href="/cloud/movetocloud?filename=<%= Url.Encode(fileName.Substring(fileName.LastIndexOf('/') + 1)) %>">Copy to Cloud</a>
                </li>
        <% } %>
    </ul>
    <p>Private Cloud Files:</p>
    <ul>
        <% foreach (var fileName in Model.PrivateCloudFiles) { %>
                <li>
                    <%= fileName %>&nbsp;
                    <a id="<%= fileName %>" class="encode-button" href="iphone">Encode Iphone</a>
                    <a id="<%= fileName %>" class="encode-button" href="ogg">Encode Ogg</a>
                </li>
        <% } %>
    </ul>
    <p>Public Cloud Files:</p>
    <ul>
        <% foreach (var fileName in Model.PublicCloudFiles) { %>
                <li>
                    <a target="_blank" href="<%= Model.CdnUri + Url.Encode(fileName) %>"><%= fileName %></a>
                    &nbsp;
                    <a target="_blank" href="/home/video?link=<%= Url.Encode(Model.CdnUri + fileName) %>">Play Video</a>
                </li>
        <% } %>
    </ul>
    
    <p><b>Encoding Queue</b> <a href="#" id="refresh-queue">Refresh</a></p>
    
    <ul id="encoding-queue"></ul>
    
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="ScriptSection" runat="server">

    <script type="text/javascript">
    
        // GLOBAL SPACE
    
        var global = this;
        global.encoding = {};
        global.encoding.queue = {};

        (function() {

            // PRIVATE
            function supports_html5_storage() {
                try { return 'localStorage' in window && window['localStorage'] !== null; }
                catch (e) { return false; }
            }

            // Get the latest status for each media and copy to local storage
            var updateQueue = function() {
                for (var id in global.encoding.queue) {
                    global.encoding.queue[id].status = global.encoding.updateStatus(id);
                }
                localStorage["encoding_queue"] = JSON.stringify(global.encoding.queue);
            };

            // Output the queue to the page
            var debugQueue = function() {

                $('#encoding-queue').empty();
                for (var i in global.encoding.queue) {
                    var job = global.encoding.queue[i];
                    var msg = job.sourcefile + ": " + job.status;

                    $('#encoding-queue').append('<li>' + msg + "</li>");
                }

            };

            // Handles the response for adding new media, only place the mediaId is saved
            var mediaAddedHandler = function(data, filename) {
                var mediaId = data.response.MediaID;
                var status = data.response.message;
                global.encoding.queue[mediaId] = { sourcefile: filename, status: status };

                if (supports_html5_storage()) {
                    debugQueue();
                    updateQueue();
                }

            };

            // PUBLIC 
            
            // Setups up the queue from local storage and updates statuses
            global.encoding.initQueue = function() {
                if (supports_html5_storage()) {
                    if (localStorage["encoding_queue"] != null) {
                        global.encoding.queue = JSON.parse(localStorage["encoding_queue"]);
                        updateQueue();
                    }
                }
            };

            // Ping the cloud service to get progress
            global.encoding.updateStatus = function(mediaid) {

                var statusHandler = function(data) {
                    var id = data.response.id;
                    var status = data.response.status;

                    global.encoding.queue[id].status = status;
                    debugQueue();
                };
                
                $.ajax({
                    url: "/encoding/getStatus/" + mediaid,
                    success: statusHandler,
                    error: function(req, txt, err) { if (console) { console.log(txt); } },
                    dataType: "json"
                });

            };

            // Init a new encoding process
            global.encoding.startEncoding = function(filename, format) {

                $.ajax({
                    url: "/encoding/addMedia",
                    success: function(data) {
                        mediaAddedHandler(data, filename);
                    },
                    error: function(req, txt, err) { if (console) { console.log(txt); } },
                    dataType: "json",
                    data: { file: filename, format : format}
                });
            };

        })();

        // MAIN

        $(document).ready(function() {

            $('a.encode-button').click(function(e) {
                e.preventDefault();
                global.encoding.startEncoding($(this).attr("id"), $(this).attr("href"));
            });
            
            $('#refresh-queue').click(function(e) {
                e.preventDefault();
                global.encoding.initQueue();
            });

            global.encoding.initQueue();

        });
        
        
    </script>
    
</asp:Content>