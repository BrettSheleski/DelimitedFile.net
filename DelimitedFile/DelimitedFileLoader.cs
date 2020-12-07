using System;
using System.IO;

namespace Sheleski.DelimitedFile
{
    public static partial class DelimitedFileLoader
    {
        public static DelimitedFile Load(TextReader reader, IDelimitedFileLoadOptions options)
        {
            var source = new DelimitedFileTextReaderSource(reader, options);

            return new DelimitedFile
            {
                Headers = source.Headers,
                Values = source
            };
        }
    }
}
