using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace MidTernProj_HangMan
{
    class HangMan
    {
        public int Misses { get; set; }
        public string Guessed { get; set; }
        public Word word { get; set; }

        public HangMan()
        {

        }


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
            PickWord();
            GetUserInput();
        }

        public static void PickWord()
        {
            List<Word> words = new List<Word>();
            StreamReader reader = new StreamReader("../../../TextFile1.txt");
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] splitWords = line.Split("|");
                words.Add(new Word(splitWords[0], splitWords[1]));
                line = reader.ReadLine();
            }
            reader.Close();
            foreach (var word in words)
            {
                Console.WriteLine(word.MysteriousName);
            }
        }
    }
}
