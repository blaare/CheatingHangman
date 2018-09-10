using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatingHangman.src
{
    class SingleCharWordTest
    {
        public static void test()
        {
            List<string> test = new List<string>();

            test.Add("abb");
            test.Add("acc");
            test.Add("add");

            Dictionary<int, List<string>> subFamily = WordBank.SingleCharWord(test, 'a');

            int choice = WordBank.PickAFamily(subFamily);
            Console.WriteLine("Should be 0");
            Console.WriteLine(choice);

            test = new List<string>();

            test.Add("bab");
            test.Add("cac");
            test.Add("dad");

            subFamily = WordBank.SingleCharWord(test, 'a');

            choice = WordBank.PickAFamily(subFamily);
            Console.WriteLine("Should be 1");
            Console.WriteLine(choice);

            test = new List<string>();

            test.Add("bba");
            test.Add("cca");
            test.Add("dda");

            subFamily = WordBank.SingleCharWord(test, 'a');

            choice = WordBank.PickAFamily(subFamily);
            Console.WriteLine("Should be 2");
            Console.WriteLine(choice);

            Console.ReadLine();
        }
    }
}
