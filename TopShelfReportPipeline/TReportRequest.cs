using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TopShelfReportPipeline
{
    public class TReportRequest : IReportRequest
    {
        public TReportRequest()
        {

        }
        public TReportRequest(string ReportPath, params IFilter[] Filters)
        {
            this.ReportPath = ReportPath;
            this.Filters = Filters.ToList();
        }
        [JsonProperty("ReportPath")]
        public string ReportPath { get; set; }
        [JsonProperty("Filters")]
        public List<IFilter> Filters { get; set; }
    }
}
