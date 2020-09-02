using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiptosRageConfig.Memory;

namespace RiptosRageConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            Reader read = new Reader();
            read.Read();
            Console.ReadLine();
        }
    }
}
