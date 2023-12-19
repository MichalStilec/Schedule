using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Watchdog
    {
        public Stopwatch Stopwatch { get; }

        public Watchdog()
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }
    }
}
