using System.IO;

namespace Sheleski.DelimitedFile
{
    public class TabDelimitedFile : DelimitedFile
    {
        public static TabDelimitedFile Parse(string tabString)
        {
            return Parse(tabString, TabDelimitedFileOptions.WithoutHeaders);
        }

        public static TabDelimitedFile Parse(string tabString, TabDelimitedFileLoadOptions options)
        {
            var reader = new StringReader(tabString);

            return Load(reader, options);
        }

        public static TabDelimitedFile Load(TextReader textReader)
        {
            return Load(textReader, TabDelimitedFileLoadOptions.WithoutHeaders);
        }

        public static TabDelimitedFile Load(TextReader textReader, TabDelimitedFileLoadOptions options)
        {
            var source = new DelimitedFileTextReaderSource(textReader, options);

            return new TabDelimitedFile
            {
                Headers = source.Headers,
                Values = source
            };
        }

        public void Write(TextWriter writer)
        {
            base.Write(writer, TabDelimitedFileOptions.WithHeaders);
        }

        public void Write(TextWriter writer, TabDelimitedFileOptions options)
        {
            base.Write(writer, options);
        }
    }
}
