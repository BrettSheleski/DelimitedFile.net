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

        public int BufferSize { get; set; } = 4096;


#if NET5_0
        public override CsvFileLoadOptions WithLineEnding(string lineEndings)
        {
            return new CsvFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = lineEndings,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override CsvFileLoadOptions WithFirstRowAsHeaders(bool firstRowAsHeaders = true)
        {
            return new CsvFileLoadOptions
            {
                FirstRowAsHeaders = firstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override CsvFileLoadOptions WithTextQualifier(char? textQualifier)
        {
            return new CsvFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = textQualifier,
                BufferSize = this.BufferSize
            };
        }

        public virtual CsvFileLoadOptions WithBufferSize(int bufferSize)
        {
            return new CsvFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier,
                BufferSize = bufferSize
            };
        }
#else
        public override CsvFileOptions WithLineEnding(string lineEndings)
        {
            return new CsvFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = lineEndings,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override CsvFileOptions WithFirstRowAsHeaders(bool firstRowAsHeaders = true)
        {
            return new CsvFileLoadOptions
            {
                FirstRowAsHeaders = firstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier,
                BufferSize = this.BufferSize
            };
        }

        public override CsvFileOptions WithTextQualifier(char? textQualifier)
        {
            return new CsvFileLoadOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = textQualifier,
                BufferSize = this.BufferSize
            };
        }

        public virtual CsvFileOptions WithBufferSize(int bufferSize)
        {
            return new CsvFileLoadOptions
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
