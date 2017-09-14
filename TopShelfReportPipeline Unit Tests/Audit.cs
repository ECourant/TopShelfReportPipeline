using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TopShelfReportPipeline_Unit_Tests
{
    [TestClass]
    public class Audit
    {
        [TestMethod]
        public void Audit_AssetAdd()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Audit\\Asset Add"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void Audit_AssetAllByPart()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Audit\\Asset All by Part"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void Audit_AssetBuilt()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Audit\\Asset Built"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void Audit_AssetKit()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Audit\\Asset Kit"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void Audit_AssetMove()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Audit\\Asset Move"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }



        /// <summary>
        /// Because the User Actions Report returns multiple tables this request will fail, I plan on adding support for multi-table responses in the future.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Audit_UserActionsReport()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Audit\\User Actions Report"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
