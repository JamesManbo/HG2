using HG.Entities.BaoCaoThongKe;
using HG.WebApp.Models.BaoCaoThongKe;
using Microsoft.Reporting.NETCore;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

using System.Reflection;

namespace HG.WebApp.Reports
{
    public class ReportHelper
    {

        public static void LoadBaoCaoSoLuong(LocalReport report, List<EBaoCaoSoLuong> data, string title)
        {

            var parameters = new[] { new ReportParameter("title", title) };


            using var rs = new StreamReader("./Reports/BaoCaoSoLuong.rdlc");
            report.LoadReportDefinition(rs);
            report.DataSources.Add(new ReportDataSource("Items", data));
            report.SetParameters(parameters);
        }


        public static void Load<T>(LocalReport report, List<T> data, string path, string datasetName, params ReportParameter[] reportParameters)
        {

            using var rs = new StreamReader(path);
            report.LoadReportDefinition(rs);
            report.DataSources.Add(new ReportDataSource(datasetName, data));
            if (reportParameters != null && reportParameters.Length > 0)
                report.SetParameters(reportParameters);
            // report.SetParameters(parameters);
        }
    }
}
