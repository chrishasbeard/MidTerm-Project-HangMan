using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MidTernProj_HangMan
{
    class HangMan
    {
        public int Misses { get; set; }
        public string Guessed { get; set; }
        public string MysteryWord { get; set; }


        public HangMan()
        {        }

     



        public static string GetUserInput()
        {
            Console.WriteLine("Pick a letter!");
            string message = Console.ReadLine();
            if (Regex.IsMatch(message, @"[a-z]"))
            {
                return message;
            }
            return GetUserInput();
        }


        public static void StartHangMan()
        {
            Console.WriteLine("Pick a letter!");

        }
    }
}
