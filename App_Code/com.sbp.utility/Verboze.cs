using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingForexService.com.sbp.utility
{
    public static class Verboze
    {
        public static void Echo(string txt)
        {
            Console.WriteLine(txt);
        }

        public static string Hear()
        {
            return Console.ReadLine();
        }
    }
}
