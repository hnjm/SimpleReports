using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleReports;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SimpleReport.Test
{
    [TestClass]
    public class ReportRendererTests
    {
        [TestMethod]
        public void GetParamters_Returns_OneParameter()
        {
            ReportRenderer renderer = new ReportRenderer();
            var parameters = renderer.GetParameters(new { Foo = "Bar" });

            Assert.IsTrue(parameters.ContainsKey("Foo"));
            Assert.AreEqual(parameters["Foo"], "Bar");
        }

        [TestMethod]
        public void GetReport_Returns_RenderedReport()
        {
            using (var reportDefinition = new FileStream("Reports\\ExampleReport.rdlc", FileMode.Open))
            {
                var reportRenderer = new ReportRenderer("FooDisplay", reportDefinition);
                reportRenderer.Initialise();

                var data = new List<string>();
                byte[] report = reportRenderer.GetReportAsPdf(data, new { Foo = "Bar" });
                Assert.IsTrue(report.Length > 0);
            }

        }

        [TestMethod]
        public void GetReportMultipleDataSources_Returns_RenderedReport()
        {
            using (var reportDefinition = new FileStream("Reports\\ExampleReport2.rdlc", FileMode.Open))
            {
                var reportRenderer = new ReportRenderer("FooDisplay", reportDefinition);
                reportRenderer.Initialise();

                var data = new
                {
                    DataSet1 = new List<string>(), // These propery names must match that of the datasets in the report
                    DataSet2 = new List<string>()
                };

                byte[] report = reportRenderer.GetReportAsPdf(data, new { Foo = "Bar" });
                Assert.IsTrue(report.Length > 0);
            }
        }

    }
}


