using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
namespace TopShelfReportPipeline
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TReportConnection : IDisposable
    {
        /// <summary>
        /// Initializes a new connection to TopShelf to allow retrieval of reports.
        /// <para>Note: The report you are requesting must exist in the account you are using.</para>
        /// </summary>
        /// <param name="TopShelfUsername">The TopShelf username for the desired account.</param>
        /// <param name="TopShelfPassword">The TopShelf password for the desired account.</param>
        public TReportConnection(string TopShelfUsername, string TopShelfPassword)
        {
            this.RequestHandler = new Network.RequestHandler(TopShelfUsername, TopShelfPassword);
        }

        /// <summary>
        /// Retrieves data from the requested report, it will use a custom constructor to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="T">The custom type you are requesting data for.</typeparam>
        /// <typeparam name="C">The custom <see cref="IConstructor{T}"/> that will be used to deserialize the response data.</typeparam>
        /// <param name="ReportPath">The path to the report in TopShelf.</param>
        /// <param name="Filters">Any filters to constrain the data returned by the report.</param>
        /// <returns>Returns a custom object from the report data.</returns>
        public T GetCustomReport<T, C>(string ReportPath, params IFilter[] Filters)
            where C : IConstructor<T>, new()
            => new C().Construct(this.GetJToken(new TReportRequest(ReportPath, Filters)));

        /// <summary>
        /// Retrieves data from the requested report, it will use a custom constructor to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="T">The custom type you are requesting data for.</typeparam>
        /// <typeparam name="C">The custom <see cref="IConstructor{T}"/> that will be used to deserialize the response data.</typeparam>
        /// <param name="Request">The request information needed to retrieve the data, including report path and filters.</param>
        /// <returns>Returns a custom object from the report data.</returns>
        public T GetCustomReport<T, C>(IReportRequest Request)
            where C : IConstructor<T>, new()
            => new C().Construct(this.GetJToken(Request));

        /// <summary>
        /// Retrieves data from the requested report, it will use <see cref="TDefaultConstructor{ReportTemplate, R}"/> to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="R">The class that will be deserialized, one class will be returned for each row in the response</typeparam>
        /// <param name="ReportPath">The path to the report in TopShelf.</param>
        /// <param name="Filters">Any filters to constrain the data returned by the report.</param>
        /// <returns>Returns an array for the rows in the report.</returns>
        public R[] GetReport<R>(string ReportPath, params IFilter[] Filters)
            => this.GetReport<TDefaultReport<R>, R>(ReportPath, Filters).Items;

        /// <summary>
        /// Retrieves data from the requested report, it will use <see cref="TDefaultConstructor{ReportTemplate, ReportRow}"/> to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="R">The class that will be deserialized, one class will be returned for each row in the response.</typeparam>
        /// <param name="Request">The request information needed to retrieve the data, including report path and filters.</param>
        /// <returns>Returns an array for the rows in the report.</returns>
        public R[] GetReport<R>(IReportRequest Request)
            => this.GetReport<TDefaultReport<R>, R>(Request).Items;

        /// <summary>
        /// Retrieves data from the requested report, it will use <see cref="TDefaultConstructor{R, T}"/> to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="T">The wrapper object for the data that will be returned.</typeparam>
        /// <typeparam name="R">The class that will be deserialized, one class will be returned for each row in the response.</typeparam>
        /// <param name="ReportPath">The path to the report in TopShelf.</param>
        /// <param name="Filters">Any filters to constrain the data returned by the report.</param>
        /// <returns>Returns an <see cref="IReport{T}"/> that contains a collection of the rows in the report.</returns>
        public T GetReport<T, R>(string ReportPath, params IFilter[] Filters) 
            where T : IReport<R>, new() 
            => this.GetReport<T, R, Constructors.TDefaultConstructor<T, R>>(ReportPath, Filters);

        /// <summary>
        /// Retrieves data from the requested report, it will use a custom constructor to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="T">The wrapper object for the data that will be returned.</typeparam>
        /// <typeparam name="R">The class that will be deserialized, one class will be returned for each row in the response.</typeparam>
        /// <typeparam name="C">Specifies a custom constructor for the template and row.</typeparam>
        /// <param name="ReportPath">The path to the report in TopShelf.</param>
        /// <param name="Filters">Any filters to constrain the data returned by the report.</param>
        /// <returns>Returns an <see cref="IReport{T}"/> that contains a collection of the rows in the report.</returns>
        public T GetReport<T, R, C>(string ReportPath, params IFilter[] Filters) 
            where T : IReport<R>, new() 
            where C : IReportConstructor<T, R>, new() 
            => this.GetReport<T, R, C>(new TReportRequest(ReportPath, Filters));

        /// <summary>
        /// Retrieves data from the requested report, it will use <see cref="TDefaultConstructor{ReportTemplate, ReportRow}"/> to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="T">The wrapper object for the data that will be returned.</typeparam>
        /// <typeparam name="R">The class that will be deserialized, one class will be returned for each row in the response.</typeparam>
        /// <param name="Request">The request information needed to retrieve the data, including report path and filters.</param>
        /// <returns>Returns an <see cref="IReport{T}"/> that contains a collection of the rows in the report.</returns>
        public T GetReport<T, R>(IReportRequest Request) 
            where T : IReport<R>, new() 
            => new Constructors.TDefaultConstructor<T, R>().Construct(this.GetJToken(Request));

        /// <summary>
        /// Retrieves data from the requested report, it will use a custom constructor to deserialize the data from TopShelf.
        /// </summary>
        /// <typeparam name="T">The wrapper object for the data that will be returned.</typeparam>
        /// <typeparam name="R">The class that will be deserialized, one class will be returned for each row in the response.</typeparam>
        /// <typeparam name="C">Specifies a custom constructor for the template and row.</typeparam>
        /// <param name="Request">The request information needed to retrieve the data, including report path and filters.</param>
        /// <returns>Returns an <see cref="IReport{T}"/> that contains a collection of the rows in the report.</returns>
        public T GetReport<T, R, C>(IReportRequest Request) 
            where T : IReport<R>, new() 
            where C : IReportConstructor<T, R>, new() 
            => new C().Construct(this.GetJToken(Request));

        private Newtonsoft.Json.Linq.JToken[] GetJToken<T>(T Request) where T : IReportRequest => this.RequestHandler.RetrieveReport(Request);

        private Network.RequestHandler RequestHandler { get; set; }

        private bool disposed = false;

        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Disposes the connection to TopShelf.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
                try
                {
                    this.RequestHandler.ProcessThread.Abort();
                }
                catch
                {

                }
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
