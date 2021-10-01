using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sheleski.DelimitedFile.Converter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConverterOptions options = new ConverterOptions()
            {
                SourceOptions = CsvFileLoadOptions.WithHeaders,
                OutputOptions = TabDelimitedFileOptions.WithHeaders
            };

            TextReader reader;
            TextWriter writer;

            string url = null;

            if (args.Length > 0)
            {
                url = args[0];
            }

            using (reader = new System.IO.StreamReader(await GetSourceStreamAsync(url)))
            await using (writer = new System.IO.StreamWriter(Console.OpenStandardOutput()))
            {

                var converter = new Converter(options);

                CancellationTokenSource cts = new CancellationTokenSource();

                await converter.ConvertAsync(reader, writer, cts.Token);
            }
        }

        private static async Task<Stream> GetSourceStreamAsync(string path)
        {
            if (path == null)
                return Console.OpenStandardInput();

            Uri uri;

            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                var request = System.Net.HttpWebRequest.CreateHttp(uri);

                var response = await request.GetResponseAsync();

                return response.GetResponseStream();
            }
            else if (System.IO.File.Exists(path))
            {
                return new FileStream(path, FileMode.Open, FileAccess.Read);
            }
            else
            {
                throw new FileNotFoundException("File not found", path);
            }
        }
    }

    class ConverterOptions
    {
        public IDelimitedFileLoadOptions SourceOptions { get; set; }
        public IDelimitedFileOptions OutputOptions { get; set; }
    }

    class Converter
    {
        public Converter(ConverterOptions options)
        {
            this.Options = options;
        }

        public ConverterOptions Options { get; }

        public async Task ConvertAsync(TextReader reader, TextWriter writer, CancellationToken cancellationToken)
        {
            DelimitedFile source = DelimitedFileLoader.Load(reader, this.Options.SourceOptions);

            await DelimitedFile.WriteAsync(source, writer, this.Options.OutputOptions, cancellationToken);
        }
    }
}
