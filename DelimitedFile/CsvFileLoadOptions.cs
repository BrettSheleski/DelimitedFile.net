namespace Sheleski.DelimitedFile
{
    public class CsvFileLoadOptions : CsvFileOptions, IDelimitedFileLoadOptions
    {
        public static new CsvFileLoadOptions WithHeaders { get; } = new CsvFileLoadOptions
        {
            FirstRowAsHeaders = true
        };

        public static new CsvFileLoadOptions WithoutHeaders { get; } = new CsvFileLoadOptions
        {
            FirstRowAsHeaders = true
        };

        public int BufferSize { get; set; } = 1024;
    }
}
