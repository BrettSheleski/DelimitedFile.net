using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Sheleski.DelimitedFile.MvcCore
{
    public class CsvFileResult : ActionResult
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

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ClearHeaders();
            context.HttpContext.Response.ClearContent();

            if (!string.IsNullOrWhiteSpace(Filename))
            {
                context.HttpContext.Response.AddHeader("Content-Disposition", $"attachment; filename=\"{this.Filename}\"");
            }

            context.HttpContext.Response.ContentType = "text/csv";
            context.HttpContext.Response.AddHeader("Pragma", "public");

            using (var writer = new StreamWriter(context.HttpContext.Response.OutputStream, this.Encoding))
            {
                this.CsvFile.Write(writer);
            }

            context.HttpContext.Response.End();
        }
    }
}
