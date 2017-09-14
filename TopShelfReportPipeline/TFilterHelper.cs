using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline
{
    public class TFilterHelper : TFilter
    {
        public TFilterHelper(string Alias, OperatorValue OperatorValue, string Value)
        {
            this.Alias = Alias;
            this.OperatorValue = OperatorValue;
            this.Values = new string[] { Value };
        }

        public TFilterHelper(string Alias, OperatorValue OperatorValue, params string[] Value)
        {
            this.Alias = Alias;
            this.OperatorValue = OperatorValue;
            this.Values = new string[] { string.Join(", ", Value) };
        }
    }
}
