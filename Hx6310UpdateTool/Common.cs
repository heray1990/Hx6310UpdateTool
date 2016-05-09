using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx6310UpdateTool
{
    class Common
    {
        #region CONSTANT_VALUES

        #endregion

        public static long DisBit(long m, long n)
        {
            m = m & (0xFFFFFFFF - 2 ^ n);
            return m;
        }
    }
}
