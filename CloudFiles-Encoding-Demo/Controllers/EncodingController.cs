using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudExamples.Utilities;
using System.Configuration;
using CloudExamples.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CloudExamples.Controllers
{
    [HandleError]
    public class EncodingController : Controller
    {
        private string userId, userKey, apiUrl, pullLocation, pushLocation;
        private EncodingService service;

        public EncodingController()
        {
            userId = ConfigurationManager.AppSettings["encodingUserId"];
            userKey = ConfigurationManager.AppSettings["encodingApiKey"];
            apiUrl = ConfigurationManager.AppSettings["encodingApiUrl"];
            string cloudHost = ConfigurationManager.AppSettings["cloudStorageHost"];
            pullLocation = cloudHost + "/" + ConfigurationManager.AppSettings["cloudContainerPrivate"] + "/";
            pushLocation = cloudHost + "/" + ConfigurationManager.AppSettings["cloudContainerPublic"] + "/";
            
            service = new EncodingService(userId, userKey, apiUrl);
        }

        public EncodingController(EncodingService service)
        {
            this.service = service;
        }

        public ActionResult GetStatus(string id)
        {
            JsonRawResult json = new JsonRawResult
            {
                Data = this.service.GetStatus(id)
            };

            return json;
        }

        public ActionResult GetMediaList()
        {
            JsonRawResult json = new JsonRawResult
            {
                Data = service.GetMediaList()
            };

            return json;
        }

        public ActionResult AddMedia(string file, string format)
        {
            string fileName = file;
            string extension = string.Empty;

            if(file.Contains('.')) {
                extension = file.Substring(file.LastIndexOf('.') + 1);
                fileName = file.Substring(0, file.LastIndexOf('.'));
            }

            switch (format)
            {
                case "iphone":
                    extension = "mp4";
                    break;
                case "ogg":
                    extension = "ogg";
                    break;
            }

            JsonRawResult json = new JsonRawResult
            {
                Data = service.AddMedia(pullLocation + file, pushLocation + fileName , format, extension)
            };

            return json;
        }
    }
}
