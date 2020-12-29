using System.Collections.Generic;

namespace Sheleski.DelimitedFile
{
    public interface IDelimitedFileEnumerableMapping<T>
    {
        T GetInstance(DelimitedFile delimitedFile, IEnumerable<string> line);
    }
}