namespace Sheleski.DelimitedFile
{
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

        public string LineEnding { get; protected set; } = "\n";
        public char? TextQualifier { get; protected set; } = '"';
        public bool FirstRowAsHeaders { get; protected set; } = false;
        char IDelimitedFileOptions.Delimiter { get; } = '\t';

        public virtual TabDelimitedFileOptions WithLineEnding(string lineEndings)
        {
            return new TabDelimitedFileOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = lineEndings,
                TextQualifier = this.TextQualifier
            };
        }

        public virtual TabDelimitedFileOptions WithFirstRowAsHeaders(bool firstRowAsHeaders = true)
        {
            return new TabDelimitedFileOptions
            {
                FirstRowAsHeaders = firstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier
            };
        }

        public virtual TabDelimitedFileOptions WithTextQualifier(char? textQualifier)
        {
            return new TabDelimitedFileOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = textQualifier
            };
        }
    }
}
