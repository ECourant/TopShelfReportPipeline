using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline.Constructors
{
    public class TDefaultConstructor<T, R> : IReportConstructor<T, R> where T : IReport<R>, new()
    {
        public T Construct(Newtonsoft.Json.Linq.JToken[] Tokens)
        {
            return new T()
            {
                Items = Tokens.Select(Token => Newtonsoft.Json.JsonConvert.DeserializeObject<R>(Token.ToString())).ToArray()
            };
        }
    }
}
