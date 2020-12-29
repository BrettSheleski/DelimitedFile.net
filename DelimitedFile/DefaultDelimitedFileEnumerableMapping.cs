using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheleski.DelimitedFile
{
    internal class DefaultDelimitedFileEnumerableMapping<T> : IDelimitedFileEnumerableMapping<T>
    {
        public DefaultDelimitedFileEnumerableMapping()
        {

        }

        private bool _hasMappings = false;

        public T GetInstance(DelimitedFile delimitedFile, IEnumerable<string> line)
        {
            GenerateMapping();

            throw new System.NotImplementedException();
        }

        private void GenerateMapping()
        {
            if (!_hasMappings)
            {

            }
        }
    }
}