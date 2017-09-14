using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline.Network
{
    internal class ReportRequestWrapper<T> where T : IReportRequest
    {
        internal ReportRequestWrapper(T ReportRequest)
        {
            this.ReportRequest = ReportRequest;
            this.RequestID = Guid.NewGuid();
        }

        internal T ReportRequest { get; set; }
        internal Guid RequestID { get; private set; }
    }
}
