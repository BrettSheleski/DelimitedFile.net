#if !NETSTANDARD1_0
using System.IO;

namespace Sheleski.DelimitedFile
{
    partial class CsvFile
    {
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
    }
}
#endif