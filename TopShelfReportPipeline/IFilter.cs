using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TopShelfReportPipeline
{
    public interface IFilter
    {
        bool Removed { get; set; }
        string Uid { get; set; }
        string GUID { get; set; }
        string Column { get; set; }
        bool FieldFilter { get; set; }
        OperatorValue OperatorValue { get; set; }
        string AliasTable { get; set; }
        string Alias { get; set; }
        string[] Values { get; set; }
    }
}
