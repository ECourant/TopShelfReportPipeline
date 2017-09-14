using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline
{
    public interface IReportConstructor<T, R> : IConstructor<T> where T : IReport<R>
    {

    }
}
