using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if NET45 || NETSTANDARD
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Sheleski.DelimitedFile
{
    public partial class DelimitedFile
    {
        public virtual IEnumerable<string> Headers { get; set; }

        public virtual IEnumerable<IEnumerable<string>> Values { get; set; }

        internal static void Write(TextWriter writer, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> values, IDelimitedFileOptions options)
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

        private static string EscapeValue(string value, IDelimitedFileOptions options)
        {
            if (value == null)
                return null;

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
    }
}
