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
            var reportDefinition = "<?xml version='1.0' encoding='utf-8'?><Report xmlns='http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition' xmlns:rd='http://schemas.microsoft.com/SQLServer/reporting/reportdesigner'>  <Body>    <Height>2in</Height>    <Style />  </Body>  <Width>6.5in</Width>  <Page>    <PageHeight>29.7cm</PageHeight>    <PageWidth>21cm</PageWidth>    <LeftMargin>2cm</LeftMargin>    <RightMargin>2cm</RightMargin>    <TopMargin>2cm</TopMargin>    <BottomMargin>2cm</BottomMargin>    <ColumnSpacing>0.13cm</ColumnSpacing>    <Style />  </Page>  <AutoRefresh>0</AutoRefresh>  <ReportParameters>    <ReportParameter Name='Foo'>      <DataType>String</DataType>      <Prompt>ReportParameter1</Prompt>    </ReportParameter>  </ReportParameters>  <rd:ReportUnitType>Cm</rd:ReportUnitType>  <rd:ReportID>265d78f0-74e3-4905-954a-9c0b7be1b264</rd:ReportID></Report>";
            byte[] byteArray = Encoding.ASCII.GetBytes( reportDefinition );
            var memoryStream = new MemoryStream(byteArray);
            
            var reportRenderer = new ReportRenderer("FooDisplay", memoryStream);
            reportRenderer.Initialise();

            var data = new List<string>();
            byte[] report = reportRenderer.GetReportAsPdf(data, new { Foo = "Bar" });

            Assert.IsTrue(report.Length > 0);
        }
    }
}


