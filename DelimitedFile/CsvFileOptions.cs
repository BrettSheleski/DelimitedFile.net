﻿namespace Sheleski.DelimitedFile
{

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

        public string LineEnding { get; protected set; } = "\n";
        public char? TextQualifier { get; protected set; } = '"';
        public bool FirstRowAsHeaders { get; protected set; } = false;
        char IDelimitedFileOptions.Delimiter { get; } = ',';

        public virtual CsvFileOptions WithLineEnding(string lineEndings)
        {
            return new CsvFileOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = lineEndings,
                TextQualifier = this.TextQualifier
            };
        }

        public virtual CsvFileOptions WithFirstRowAsHeaders(bool firstRowAsHeaders = true)
        {
            return new CsvFileOptions
            {
                FirstRowAsHeaders = firstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = this.TextQualifier
            };
        }

        public virtual CsvFileOptions WithTextQualifier(char? textQualifier)
        {
            return new CsvFileOptions
            {
                FirstRowAsHeaders = this.FirstRowAsHeaders,
                LineEnding = this.LineEnding,
                TextQualifier = textQualifier
            };
        }
    }
}
