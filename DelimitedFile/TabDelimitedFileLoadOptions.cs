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

        public int BufferSize { get; set; } = 4096;

#if NET5_0
        public override TabDelimitedFileLoadOptions WithLineEnding(string lineEndings)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = lineEndings,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override TabDelimitedFileLoadOptions WithFirstRowAsHeaders(bool firstRowAsHeaders = true)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = firstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override TabDelimitedFileLoadOptions WithTextQualifier(char? textQualifier)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = textQualifier,
                BufferSize = this.BufferSize
            };
        }

        public virtual TabDelimitedFileLoadOptions WithBufferSize(int bufferSize)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier,
                BufferSize = bufferSize
            };
        }
#else
        public override TabDelimitedFileOptions WithLineEnding(string lineEndings)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = lineEndings,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override TabDelimitedFileOptions WithFirstRowAsHeaders(bool firstRowAsHeaders = true)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = firstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override TabDelimitedFileOptions WithTextQualifier(char? textQualifier)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = textQualifier,
                BufferSize = this.BufferSize
            };
        }

        public virtual TabDelimitedFileOptions WithBufferSize(int bufferSize)
        {
            return new TabDelimitedFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier,
                BufferSize = bufferSize
            };
        }
#endif

        
    }
}
