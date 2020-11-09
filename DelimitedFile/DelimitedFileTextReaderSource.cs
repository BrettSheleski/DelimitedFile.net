using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sheleski.DelimitedFile
{
    class DelimitedFileTextReaderSource : IEnumerable<IEnumerable<string>>, IDisposable
    {
        public DelimitedFileTextReaderSource(TextReader textReader, IDelimitedFileLoadOptions options)
        {
            this.TextReader = textReader;
            this.Options = options;

            if (options.FirstRowAsHeaders)
            {
                this.Headers = this.Take(1).FirstOrDefault();
            }
        }

        public TextReader TextReader { get; private set; }
        public IDelimitedFileLoadOptions Options { get; }

        public IEnumerable<string> Headers { get; private set; }

        public void Dispose()
        {
            this.TextReader?.Dispose();
            this.TextReader = null;
        }

        public IEnumerator<IEnumerable<string>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected internal IEnumerable<string> GetNextLine()
        {

            StringBuilder fieldBuilder = new StringBuilder();
            bool isFirstCharacterInField = true;

            bool isQualified = false;
            ushort consecutiveTextQualifiers = 0;

            int charsRead;
            char currentChar;
            while ((charsRead = this.TextReader.Read()) > 0)
            {
                currentChar = (char)charsRead;

                if (currentChar == Options.TextQualifier)
                {
                    if (isFirstCharacterInField)
                    {
                        // This is the first character in the field, it 
                        // must be the qualifier and mustn't be data.
                        isQualified = true;
                        consecutiveTextQualifiers = 0;
                    }
                    else
                    {
                        ++consecutiveTextQualifiers;

                        isQualified = !isQualified;

                        if (consecutiveTextQualifiers == 2)
                        {
                            consecutiveTextQualifiers = 0;
                            fieldBuilder.Append(currentChar);
                        }
                    }
                    isFirstCharacterInField = false;
                    continue;
                }
                else
                {
                    consecutiveTextQualifiers = 0;
                }

                isFirstCharacterInField = false;

                if (!isQualified && currentChar == Options.Delimiter)
                {
                    yield return fieldBuilder.ToString();

                    fieldBuilder.Clear();
                    isFirstCharacterInField = true;
                }
                else
                {
                    fieldBuilder.Append(currentChar);

                    if (!isQualified && StringBuilderEndsWith(fieldBuilder, Options.LineEnding))
                    {
                        fieldBuilder.Remove(fieldBuilder.Length - Options.LineEnding.Length, Options.LineEnding.Length);

                        yield return fieldBuilder.ToString();

                        fieldBuilder.Clear();
                        break;
                    }
                }
            }


            if (fieldBuilder.Length > 0)
            {
                yield return fieldBuilder.ToString();
            }
        }

        bool StringBuilderEndsWith(StringBuilder stringBuilder, string value)
        { 
            for(int i = 0; i < value.Length; ++i)
            {
                if (stringBuilder[stringBuilder.Length - 1 - i] != value[i])
                    return false;
            }

            return true;
        }


        class Enumerator : IEnumerator<IEnumerable<string>>
        {
            public Enumerator(DelimitedFileTextReaderSource parent)
            {
                Parent = parent;
            }

            public IEnumerable<string> Current { get; set; }

            public DelimitedFileTextReaderSource Parent { get; private set; }

            object IEnumerator.Current { get { return this.Current; } }

            public void Dispose()
            {
                this.Parent?.Dispose();
                this.Parent = null;
            }

            public bool MoveNext()
            {
                this.Current = Parent.GetNextLine();

                return this.Parent.TextReader.Peek() >= 0;
            }

            public void Reset()
            {
                throw new InvalidOperationException();
            }
        }
    }
}
