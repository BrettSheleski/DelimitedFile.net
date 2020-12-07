namespace Sheleski.DelimitedFile
{
    public interface IDelimitedFileOptions
    {
        string LineEnding { get; }
        char? TextQualifier { get; }
        bool FirstRowAsHeaders { get; }
        char Delimiter { get; }
    }

    public class DelimitedFileOptions : IDelimitedFileOptions
    {
        public DelimitedFileOptions(char delimiter)
        {
            Delimiter = delimiter;
        }

        public string LineEnding { get; set; } = "\n";

        public char? TextQualifier { get; set; } = '"';

        public bool FirstRowAsHeaders { get; set; } = true;

        public char Delimiter { get; }
    }

    public class DelimitedFileLoadOptions : DelimitedFileOptions, IDelimitedFileLoadOptions
    {
        public DelimitedFileLoadOptions(char delimiter) : base(delimiter)
        {
        }

        public int BufferSize { get; set; } = 4096;
    }
}
