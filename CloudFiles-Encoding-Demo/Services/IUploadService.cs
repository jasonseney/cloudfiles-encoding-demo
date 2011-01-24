//-----------------------------------------------------------------------
// <copyright file="IUploadService.cs" >
// </copyright>
//-----------------------------------------------------------------------

namespace UploadExamples.Services
{
    #region Using
    using System.IO;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Upload service interface
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Stores the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="path">The path to save to.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="firstChunk">if set to <c>true</c> [first chunk].</param>
        /// <param name="lastChunk">if set to <c>true</c> [last chunk].</param>
        void StoreFileAdvanced(string fileName, string path, Stream stream, string parameters, bool firstChunk, bool lastChunk);

        /// <summary>
        /// Retrieves a list of the files
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetFileList();

        /// <summary>
        /// Cancels the upload.
        /// </summary>
        /// <param name="filename">The filename.</param>
        void CancelUpload(string filename);
    }
}