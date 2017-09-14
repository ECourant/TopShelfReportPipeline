using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline_Unit_Tests
{
    public class DocumentItem
    {
        public DocumentLine[] DocumentLines { get; set; }
        public int PartID { get; set; }
        public string PartName { get; set; }
        public double QTY => this.DocumentLines.Select(Line => Line.QTY).Sum();
        public double QTYComplete => this.DocumentLines.Select(Line => Line.QTYComplete).Sum();
        public bool RequiresSerialNumber { get; set; }
    }
}
