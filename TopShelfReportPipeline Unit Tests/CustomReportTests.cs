using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TopShelfReportPipeline_Unit_Tests
{
    [TestClass]
    public class CustomReportTests
    {
        [TestMethod]
        public void JustInTime_DocumentArray()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "JustInTime\\DocumentArray",
                Filters = new List<TopShelfReportPipeline.IFilter>()
                {
                    new TopShelfReportPipeline.TFilterHelper("OrderIDs", TopShelfReportPipeline.OperatorValue.Equals_TextArea, new string[] 
                    {
                        "113-7722386-6287461",
                        "401118934617-654478938027",
                        "233521081010"
                    })
                }
            };
            DocumentsArray Report = reportConnection.GetReport<DocumentsArray, Document, DocumentConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
