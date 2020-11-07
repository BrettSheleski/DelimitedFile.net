using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sheleski.DelimitedFile
{
    public abstract class DelimitedFile
    {
        public virtual IEnumerable<string> Headers { get; set; }

        public virtual IEnumerable<IEnumerable<string>> Values { get; set; }

        internal void Write(TextWriter writer, IDelimitedFileOptions options)
        {
            bool writeNewline = options.FirstRowAsHeaders && WriteValues(writer, Headers, options) > 0;

            if (Values != null)
            {
                foreach (var line in Values)
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
