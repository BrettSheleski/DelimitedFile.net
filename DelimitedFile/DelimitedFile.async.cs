#if NET5_0 || NET45 || NET46|| NET47|| NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sheleski.DelimitedFile
{
    partial class DelimitedFile
    {
        public static Task WriteAsync(DelimitedFile delimitedFile, TextWriter textWriter, IDelimitedFileOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return WriteAsync(textWriter, delimitedFile.Headers, delimitedFile.Values, options, cancellationToken);
        }

        private static async Task WriteAsync(TextWriter writer, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> values, IDelimitedFileOptions options, CancellationToken cancellationToken)
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
    }
}

#endif