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
    public class NetworkController : Controller
    {
        private IUploadService uploadService;
        private string tempFolderName;
        private string storagePath;

        public NetworkController() : this(new NetworkUploadService(
            ConfigurationManager.AppSettings["networkStoragePath"],
            ConfigurationManager.AppSettings["networkTempFolderName"]
            )) { }

        public NetworkController(IUploadService uploadService) {
            this.uploadService = uploadService;
            this.tempFolderName = ConfigurationManager.AppSettings["networkTempFolderName"];
            this.storagePath = ConfigurationManager.AppSettings["networkStoragePath"];
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Uploads the advanced single.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <param name="data">The dile's data.</param>
        /// <param name="dataLength">Length of the data.</param>
        /// <param name="param">The param list.</param>
        /// <param name="first">The first byte switch.</param>
        /// <param name="last">The last byte switch.</param>
        [HttpPost]
        [ValidateInput(false)]
        public void UploadAdvancedSingle(string file, byte[] data, int? dataLength, string param, bool? first, bool? last)
        {
            this.uploadService.StoreFileAdvanced(file, string.Empty, this.HttpContext.Request.InputStream, param, first ?? false, last ?? false);
        }

        /// <summary>
        /// Receive a chunk of data and store it on disk
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <param name="data">The file's data.</param>
        /// <param name="dataLength">Length of the data.</param>
        /// <param name="param">The param list.</param>
        /// <param name="first">The first byte switch.</param>
        /// <param name="last">The last byte switch.</param>
        [HttpPost]
        [ValidateInput(false)]
        public void UploadAdvanced(string file, byte[] data, int? dataLength, string param, bool? first, bool? last)
        {

            this.uploadService.StoreFileAdvanced(file, string.Empty, this.HttpContext.Request.InputStream, param, first ?? false, last ?? false);
        }

    }
}
