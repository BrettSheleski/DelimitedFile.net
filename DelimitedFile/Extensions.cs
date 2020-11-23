using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sheleski.DelimitedFile
{
    public static class Extensions
    {
#if NET5_0 || NET45 || NET46|| NET47|| NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
        public static CsvFile ToCsvFile<T>(this IEnumerable<T> source)
        {
            DelimitedFileObjectMapping<T> mapping = DelimitedFileObjectMapping<T>.GetDefault();

            return ToCsvFile(source, mapping);
        }

        public static CsvFile ToCsvFile<T>(this IEnumerable<T> source, DelimitedFileObjectMapping<T> mapping)
        {
            return new CsvFile
            {
                Headers = mapping.Columns.Select(x => x.Header),
                Values = source.Select((x, i) => mapping.Columns.Select(col => col.GetValue(i, x)))
            };
        }

        public static TabDelimitedFile ToTabDelimitedFile<T>(this IEnumerable<T> source)
        {
            DelimitedFileObjectMapping<T> mapping = DelimitedFileObjectMapping<T>.GetDefault();

            return ToTabDelimitedFile(source, mapping);
        }

        public static TabDelimitedFile ToTabDelimitedFile<T>(this IEnumerable<T> source, DelimitedFileObjectMapping<T> mapping)
        {
            return new TabDelimitedFile
            {
                Headers = mapping.Columns.Select(x => x.Header),
                Values = source.Select(x => mapping.Columns.Select((c, i) => c.GetValue(i, x)))
            };
        }
#endif
    }
}
