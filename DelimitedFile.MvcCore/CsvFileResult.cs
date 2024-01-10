using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sheleski.DelimitedFile.MvcCore
{
    public class CsvFileResult : IActionResult
    {
        public CsvFileResult(CsvFile csvFile, string fileName) : this(csvFile, fileName, Encoding.Default)
        {


        }

        public CsvFileResult(CsvFile csvFile, string fileName, Encoding encoding)
        {
            if (csvFile == null)
                throw new ArgumentNullException(nameof(csvFile));

            this.CsvFile = csvFile;
            this.Filename = fileName;
            this.Encoding = encoding;
        }

        public CsvFile CsvFile { get; }
        public string Filename { get; }
        public Encoding Encoding { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Clear();

            if (!string.IsNullOrWhiteSpace(Filename))
            {
                context.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{this.Filename}\"");
            }

            context.HttpContext.Response.ContentType = "text/csv";
            context.HttpContext.Response.Headers.Add("Pragma", "public");

#if NET5_0_OR_GREATER
            await
#endif
            using (var writer = new StreamWriter(context.HttpContext.Response.Body, this.Encoding))
            {
                await this.CsvFile.WriteAsync(writer);
            }
        }
    }
}
