using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace MidTernProj_HangMan
{
    class HangMan
    {
        //public int Misses { get; set; }
        // public string Guessed { get; set; }
        // public Word word { get; set; }

        public HangMan()
        {

        }


        public static char GetUserInput()
        {
            Console.WriteLine("Pick a letter!");
            string message = Console.ReadLine().ToLower();
            if (Regex.IsMatch(message, @"^[a-z]{1}$"))
            {
                return message[0];
            }
            return GetUserInput();
        }

        public static string GetDifficulty()
        {
            Console.WriteLine("Welcome to Dynamite-man! the game that will blow your socks off, literally.\n\nLet's play!");
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
            GuessingGame(userWord);
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
            while (dynamite.Contains("*"))
            {

            }
            if (length == 0)
            {
                Console.WriteLine("\\**|**/\n" +
                                  "*\\*|*/*\n" +
                                  "**\\|/**\n" +
                                  "---*---\n" +
                                  "**/|\\**\n" +
                                  "*/*|*\\*\n" +
                                  "/**|**\\\n" +
                                  "");
            }
            else
            {
                Console.WriteLine(dynamite);
            }
        }
        public static void GuessingGame(Word word)
        {
            int missCount = 0;
            int misses = 0;
            bool checkLoss = false;
            string mysteryWord = word.MysteriousName;
            char[] guess = new char[mysteryWord.Length];
            bool checkWin = false;
            if (word.Difficulty == "easy")
            {
                missCount = mysteryWord.Length + 3;
            }
            else if (word.Difficulty == "medium")
            {
                missCount = mysteryWord.Length;
            }
            else if (word.Difficulty == "hard")
            {
                missCount = mysteryWord.Length - 1;
            }


            //HangMan displayToPlayer = new HangMan();
            for (int i = 0; i < mysteryWord.Length; i++)
            {
                Console.Write("*");
                //Console.WriteLine("");
                guess[i] = '*';
            }

            while (misses != missCount)
            {

                Console.WriteLine();
                checkWin = true;
                char playerGuess = GetUserInput();
                for (int j = 0; j < mysteryWord.Length; j++)
                {
                    if (playerGuess == mysteryWord[j])
                    {
                        guess[j] = playerGuess;
                        checkLoss = true;
                    }
                }
                for (int i = 0; i < guess.Length; i++)
                {
                    if (guess[i] == '*')
                    {
                        checkWin = false;
                    }
                }
                if (checkWin)
                {
                    Console.WriteLine("You win!!!");
                    // add counter to put tally in scoreboard
                    break;
                }
                if (!checkLoss)
                {
                    misses++;

                }
                Console.WriteLine(guess);
            }
        }
    }
}
