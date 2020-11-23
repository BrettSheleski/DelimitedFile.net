#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6
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