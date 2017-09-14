using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline
{
    public interface IConstructor<T>
    {
        T Construct(Newtonsoft.Json.Linq.JToken[] Tokens);
    }
}
