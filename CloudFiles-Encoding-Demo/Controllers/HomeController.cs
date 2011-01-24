using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using UploadExamples.Services;
using System.Configuration;
using CloudExamples.Models;

namespace UploadExamples.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private IUploadService networkService;
        private IUploadService privateCloudService;
        private IUploadService publicCloudService;

        public HomeController()
            : this(
                new NetworkUploadService(
                    ConfigurationManager.AppSettings["networkStoragePath"],
                    ConfigurationManager.AppSettings["networkTempFolderName"]
                ),
                new CloudFilesUploadService(
                    ConfigurationManager.AppSettings["cloudUserId"],
                    ConfigurationManager.AppSettings["cloudApiKey"],
                    ConfigurationManager.AppSettings["cloudContainerPrivate"]
                ),
                new CloudFilesUploadService(
                    ConfigurationManager.AppSettings["cloudUserId"],
                    ConfigurationManager.AppSettings["cloudApiKey"],
                    ConfigurationManager.AppSettings["cloudContainerPublic"]
                )

                ) { }

        public HomeController(IUploadService networkService, IUploadService privateCloudService, IUploadService publicCloudService) {
            this.networkService = networkService;
            this.privateCloudService = privateCloudService;
            this.publicCloudService = publicCloudService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Files()
        {
            FilesListViewModel filesList = new FilesListViewModel
            {
                CdnUri = ((CloudFilesUploadService)this.publicCloudService).GetCdnUri(),
                NetworkFiles = this.networkService.GetFileList(),
                PrivateCloudFiles = this.privateCloudService.GetFileList(),
                PublicCloudFiles = this.publicCloudService.GetFileList()
            };

            return View(filesList);
        }

        public ActionResult Video(string link)
        {
            return View((object)link);
        }

    }
}