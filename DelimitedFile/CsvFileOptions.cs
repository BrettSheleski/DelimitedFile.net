namespace Sheleski.DelimitedFile
{
    public class TabDelimitedFileLoadOptions : TabDelimitedFileOptions, IDelimitedFileLoadOptions
    {
        public static new TabDelimitedFileLoadOptions WithHeaders = new TabDelimitedFileLoadOptions
        {
            FirstRowAsHeaders = true
        };

        public static new TabDelimitedFileLoadOptions WithoutHeaders = new TabDelimitedFileLoadOptions
        {
            FirstRowAsHeaders = false
        };

        public int BufferSize { get; set; } = 1024;
    }

    public class TabDelimitedFileOptions : IDelimitedFileOptions
    {
        public static TabDelimitedFileOptions WithHeaders = new TabDelimitedFileOptions
        {
            FirstRowAsHeaders = true
        };

        public static TabDelimitedFileOptions WithoutHeaders = new TabDelimitedFileOptions
        {
            FirstRowAsHeaders = false
        };

        public string LineEnding { get; set; } = "\n";
        public char? TextQualifier { get; set; } = '"';
        public bool FirstRowAsHeaders { get; set; } = false;
        char IDelimitedFileOptions.Delimiter { get; } = '\t';
    }

    public class CsvFileOptions : IDelimitedFileOptions
    {
        public static CsvFileOptions WithHeaders = new CsvFileOptions
        {
            FirstRowAsHeaders = true
        };

        public static CsvFileOptions WithoutHeaders = new CsvFileOptions
        {
            FirstRowAsHeaders = false
        };

        public string LineEnding { get; set; } = "\n";
        public char? TextQualifier { get; set; } = '"';
        public bool FirstRowAsHeaders { get; set; } = false;
        char IDelimitedFileOptions.Delimiter { get; } = ',';
    }
}
