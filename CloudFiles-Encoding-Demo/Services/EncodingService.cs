using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudExamples.Utilities;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace CloudExamples.Services
{
    /// <summary>
    /// A service that integrates with encoding.com API
    /// </summary>
    public class EncodingService
    {

        private string userId, userKey, apiUrl;

        /// <summary>
        /// An instance of the encoding service
        /// </summary>
        /// <param name="userId">User ID for this encoding account</param>
        /// <param name="userKey">User API key for this encoding account</param>
        /// <param name="apiUrl">The location of the encoding api</param>
        public EncodingService(string userId, string userKey, string apiUrl) {
            this.userId = userId;
            this.userKey = userKey;
            this.apiUrl = apiUrl;
        }

        /// <summary>
        /// Retrives a list of the all the media for this encoding account
        /// </summary>
        /// <returns>JSON Serialized response representing a list of media</returns>
        public string GetMediaList()
        {
            XDocument doc = new XDocument(
                new XElement("query",
                    new XElement("userid", userId),
                    new XElement("userkey", userKey),
                    new XElement("action", "GetMediaList")
                )
            );

            return SendApiRequest(doc);
        }

        /// <summary>
        /// Gets the status of a media object being encoded
        /// </summary>
        /// <param name="id">The id of the media to check</param>
        /// <returns>JSON Serialized response representing the status</returns>
        public string GetStatus(string id)
        {
            XDocument doc = new XDocument(
                new XElement("query",
                    new XElement("userid", userId),
                    new XElement("userkey", userKey),
                    new XElement("action", "GetStatus"),
                    new XElement("mediaid", id)
                )
            );

            return SendApiRequest(doc);
        }

        /// <summary>
        /// Adds a new media file to be encoded
        /// </summary>
        /// <param name="pullFile">The location of the file to get to encode</param>
        /// <param name="pushFile">The location to store the encoded file</param>
        /// <param name="format">Name of the format to encode in</param>
        /// <param name="extension">File extension to use when naming the encoded file</param>
        /// <returns>JSON Serialized api response regarding status of this request including media id</returns>
        public string AddMedia(string pullFile, string pushFile, string format, string extension)
        {
            XDocument doc = new XDocument(
                new XElement("query",
                    new XElement("userid", userId),
                    new XElement("userkey", userKey),
                    new XElement("action", "AddMedia"),
                    new XElement("source", pullFile),
                    new XElement("format", 
                        new XElement("output", format),
                        new XElement("destination", pushFile + "." + extension)
                    )
                )
            );

            return SendApiRequest(doc);
        }

        /// <summary>
        /// Generic helper function to send an xml document as an HTTP Post request
        /// </summary>
        /// <param name="doc">The xml document for the api call</param>
        /// <returns>JSON Serialized API response</returns>
        private string SendApiRequest(XDocument doc) {
            string sRequest = "xml=" + HttpUtility.UrlEncode(doc.ToString());
            string result = HttpUtilities.HTTPPost(apiUrl, sRequest);

            XmlDocument resultDoc = new XmlDocument();
            resultDoc.LoadXml(result);

            return JsonConvert.SerializeXmlNode(resultDoc);
        }
    }
}