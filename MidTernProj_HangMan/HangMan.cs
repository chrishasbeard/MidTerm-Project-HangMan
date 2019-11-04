using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

// Jack - saving of wins and losses - simple password verification

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

        public static Player VerifyLogIn()
        {
            Player.GrabPlayers();
            Console.WriteLine("What is your user name?");
            string nameEntered = Console.ReadLine().ToLower();
            bool checkExisting = Player.CheckUserName(nameEntered);
            if (!checkExisting)
            {
                Console.WriteLine("This user name doesn't exist");
                return VerifyLogIn();
            }
            Console.WriteLine("What is your password?");
            string passwordEntered = Console.ReadLine().ToLower();
            Player player = Player.CheckPassword(nameEntered, passwordEntered);
            if (player.UserName == "Fake player")
            {
                Console.WriteLine("Wrong Password!");
                return VerifyLogIn();
            }

            return player;
        }

        public static Player AddNewUser()
        {
            Console.WriteLine("What will be your user name?");
            string userName = Console.ReadLine();
            Console.WriteLine("Your password must contain a number, a symbol (not \"|\"), and must be atleast 4 characters.");
            bool badPassword = true;
            string password = "";
            string passPattern = @"^(?!.*\|)(?=.*[0-9])(?=.*\W)(?=.*\w).{4,}$";
            while (badPassword)
            {
                password = Console.ReadLine();
                if (Regex.IsMatch(password, passPattern))
                {
                    badPassword = false;
                }
                Console.WriteLine("Invalid password your password must contain a number, a symbol (not \"|\"), and must be atleast 4 characters.");
            }

            Player newPlayer = new Player(userName, password, 0, 0, 0);
            Player.AddPlayer(newPlayer);
            return newPlayer;
        }

        public static Player LogInUser()
        {
            Console.WriteLine("Do you already have an account in the game?");
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "y")
            {
                return VerifyLogIn();
            }
            else
            {
                return AddNewUser();
            }
        }

        public static void StartHangMan()
        {
            Player player = LogInUser();
            do
            {

                Word userWord = PickWord(GetDifficulty());
                if (GuessingGame(userWord))
                {
                    player.WinNum++;
                }
                else
                {
                    player.Losses++;
                }
                Player.CheckHighScore();
                Player.CalcWinLoss(player);
                Player.WriteScore();
            } while (KeepPlaying());
        }

        

        public static bool KeepPlaying()
        {
            Console.WriteLine($"\n Would you like to keep playing?: (y/n) ");
            string message = Console.ReadLine();
            if (message == "y" || message == "yes")
            {
                return true;
            }
            else if (message == "n" || message == "no")
            {
                return false;
            }
            else
            {
                return KeepPlaying();
            }
        }

        public static Word PickWord(string difficulty)
        {
            List<Word> words = new List<Word>();
            StreamReader reader = new StreamReader("../../../WordList.txt");
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
            Word word = words[random.Next(words.Count)];
            return word;
        }

        public static void DisplayDynamite(int length)
        {

            string dynamite = "\n______________\n" +
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
        public static bool GuessingGame(Word word)
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
                guess[i] = '_';
            }

            while (misses != missCount)
            {
                DisplayDynamite(missCount - misses);
                Console.WriteLine();
                checkWin = true;
                checkLoss = false;
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
                    if (guess[i] == '_')
                    {
                        checkWin = false;
                    }
                }
                if (checkWin)
                {
                    Console.WriteLine("YOU WON!!!");
                    return true;

                    // add counter to put tally in scoreboard

                }
                if (!checkLoss)
                {
                    misses++;

                }
                Console.WriteLine(guess);
            }
            if (misses == missCount)
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine("          _ ._  _ , _ ._");
                    Console.WriteLine("        (_ ' ( `  )_  .__)");
                    Console.WriteLine("      ( (  (    )   `)  ) _)");
                    Console.WriteLine("     (__ (_   (_ . _) _) ,__)");
                    Console.WriteLine("         `~~`\\ ' . /`~~`");
                    Console.WriteLine("              ;    ;");
                    Console.WriteLine("              /   \\");
                    Console.WriteLine("_____________/_ _ _\\_____________");
                    Thread.Sleep(50);
                    Console.Clear();

                }
                Console.WriteLine("You Lost.");
                return false;
            }
            else
            {
                return false;
            }

        }
    }
}
