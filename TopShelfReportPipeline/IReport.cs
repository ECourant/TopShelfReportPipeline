using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline
{
    public interface IReport<T>
    {
        T[] Items { get; set; }
    }
}
