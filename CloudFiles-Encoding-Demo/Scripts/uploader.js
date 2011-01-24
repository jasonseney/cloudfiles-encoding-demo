var global = this;

function createSilverlight(id, parentId, source, params, onLoadName) {

    Silverlight.createObjectEx({
        id: id,
        source: source,
        parentElement: document.getElementById(parentId),
        properties: {
            width: "100%",
            height: "100%",
            version: "3.0.40624.0",
            enableHtmlAccess: "true",
            windowless: "true",
            InitParams: params,
            onload: onLoadName
        },
        events: {}
    });
};

function onSilverlightError(sender, args) {

    var appSource = "";
    if (sender != null && sender != 0) {
        appSource = sender.getHost().Source;
    } 
    var errorType = args.ErrorType;
    var iErrorCode = args.ErrorCode;
    
    var errMsg = "Unhandled Error in Silverlight 2 Application " +  appSource + "\n" ;

    errMsg += "Code: "+ iErrorCode + "    \n";
    errMsg += "Category: " + errorType + "       \n";
    errMsg += "Message: " + args.ErrorMessage + "     \n";

    if (errorType == "ParserError")
    {
        errMsg += "File: " + args.xamlFile + "     \n";
        errMsg += "Line: " + args.lineNumber + "     \n";
        errMsg += "Position: " + args.charPosition + "     \n";
    }
    else if (errorType == "RuntimeError")
    {           
        if (args.lineNumber != 0)
        {
            errMsg += "Line: " + args.lineNumber + "     \n";
            errMsg += "Position: " +  args.charPosition + "     \n";
        }
        errMsg += "MethodName: " + args.methodName + "     \n";
    }

    throw new Error(errMsg);
}

function singleUploaderLoaded() {

    var uploader = document.getElementById("uploaderSingle");

    uploader.Content.Files.FileAdded = function(fileName) {
    };

    uploader.Content.Files.FileRemoved = function() {

    };

    uploader.Content.Files.AllFilesFinished = function() {
        global.pingCloudUpload();
    };

}

function mutliUploaderLoaded() {
   
    var uploader = document.getElementById("uploaderMulti");

    uploader.Content.Files.AllFilesFinished = function() {
        global.pingCloudUpload();
    };
    
    uploader.Content.Files.FileAdded = function() { };
    uploader.Content.Files.FileRemoved = function() { };
    
}