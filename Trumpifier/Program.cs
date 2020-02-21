using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trumpifier
{
    class Program
    {
        static void Main(string[] args)
        {
            InputData data = new InputData("testinput.txt");

            Console.Write(data.GetNextSentence());
            Console.Write("\n");

            ConsoleKeyInfo c = Console.ReadKey();
            while(c.KeyChar != 'q')
            {
                Console.Write("\n");
                Console.Write(data.GetNextSentence());
                Console.Write("\n");
                c = Console.ReadKey();
            }
        }
    }
}
