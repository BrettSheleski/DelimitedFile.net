#if NET35
using System.Text;

namespace Sheleski.DelimitedFile
{
    partial class PolyfillExtensions
    {
        public static void Clear(this StringBuilder stringBuilder)
        {
            stringBuilder.Length = 0;
        }
    }
}
#endif