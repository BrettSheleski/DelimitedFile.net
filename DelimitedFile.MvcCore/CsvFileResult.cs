using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sheleski.DelimitedFile.MvcCore
{
    public class CsvFileResult : IActionResult
    {
        public CsvFileResult(CsvFile csvFile, string fileName)
        {
            if (csvFile == null)
                throw new ArgumentNullException(nameof(csvFile));

            this.CsvFile = csvFile;
            this.Filename = fileName;
        }

        public CsvFile CsvFile { get; }
        public string Filename { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Clear();

            if (!string.IsNullOrWhiteSpace(Filename))
            {
                context.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{this.Filename}\"");
            }

            context.HttpContext.Response.ContentType = "text/csv";
            context.HttpContext.Response.Headers.Add("Pragma", "public");

            using (var writer = new StreamWriter(context.HttpContext.Response.Body))
            {
                await this.CsvFile.WriteAsync(writer);
            }
        }
    }
}
