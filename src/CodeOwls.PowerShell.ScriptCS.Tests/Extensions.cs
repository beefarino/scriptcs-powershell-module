using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeOwls.PowerShell.ScriptCS.Tests
{
    static public class Extensions
    {
        static public IEnumerable<T> NotNull<T>(this IEnumerable<T> _this)
        {
            return _this.Where(a => null != a);
        }
        
    }
}
