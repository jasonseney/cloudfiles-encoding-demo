//-----------------------------------------------------------------------
// <copyright file="IUploadService.cs" >
// </copyright>
//-----------------------------------------------------------------------

namespace UploadExamples.Services
{
    #region Using
    using System.IO;
    using System.Web.Hosting;
    using UploadExamples.Services;
    using System.Collections.Generic;
using System;
    #endregion

    /// <summary>
    /// Upload service for storing files locally on the network or server
    /// </summary>
    public class NetworkUploadService : IUploadService
    {
        /// <summary>
        /// Temp file extension
        /// </summary>
        private string tempExtension = "_temp";
        private string tempDirectory;
        private string storageDirectory;

        /// <summary>
        /// Create a new instance of the network upload service.
        /// </summary>
        /// <param name="uploadLocation">The location to store files locally</param>
        /// <param name="tempFolderName">A temporary folder name to use for upload transfer</param>
        public NetworkUploadService(string uploadLocation, string tempFolderName)
        {
            this.storageDirectory = uploadLocation + "/";
            this.tempDirectory = this.storageDirectory + tempFolderName + "/";

            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
        }

        #region IUploadService Members

        /// <summary>
        /// Cancel the upload and delete the TEMP file
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void CancelUpload(string fileName)
        {
            string tempFileName = fileName + this.tempExtension;

            if (File.Exists(tempDirectory + tempFileName))
            {
                File.Delete(tempDirectory + tempFileName);
            }
        }

        /// <summary>
        /// Receive a chunk of data and store it on disk
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">the path to store the file</param>
        /// <param name="data">The file's data.</param>
        /// <param name="dataLength">Length of the data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="firstChunk">if set to <c>true</c> [first chunk].</param>
        /// <param name="lastChunk">if set to <c>true</c> [last chunk].</param>
        public void StoreFileAdvanced(string fileName, string path, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk)
        {
            this.StoreFile(fileName, path, parameters, firstChunk, lastChunk, (fs) => fs.Write(data, 0, dataLength));
        }

        /// <summary>
        /// Stores the file advanced.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">path to save to</param>
        /// <param name="stream">The stream.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="firstChunk">if set to <c>true</c> [first chunk].</param>
        /// <param name="lastChunk">if set to <c>true</c> [last chunk].</param>
        public void StoreFileAdvanced(string fileName, string path, Stream stream, string parameters, bool firstChunk, bool lastChunk)
        {
            this.StoreFile(fileName, path, parameters, firstChunk, lastChunk, (fs) => this.SaveFile(stream, fs));
        }

        /// <summary>
        /// Get a filestream for a local file on the server
        /// </summary>
        /// <param name="fileName">The name of the file to stream back</param>
        /// <param name="path">The path </param>
        /// <returns></returns>
        public Stream GetFileStream(string fileName, string path)
        {
            string location = Path.Combine(Path.Combine(this.storageDirectory, path),fileName);
            FileStream fs = File.Open(location, FileMode.Open);
            return fs;
        }

        /// <summary>
        /// Stores the file using a generic store action for working with a filestream
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">Path to save to</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="firstChunk">if set to <c>true</c> [first chunk].</param>
        /// <param name="lastChunk">if set to <c>true</c> [last chunk].</param>
        /// <param name="storeAction">The action to actually save the file with a file stream</param>
        private void StoreFile(string fileName, string path, string parameters, bool firstChunk, bool lastChunk, Action<FileStream> storeAction)
        {
            string tempFileName = fileName + this.tempExtension;
            string tempFileLocation = Path.Combine(this.tempDirectory, tempFileName);
            string finalLocation = Path.Combine(Path.Combine(this.storageDirectory, path),fileName);

            // Is this the first chunk of the file?
            if (firstChunk)
            {
                // Delete temp file
                if (File.Exists(tempFileLocation))
                {
                    File.Delete(tempFileLocation);
                }

                // Delete target file
                if (File.Exists(finalLocation))
                {
                    File.Delete(finalLocation);
                }
            }

            using (FileStream fs = File.Open(tempFileLocation, FileMode.Append))
            {
                storeAction(fs);
                fs.Close();
            }

            // Finish up if this is the last chunk of the file
            if (lastChunk)
            {
                // Rename file to original file
                File.Move(tempFileLocation, finalLocation);

                // Finish stuff....
                this.FinishedFileUpload(fileName, parameters);
            }
        }

        /// <summary>
        /// Delete an uploaded file
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        protected void DeleteUploadedFile(string fileName)
        {
            if (File.Exists(storageDirectory + fileName))
            {
                File.Delete(storageDirectory + fileName);
            }
        }

        /// <summary>
        /// Do your own stuff here when the file is finished uploading
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="parameters">The parameters.</param>
        protected virtual void FinishedFileUpload(string fileName, string parameters)
        {
            // Thread.Sleep(5000);
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fs">The filestream to save to.</param>
        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];

            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }

        #endregion

        #region IUploadService Members


        /// <summary>
        /// Returns a list of the network files
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetFileList()
        {
            return Directory.GetFiles(storageDirectory);
        }

        #endregion
    }
}
