using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace CloudExamples.Utilities
{

    /// <summary>
    /// An JSON ActionResult that allows direct access to Data field
    /// </summary>
    public class JsonRawResult : ActionResult {

        public Encoding ContentEncoding {
            get;
            set;
        }

        public string ContentType {
            get;
            set;
        }

        public object Data {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType)) {
                response.ContentType = ContentType;
            }
            else {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null) {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (Data != null) {
                response.Write(this.Data);
            }
        }
    }
}
