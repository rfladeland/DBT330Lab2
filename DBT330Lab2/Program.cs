using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBT330Lab2
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MangoManipulator mangoManipulator = new MangoManipulator();
            mangoManipulator.Run();
        }
    }
}
