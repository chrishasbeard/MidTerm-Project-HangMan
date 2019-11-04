using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Security.Cryptography;



namespace MidTernProj_HangMan
{
    class HangMan
    {


        public HangMan()
        {

        }

        //this will start the game.
        public static void StartHangMan()
        {
            Player player = LogInUser();
            do
            {
                Console.WriteLine(player.UserName);
                Word userWord = PickWord(GetDifficulty());
                if (GuessingGame(userWord))
                {
                    player.WinNum++;
                }
                else
                {
                    player.Losses++;
                }
                Player.CalcWinLoss(player);
                Player.CheckHighScore();
                Player.WriteScore();
            } while (KeepPlaying());
        }

        //this will check if user wants to log on to an account or create a new account.
        public static Player LogInUser()
        {
            Console.WriteLine("Do you already have an account in the game?");
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "y")
            {
                return VerifyLogIn();
            }
            else if (answer == "admin")
            {
                return VerifyAdmin();
            }
            else
            {
                return AddNewUser();
            }
        }
        //verifies a users existence and checks the password.
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
            string passwordEntered = PasswordMasking().ToLower();
            string hashPass = GetHashString(passwordEntered);
            Player player = Player.CheckPassword(nameEntered, passwordEntered);
            Player playerHash = Player.CheckPassword(nameEntered, hashPass);
            //Console.WriteLine(hashPass);
            if (playerHash.UserName == "Fake player")
            {
                Console.WriteLine("Wrong Password!");
                return VerifyLogIn();
            }

            return playerHash;
        }
        //adds a new user to the game, that can be stored for a later date
        public static Player AddNewUser()
        {
            Console.WriteLine("What will be your user name?");
            string userName = "";
            string userPattern = @"^(?!.*\|)(?=.*\w).*$";
            bool badUserName = true;
            while (badUserName)
            {
                userName = Console.ReadLine();
                if (Regex.IsMatch(userName, userPattern))
                {
                    badUserName = false;
                }
                else
                {
                    Console.WriteLine("Invalid user name, no \"|\"");

                }
            }
            Console.WriteLine("Your password must contain a number, a symbol (not \"|\"), and must be atleast 4 characters.");
            bool badPassword = true;

            string password = "";
            string passPattern = @"^(?!.*\|)(?=.*[0-9])(?=.*\W)(?=.*\w).{4,}$";
            while (badPassword)
            {
                password = Console.ReadLine().ToLower();
                if (Regex.IsMatch(password, passPattern))
                {
                    badPassword = false;
                }
                else
                {
                    Console.WriteLine("Invalid password your password must contain a number, a symbol (not \"|\"), and must be atleast 4 characters.");

                }
            }
            //Console.WriteLine(password);
            password.Trim();
            password = GetHashString(password);
            Player newPlayer = new Player(userName, password, 0, 0, 0);
            Player.AddPlayer(newPlayer);
            return newPlayer;
        }
        //checks to see if an admin account is being used.
        public static Player VerifyAdmin()
        {
            Player.GrabPlayers();
            Console.WriteLine("What is your user name?");
            string nameEntered = Console.ReadLine().ToLower();
            bool checkExisting = Player.CheckUserName(nameEntered);

            if (!checkExisting)
            {
                Console.WriteLine("This user name doesn't exist");
                return VerifyAdmin();
            }

            Console.WriteLine("What is your password?");
            string passwordEntered = PasswordMasking().ToLower();

            Player player = Player.CheckPassword(nameEntered, passwordEntered);


            if (player.UserName == "Fake player")
            {
                Console.WriteLine("Wrong Password!");
                return VerifyAdmin();
            }

            return player;
        }
        //this method in conjunction with the GetHash method hides the passwords stored from the user.
        public static string GetHashString(string input)
        {
            //Console.WriteLine("this string" + input);
            StringBuilder sb = new StringBuilder();
            foreach (Byte b in GetHash(input))
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static byte[] GetHash(string input)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
        //hides the characters inputed in the password field
        public static string PasswordMasking()
        {
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    Console.Write("\b");
                }
            } while (key.Key != ConsoleKey.Enter);
            pass = pass.Trim();
            Console.WriteLine();
            return pass;
        }
        //sets the difficulty of the game for the user
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
        //this method will take in the user input and compare to the characters of the word stored in game to see if they match.
        public static bool GuessingGame(Word word)
        {
            int missCount = 0;
            int misses = 0;
            bool checkWin = false;
            bool checkLoss = false;
            string guessedLetters = string.Empty;
            string mysteryWord = word.MysteriousName;
            char[] guess = new char[mysteryWord.Length];
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
                if (guessedLetters != string.Empty)
                {

                    Console.WriteLine("You have guessed: " + guessedLetters);
                }
                char playerGuess = GetUserInput(guessedLetters);
                guessedLetters += playerGuess;
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
                    Console.WriteLine("              ;   ;");
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
        //this picks the words from a stored list in game.
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

        //grabs user's letters and validates that a single letter was inputed
        public static char GetUserInput(string guessedLetters)
        {

            Console.WriteLine("Pick a letter!");
            string message = Console.ReadLine().ToLower();
            if (Regex.IsMatch(message, @"^[a-z]{1}$") && !(guessedLetters.Contains(message)))
            {
                return message[0];

            }
            return GetUserInput(guessedLetters);
        }
        //displays guess attempts left
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
        //determines if the user wishes to continue playing
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




    }
}
