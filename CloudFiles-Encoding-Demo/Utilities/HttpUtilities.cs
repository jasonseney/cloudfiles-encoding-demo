using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace CloudExamples.Utilities
{
    public static class HttpUtilities
    {
        /// <summary>
        /// Sends an HTTP POST Web Request (synchronous)
        /// </summary>
        /// <param name="sUrl">The url to send to</param>
        /// <param name="sRequest">The request body</param>
        /// <returns>The web response body</returns>
        public static string HTTPPost(string sUrl, string sRequest)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = sRequest.Length;
            request.GetRequestStream().Write(Encoding.UTF8.GetBytes(sRequest), 0, sRequest.Length);
            request.GetRequestStream().Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }
    }
}
