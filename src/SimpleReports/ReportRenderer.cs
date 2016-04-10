using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Collections;

namespace SimpleReports
{
    public class ReportRenderer
    {
        ReportViewer ReportViewer { get; set; }

        // CONFIG 
        string displayName;
        Stream reportDefinition;

        public ReportRenderer()
        {

        }

        public ReportRenderer(string displayName, Stream reportDefinition)
        {



            this.displayName = displayName;
            this.reportDefinition = reportDefinition;
        }

        public void Initialise()
        {
            this.ReportViewer = new ReportViewer();
            ReportViewer.Width = new Unit(100, UnitType.Percentage);
            ReportViewer.Height = new Unit(100, UnitType.Percentage);
            ReportViewer.EnableViewState = false;
            ReportViewer.LocalReport.DisplayName = this.displayName;
            ReportViewer.LocalReport.LoadReportDefinition(reportDefinition);
            ReportViewer.LocalReport.EnableHyperlinks = true;
            ReportViewer.ShowBackButton = false;
            ReportViewer.ShowCredentialPrompts = false;
            ReportViewer.ShowDocumentMapButton = false;
            ReportViewer.ShowExportControls = false;
            ReportViewer.ShowFindControls = false;
            ReportViewer.ShowPageNavigationControls = false;
            ReportViewer.ShowParameterPrompts = false;
            ReportViewer.ShowPrintButton = false;
            ReportViewer.ShowPromptAreaButton = false;
            ReportViewer.ShowRefreshButton = false;
            ReportViewer.ShowReportBody = false;
            ReportViewer.ShowToolBar = false;
            ReportViewer.ShowWaitControlCancelLink = false;
            ReportViewer.ShowZoomControl = false;
            ReportViewer.KeepSessionAlive = false;
        }

        public byte[] GetReportAsPdf<T>(List<T> data, dynamic param, string Extension, bool landscape)
        {
            var datasourceName = this.ReportViewer.LocalReport.GetDataSourceNames().FirstOrDefault();
            if (string.IsNullOrEmpty(datasourceName))
            {
                throw new ApplicationException("The report should contian at least 1 datasource");
            }

            var reportDataSource = new ReportDataSource(datasourceName, data);
            this.ReportViewer.LocalReport.DataSources.Add(reportDataSource);

            Dictionary<string, object> reportProperties = GetParameters(param);
            var listOfReportParameters = new List<ReportParameter>();
            foreach (var dictvalue in reportProperties)
            {
                listOfReportParameters.Add(new ReportParameter(dictvalue.Key, Convert.ToString(dictvalue.Value)));
            }
            this.ReportViewer.LocalReport.SetParameters(listOfReportParameters);

            string mimeType = string.Empty;
            string encoding = string.Empty;
            string fileNameExtension = string.Empty;

            var deviceInfo = new StringBuilder();
            deviceInfo.Append("<DeviceInfo>");
            deviceInfo.Append("<OutputFormat>" + Extension + @"</OutputFormat>");
            if (landscape)
            {
                deviceInfo.Append("<PageWidth>29.7cm</PageWidth><PageHeight>21.0cm</PageHeight>,");
            }
            else
            {
                deviceInfo.Append("<PageWidth>21.0cm</PageWidth><PageHeight>29.7cm</PageHeight>,");
            }
            deviceInfo.Append(@"<MarginTop>0.1cm</MarginTop>
		      <MarginLeft>0.3cm</MarginLeft>
		      <MarginRight>0.3cm</MarginRight>
		      <MarginBottom>0.1cm</MarginBottom>
		    </DeviceInfo>");

            Warning[] warnings = null;
            string[] streams = null;
            byte[] renderedBytes = null;

            //Render the report
            renderedBytes = this.ReportViewer.LocalReport.Render(
                "PDF", deviceInfo.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return renderedBytes;
        }


        public byte[] GetReportAsPdf(dynamic dataSources, dynamic param, string Extension, bool landscape)
        {
            var datasourceName = this.ReportViewer.LocalReport.GetDataSourceNames().FirstOrDefault();
            if (string.IsNullOrEmpty(datasourceName))
            {
                throw new ApplicationException("The report should contian at least 1 datasource");
            }


            Dictionary<string, object> individualDataSources = GetParameters(dataSources);
            foreach (var datasource in individualDataSources)
            {
                var dataList = datasource.Value; //as IList;
                if (dataList != null)
                {
                    var reportDataSource = new ReportDataSource(datasource.Key, dataList);
                    this.ReportViewer.LocalReport.DataSources.Add(reportDataSource);
                }
                else
                {
                    throw new ApplicationException("All the properties in the for dataSources must be ILists");
                }
            }

            Dictionary<string, object> reportProperties = GetParameters(param);
            var listOfReportParameters = new List<ReportParameter>();
            foreach (var dictvalue in reportProperties)
            {
                listOfReportParameters.Add(new ReportParameter(dictvalue.Key, Convert.ToString(dictvalue.Value)));
            }
            this.ReportViewer.LocalReport.SetParameters(listOfReportParameters);

            string mimeType = string.Empty;
            string encoding = string.Empty;
            string fileNameExtension = string.Empty;

            var deviceInfo = new StringBuilder();
            deviceInfo.Append("<DeviceInfo>");
            deviceInfo.Append("<OutputFormat>" + Extension + @"</OutputFormat>");
            if (landscape)
            {
                deviceInfo.Append("<PageWidth>29.7cm</PageWidth><PageHeight>21.0cm</PageHeight>,");
            }
            else
            {
                deviceInfo.Append("<PageWidth>21.0cm</PageWidth><PageHeight>29.7cm</PageHeight>,");
            }
            deviceInfo.Append(@"<MarginTop>0.1cm</MarginTop>
		      <MarginLeft>0.3cm</MarginLeft>
		      <MarginRight>0.3cm</MarginRight>
		      <MarginBottom>0.1cm</MarginBottom>
		    </DeviceInfo>");

            Warning[] warnings = null;
            string[] streams = null;
            byte[] renderedBytes = null;

            //Render the report
            renderedBytes = this.ReportViewer.LocalReport.Render(
                "PDF", deviceInfo.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return renderedBytes;
        }

        public Dictionary<string, object> GetParameters(dynamic param)
        {
            var properties = param.GetType().GetProperties();
            var dictionary = new Dictionary<string, object>();

            foreach (PropertyInfo prop in properties)
            {
                dictionary.Add(prop.Name, prop.GetValue(param));
            }

            return dictionary;
        }
    }
}