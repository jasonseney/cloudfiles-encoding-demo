using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudExamples.Models
{
    public class FilesListViewModel
    {
        public FilesListViewModel() { }

        public string CdnUri { get; set; }
        public IEnumerable<string> NetworkFiles { get; set; }
        public IEnumerable<string> PrivateCloudFiles { get; set; }
        public IEnumerable<string> PublicCloudFiles { get; set; }
    }
}
