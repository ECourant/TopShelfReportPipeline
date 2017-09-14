using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline_Unit_Tests
{
    public class Document
    {
        public string DocumentNumber { get; set; }
        public int DocumentID { get; set; }
        public Dictionary<string, DocumentItem> Items { get; set; }
    }
}
