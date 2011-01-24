//-----------------------------------------------------------------------
// <copyright file="CloudFilesUploadService.cs" >
// </copyright>
//-----------------------------------------------------------------------

namespace UploadExamples.Services
{
    using System;
    using com.mosso.cloudfiles.domain;
    using com.mosso.cloudfiles;
    using System.IO;
    using System.Collections.Generic;
    using System.Web.Caching;

    /// <summary>
    /// Upload service that integrates with cloud files api
    /// </summary>
    public class CloudFilesUploadService : IUploadService
    {
        #region IUploadService Members

        private string authUserName;
        private string authKey;
        private string containerName;
        private Connection cfConnection;

        private static string WEB_CACHE_KEY = "_transfer";

        public CloudFilesUploadService(string username, string apiKey, string containerName) 
        {
            this.authUserName = username;
            this.authKey = apiKey;
            this.containerName = containerName;
            cfConnection = new Connection(new UserCredentials(authUserName, authKey));
        }

        /// <summary>
        /// Stores a file passed in through an IO stream.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="path">Additional path to store the file in</param>
        /// <param name="stream">Data for the file</param>
        /// <param name="parameters">Extra meta data for this file</param>
        /// <param name="firstChunk">Is the first chunch of data</param>
        /// <param name="lastChunk">Is the last chunch of data</param>
        void IUploadService.StoreFileAdvanced(string fileName, string path, System.IO.Stream stream, string parameters, bool firstChunk, bool lastChunk)
        {
            string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(fileName).ToLower();

            var currFile = new FileTransferData {
                Name = uniqueName,
                TotalTransferred = 0,
                FileSize = stream.Length
            };

            this.WebCache[this.containerName + WEB_CACHE_KEY] = currFile;

            cfConnection.AddProgressWatcher(this.fileTransferProgress);
            cfConnection.PutStorageItemAsync(containerName, stream, uniqueName);

        }

        /// <summary>
        /// Get the CDN URI for this container
        /// </summary>
        /// <returns>The CDN URI</returns>
        public string GetCdnUri()
        {
            return cfConnection.GetContainerInformation(this.containerName).CdnUri;
        }

        /// <summary>
        /// Cancels a transfer in progress
        /// </summary>
        /// <param name="filename">The file to cancel</param>
        void IUploadService.CancelUpload(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of the files in this container
        /// </summary>
        /// <returns>A list of file names</returns>
        public IEnumerable<string> GetFileList()
        {
            var items = cfConnection.GetContainerItemList(this.containerName);
            return items;
        }
        #endregion

        /// <summary>
        /// Deletes a file from this container
        /// </summary>
        /// <param name="filename">Name of the file to delete</param>
        public void DeleteFile(string filename)
        {
            cfConnection.DeleteStorageItem(this.containerName, filename);
        }

        /// <summary>
        /// Grabs the current progress from cache
        /// </summary>
        /// <returns>The percent complete for the file transfer</returns>
        public float GetProgress()
        {
            var progressCache = (this.WebCache[this.containerName + WEB_CACHE_KEY]) as FileTransferData;
            float progress = progressCache == null ? -1 : progressCache.Progress;
            return progress;
        }

        /// <summary>
        /// The web cache used to store the progress in
        /// </summary>
        public Cache WebCache
        {
            get;
            set;
        }

        /// <summary>
        /// Calculates and stores the progress in this class's cache
        /// </summary>
        /// <param name="bytesTransferred">The number of bytes transfered so far</param>
        private void fileTransferProgress(int bytesTransferred)
        {
            var currTransfer = this.WebCache[this.containerName + WEB_CACHE_KEY] as FileTransferData;
            if (currTransfer != null)
            {
                float totalTransferred = currTransfer.TotalTransferred;
                float fileSize = currTransfer.FileSize;

                totalTransferred += bytesTransferred;

                var progress = (int)((totalTransferred / fileSize) * 100.0f);
                if (progress > 100)
                {
                    progress = 100;
                }
                currTransfer.Progress = progress;
                currTransfer.TotalTransferred = totalTransferred;
            }
        }
    }

    /// <summary>
    /// POCO to hold data on the transfer
    /// </summary>
    public class FileTransferData {
        public string Name { get; set; }
        public float TotalTransferred { get; set; }
        public float FileSize { get; set; }
        public float Progress { get; set; }
    }
}