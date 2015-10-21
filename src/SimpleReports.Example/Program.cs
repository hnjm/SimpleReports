using SimpleReports.Example.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleReports.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Where do you want to save your report?");
            var saveLocation = Console.ReadLine();
            using (var reportDefinition = new FileStream("Reports\\ExampleReport.rdlc", FileMode.Open))
            {
                ReportRenderer renderer = new ReportRenderer("Example Report", reportDefinition);
                var invoices = new List<Invoice>{
                        new Invoice{
                            InvoiceNo  ="00001"
                        },
                        new Invoice{
                         InvoiceNo  ="00002"
                        }};

                renderer.Initialise();
                var report = renderer.GetReportAsPdf(invoices, new { HelloWorld = "hello world" });
                File.WriteAllBytes(saveLocation, report);
            }

            Console.WriteLine("Report completed writing");
            Console.ReadLine();
        }
    }
}
