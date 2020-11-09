#if NETSTANDARD1_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sheleski.DelimitedFile
{
    partial class PolyfillExtensions
    {
        public static bool Any(this string value, Func<char, bool> func)
        {
            foreach(char c in value){
                if (func(c))
                    return true;
            }

            return false;
        }
    }
}
#endif