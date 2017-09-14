using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TopShelfReportPipeline_Unit_Tests
{
    [TestClass]
    public class Inventory
    {
        [TestMethod]
        public void Inventory_InventoryReport()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Inventory\\Inventory Report"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void Inventory_TotalPartInventory()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Inventory\\Total Part Inventory"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void Inventory_TotalQTYReport()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Inventory\\Total QTY Report"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }

        [TestMethod]
        public void Inventory_TotalQTYReportBin()
        {
            TopShelfReportPipeline.TReportConnection reportConnection = new TopShelfReportPipeline.TReportConnection(Definitions.Username, Definitions.Password);
            TopShelfReportPipeline.TReportRequest reportRequest = new TopShelfReportPipeline.TReportRequest()
            {
                ReportPath = "Inventory\\Total QTY Report Bin"
            };
            System.Data.DataTable Report = reportConnection.GetCustomReport<System.Data.DataTable, TopShelfReportPipeline.Constructors.TDataTableConstructor>(reportRequest);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Report, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
