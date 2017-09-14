using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TopShelfReportPipeline
{
    public class TFilter : IFilter
    {
        [JsonProperty("Removed")]
        public bool Removed { get; set; }
        [JsonProperty("Uid")]
        public string Uid { get; set; }
        [JsonProperty("GUID")]
        public string GUID { get; set; }
        [JsonProperty("ColumnName")]
        public string Column { get; set; }
        [JsonProperty("FieldFilter")]
        public bool FieldFilter { get; set; }
        [JsonProperty("OperatorValue"), JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public OperatorValue OperatorValue { get; set; }
        [JsonProperty("AliasTable")]
        public string AliasTable { get; set; }
        [JsonProperty("Alias")]
        public string Alias { get; set; }
        [JsonProperty("Values")]
        public string[] Values { get; set; }
    }
}
