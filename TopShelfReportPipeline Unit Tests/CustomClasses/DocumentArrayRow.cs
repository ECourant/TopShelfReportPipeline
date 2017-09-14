using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TopShelfReportPipeline_Unit_Tests
{
    public class DocumentArrayRow
    {
        [JsonProperty("Doc Number")]
        public string DocumentNumber { get; set; }
        [JsonProperty("Document ID")]
        public int DocumentID { get; set; }
        [JsonProperty("Document Line ID")]
        public int DocumentLineID { get; set; }
        [JsonProperty("Part ID")]
        public int PartID { get; set; }
        [JsonProperty("Part Name")]
        public string PartName { get; set; }
        [JsonProperty("QTY")]
        public double QTY { get; set; }
        [JsonProperty("QTYComplete")]
        public double QTYComplete { get; set; }
        [JsonProperty("Requires Serial Number")]
        public bool RequiresSerialNumber { get; set; }
    }
}
