using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sheleski.DelimitedFile
{
    public class CsvFile : DelimitedFile
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


        public Task WriteAsync(TextWriter writer)
        {
            return WriteAsync(writer, CsvFileOptions.WithHeaders, CancellationToken.None);
        }

        public Task WriteAsync(TextWriter writer, CsvFileOptions options)
        {
            return WriteAsync(writer, options, CancellationToken.None);
        }

        public Task WriteAsync(TextWriter writer, CancellationToken cancellationToken)
        {
            return WriteAsync(writer, CsvFileOptions.WithHeaders, cancellationToken);
        }

        public async Task WriteAsync(TextWriter writer, CsvFileOptions options, CancellationToken cancellationToken)
        {
            await WriteAsync(writer, this.Headers, this.Values, options, cancellationToken);
        }




        public void Write(TextWriter writer)
        {
            this.Write(writer, CsvFileOptions.WithHeaders);
        }

        public void Write(TextWriter writer, CsvFileOptions options)
        {
            Write(writer, options, this.Headers, this.Values);
        }

        public void Save(string filePath)
        {
            Save(filePath, CsvFileOptions.WithHeaders);
        }

        public void Save(string filePath, CsvFileOptions options)
        {
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(stream))
            {
                Write(writer, options);
            }
        }


        public Task SaveAsync(string filePath)
        {
            return SaveAsync(filePath, CancellationToken.None);
        }

        public Task SaveAsync(string filePath, CancellationToken cancellationToken)
        {
            return SaveAsync(filePath, CsvFileOptions.WithHeaders, cancellationToken);
        }

        public Task SaveAsync(string filePath, CsvFileOptions options)
        {
            return SaveAsync(filePath, options, CancellationToken.None);
        }

        public async Task SaveAsync(string filePath, CsvFileOptions options, CancellationToken cancellationToken)
        {
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(stream))
            {
                await WriteAsync(writer, options, cancellationToken);
            }
        }

    }
}
