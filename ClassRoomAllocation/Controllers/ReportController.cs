using BusinessLogicLayer.Admin;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClassRoomAllocation.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        [Route("Report")]
        public async Task<HttpResponseMessage> GetReport()
        {
            LocalReport lr = new LocalReport();

            string paths = System.Web.HttpContext.Current.Server.MapPath("/Report/Report1.rdlc");
            if (System.IO.File.Exists(paths))
            {
                lr.ReportPath = paths;
            }

            AdminActivity activity = new AdminActivity();

            var teachers = await activity.getTeachers();

            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", teachers);
            lr.DataSources.Add(reportDataSource);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            int length = renderedBytes.Length;
            MemoryStream outputStream = new MemoryStream();
            outputStream.Write(renderedBytes, 0, length);
            outputStream.Position = 0;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(outputStream);
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            return response;
        }
    }
}
