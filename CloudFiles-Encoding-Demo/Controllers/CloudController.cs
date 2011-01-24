using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using UploadExamples.Services;
using System.Configuration;

namespace UploadExamples.Controllers
{
    [HandleError]
    public class CloudController : Controller
    {
        private IUploadService uploadService;
        private IUploadService networkService;

        public CloudController() : this(new CloudFilesUploadService(
            ConfigurationManager.AppSettings["cloudUserId"], 
            ConfigurationManager.AppSettings["cloudApiKey"], 
            ConfigurationManager.AppSettings["cloudContainerPrivate"]), 
            new NetworkUploadService(
                ConfigurationManager.AppSettings["networkStoragePath"],
                ConfigurationManager.AppSettings["networkTempFolderName"]
            )
        ) { }

        public CloudController(IUploadService uploadService, IUploadService networkService) {
            this.uploadService = uploadService;
            this.networkService = networkService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadSingle(string file, string param, bool? first, bool? last)
        {
            JsonResult result = new JsonResult
            {
                Data = new { Sucess = true }
            };

            ((CloudFilesUploadService)this.uploadService).WebCache = this.HttpContext.Cache;

            try
            {
                this.uploadService.StoreFileAdvanced(file, null, this.HttpContext.Request.InputStream, param, first ?? false, last ?? false);
            }
            catch (Exception ex)
            {
                result.Data = new { Sucess = false, Error = ex };
            }

            return result;
        }

        public ActionResult MoveToCloud(string fileName, string param)
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { Sucess = true }
            };

            ((CloudFilesUploadService)this.uploadService).WebCache = this.HttpContext.Cache;

            try
            {
                var stream = ((NetworkUploadService)this.networkService).GetFileStream(fileName, string.Empty);
                this.uploadService.StoreFileAdvanced(fileName, null, stream, param, false, false);
            }
            catch (Exception ex)
            {
                throw ex;
                //result.Data = new { Sucess = false, Error = ex };
            }

            return result;
            
        }

        public ActionResult GetProgress()
        {
            ((CloudFilesUploadService)this.uploadService).WebCache = this.HttpContext.Cache;

            float progressPercent = ((CloudFilesUploadService)this.uploadService).GetProgress();

            JsonResult result = new JsonResult { 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { Progress = progressPercent }
            };

            return result;
        }
    }
}