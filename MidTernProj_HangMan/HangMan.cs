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

        public static string GetDifficulty()
        {
            Console.WriteLine("Pick a difficulty. (Easy, Medium, Hard)");
            string message = Console.ReadLine().ToLower();
            if (message == "easy" || message == "medium" || message == "hard")
            {
                return message;
            }
            return GetDifficulty();
        }


        public static void StartHangMan()
        {
            Word userWord = PickWord(GetDifficulty());
            Console.WriteLine(userWord.MysteriousName);
            DisplayDynamite(5);
        }

        public static Word PickWord(string difficulty)
        {
            List<Word> words = new List<Word>();
            StreamReader reader = new StreamReader("../../../TextFile1.txt");
            string line = reader.ReadLine();
            Random random = new Random();
            while (line != null)
            {
                string[] splitWords = line.Split("|");
                if (splitWords[1] == difficulty)
                {
                    words.Add(new Word(splitWords[0], splitWords[1]));
                }
                line = reader.ReadLine();
            }
            reader.Close();
            Word word = words[random.Next(words.Count + 1)];
            return word;
        }

        public static void DisplayDynamite(int length)
        {
            string dynamite = "______________\n" +
                              "|  Dynamite  |";
            for (int i = 0; i < length; i++)
            {
                if (i != length - 1)
                {
                    dynamite += "-";
                }
                else
                {
                    dynamite += "*";
                }
            }
            dynamite += "\n‾‾‾‾‾‾‾‾‾‾‾‾‾‾";
            Console.WriteLine(dynamite);

        }
    }
}
