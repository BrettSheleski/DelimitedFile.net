using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheleski.DelimitedFile
{
    public abstract class DelimitedFile
    {
        public virtual IEnumerable<string> Headers { get; set; }

        public virtual IEnumerable<IEnumerable<string>> Values { get; set; }

        internal static void Write(TextWriter writer, IDelimitedFileOptions options, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> values)
        {
            bool writeNewline = options.FirstRowAsHeaders && WriteValues(writer, headers, options) > 0;

            if (values != null)
            {
                foreach (var line in values)
                {
                    if (writeNewline)
                    {
                        writer.Write(options.LineEnding);
                    }

                    WriteValues(writer, line, options);

                    writeNewline = true;
                }
            }
        }

        private static int WriteValues(TextWriter writer, IEnumerable<string> values, IDelimitedFileOptions options)
        {
            int valuesWritten = 0;

            if (values != null)
            {
                bool isFirstHeader = true;
                foreach (var value in values)
                {
                    if (!isFirstHeader)
                    {
                        writer.Write(options.Delimiter);
                    }

                    writer.Write(EscapeValue(value, options));

                    ++valuesWritten;

                    isFirstHeader = false;
                }
            }

            return valuesWritten;
        }


        private static async Task<int> WriteValuesAsync(TextWriter writer, IEnumerable<string> values, IDelimitedFileOptions options, CancellationToken cancellationToken)
        {
            int valuesWritten = 0;

            if (values != null)
            {
                bool isFirstHeader = true;
                foreach (var value in values)
                {
                    if (!isFirstHeader)
                    {
                        await writer.WriteAsync(options.Delimiter);
                    }

                    await writer.WriteAsync(EscapeValue(value, options));

                    ++valuesWritten;
                    isFirstHeader = false;

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            }

            return valuesWritten;
        }

        private static string EscapeValue(string value, IDelimitedFileOptions options)
        {
            if (
                (options.TextQualifier != null && value.Any(c => c == options.Delimiter || c == options.TextQualifier)) ||
                (options.LineEnding != null && value.Contains(options.LineEnding))
                )
            {
                string oneQuote = options.TextQualifier.ToString();
                string twoQuotes = $"{options.TextQualifier}{options.TextQualifier}";

                value = $"{options.TextQualifier}{value.Replace(oneQuote, twoQuotes)}{options.TextQualifier}";
            }

            return value;
        }

        internal static async Task WriteAsync( TextWriter writer, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> values, IDelimitedFileOptions options, CancellationToken cancellationToken)
        {
            bool writeNewline = options.FirstRowAsHeaders && (await WriteValuesAsync(writer, headers, options, cancellationToken)) > 0;

            if (values != null && !cancellationToken.IsCancellationRequested)
            {
                foreach (var line in values)
                {
                    if (writeNewline)
                    {
                        await writer.WriteAsync(options.LineEnding);
                    }

                    await WriteValuesAsync(writer, line, options, cancellationToken);

                    writeNewline = true;

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            }
        }
    }
}
