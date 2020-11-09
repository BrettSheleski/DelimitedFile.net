using System;
using System.Collections.Generic;
using System.IO;

#if NET45 || NETSTANDARD
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Sheleski.DelimitedFile
{
    public partial class CsvFile : DelimitedFile
    {
        public static CsvFile Parse(string csvString)
        {
            return Parse(csvString, CsvFileLoadOptions.WithoutHeaders);
        }

        public static CsvFile Parse(string csvString, CsvFileLoadOptions options)
        {
            var reader = new StringReader(csvString);

            return Load(reader, options);
        }

        public static CsvFile Load(TextReader textReader)
        {
            return Load(textReader, CsvFileLoadOptions.WithoutHeaders);
        }

        public static CsvFile Load(TextReader textReader, CsvFileLoadOptions options)
        {
            var source = new DelimitedFileTextReaderSource(textReader, options);

            return new CsvFile
            {
                Headers = source.Headers,
                Values = source
            };
        }

        public void Write(TextWriter writer)
        {
            this.Write(writer, CsvFileOptions.WithHeaders);
        }

        public void Write(TextWriter writer, CsvFileOptions options)
        {
            Write(writer, options, this.Headers, this.Values);
        }
    }
}
