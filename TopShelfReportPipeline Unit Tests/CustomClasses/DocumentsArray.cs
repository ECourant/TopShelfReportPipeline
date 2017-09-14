using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline_Unit_Tests
{
    public class DocumentsArray : TopShelfReportPipeline.IReport<Document>
    {
        public Document[] Items { get; set; }
    }
}
