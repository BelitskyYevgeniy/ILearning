using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace Task_3_CS
{
    class Program
    {
        const int ExitCode = 0;
        static bool IsWin(int compStep, int humanStep, int length)
        {
            int half = length / 2;
            int delta = humanStep - compStep;
            return delta <= half && delta > 0 || length + delta <= half;
        }
        static byte[] computeHMAC(int bound, out byte[] key, out int step)
        {

            key = new byte[16];
            RandomNumberGenerator.Fill(key);
            step = RandomNumberGenerator.GetInt32(1, bound);
            HMACSHA256 hmac = new HMACSHA256(key);
            return hmac.ComputeHash(BitConverter.GetBytes(step));


        }
        static void printMenu(string[] str)
        {
            Console.WriteLine("Available moves:");
            for (int i = 1; i <= str.Length; i++)
                Console.WriteLine(i + " - " + str[i - 1]);
            Console.WriteLine(ExitCode+" - Exit");
        }
        static bool check(String[] str)
        {
            if (str.Length < 3 || str.Length % 2 == 0)
                return false;
            var set = new SortedSet<string>(str);
            return set.Count == str.Length;
        }
        static string BytesToString(byte[] hmac)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var b in hmac)
                sb.Append(b.ToString("x"));
            return sb.ToString();
        }
        static void Main(string[] args)
        {
            if (!check(args))
            {
                Console.WriteLine("Arguments are not valid:\n" +
                                  "\t1) Count of arguments mustn't be less than 3!\n" +
                                  "\t2) Count of arguments is odd number!\n" +
                                  "\t3) List of arguments can not have identical line!\n" +
                                  "True example: rock paper scissors lizard Spock");
                return;
            }
            byte[] key;
            int stepComp, stepHuman;
            byte[] hmac=computeHMAC(args.Length+1,out key, out stepComp);
            Console.WriteLine("HMAC: " + BytesToString(hmac));
            do
            {
                printMenu(args);
                Console.Write("Enter your move: ");
            }
            while (!Int32.TryParse(Console.ReadLine(), out stepHuman) || stepHuman > args.Length || stepHuman < ExitCode);

            if (stepHuman == ExitCode)
            {
                Console.WriteLine("Game over");
                return;
            }
            Console.WriteLine("Your move: "+args[stepHuman-1]);
            
            Console.WriteLine("Computer move: " + args[stepComp-1]);
            string mess = (stepHuman == stepComp)? "Draw": IsWin(stepComp, stepHuman, args.Length)? "You win!":"You losе!";
            Console.WriteLine(mess);
           
            Console.WriteLine("HMAC key: " + BytesToString(key));
            return;
        }
    }
}
