using System;

namespace Sheleski.DelimitedFile
{
    internal class DelimitedFileEnumerableMapper
    {
        internal static IDelimitedFileEnumerableMapping<T> GetDefault<T>()
        {
            return new DefaultDelimitedFileEnumerableMapping<T>();
        }
    }
}