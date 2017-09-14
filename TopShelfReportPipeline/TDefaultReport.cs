using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline
{
    public class TDefaultReport<T> : IReport<T>
    {
        public T[] Items { get; set; }
    }
}
