using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MidTernProj_HangMan
{
    class Player
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public double WinPercent { get; set; }
        public int WinNum { get; set; }
        public int Losses { get; set; }

        private static List<Player> players = new List<Player>()
        {

        };

        public Player(string userName, string password, double winPercent, int winNum, int losses)
        {
            UserName = userName;
            Password = password;
            WinPercent = winPercent;
            WinNum = winNum;
            Losses = losses;
        }

        //grabs players from text file to see if there was a valid log in.
        public static void GrabPlayers()
        {
            List<string> users = new List<string>(File.ReadAllLines("../../../UserInformation.txt"));

            if (players.Count == 0)
            {
                foreach (string user in users)
                {
                    string[] word = user.Split('|');
                    players.Add(new Player(word[0], word[1], double.Parse(word[2]), int.Parse(word[3]), int.Parse(word[4])));
                }
            }

        }
       //finds a players user name from the list
        public static bool CheckUserName(string userName)
        {
            foreach (Player player in players)
            {
                if(player.UserName == userName)
                {
                    return true;
                }
            }
            return false;
        }
       //finds a players password from the list
        public static Player CheckPassword(string userName, string password)
        {
            foreach(Player player in players)
            {
                if (player.UserName == userName)
                {
                    if (password == player.Password)
                        return player;
                }
                
            }
            return new Player("Fake player", "", 0, 0,0);
        }

        // adds new player to the list
        public static void AddPlayer(Player player)
        {
            string path = "C:\\Users\\Holmes HQ\\source\\repos\\MidTernProj_HangMan\\MidTernProj_HangMan\\UserInformation.txt";
            List<string> users = new List<string>(File.ReadAllLines("../../../UserInformation.txt")); 

            foreach(string user in users)
            {
                string[] word = user.Split('|');
                players.Add(new Player(word[0], word[1], double.Parse(word[2]), int.Parse(word[3]), int.Parse(word[4])));   
                

            }

            players.Add(player);

            using(StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine($"{player.UserName}|{player.Password}|{player.WinPercent}|{player.WinNum}|{player.Losses}");
                writer.Close();
            }

        }
        //calculates the win loss percentage of user
        public static void CalcWinLoss(Player player)
        {

            try
            {
                player.WinPercent = Math.Round(((double)player.WinNum / (double)player.Losses),2);
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("U N D E F E A T E D! ! !");
               
            }
        }
        //shows the score for all players on the list
        public static void CheckHighScore()
        {
            players.Sort((x, y) => x.WinPercent.CompareTo(y.WinPercent));
            Console.WriteLine("Wins:\t Losses:  Win %:\t Name:");
            foreach(Player player in players)
            {
                Console.WriteLine($"{player.WinNum}\t {player.Losses}\t {player.WinPercent}\t {player.UserName}");
            }
        }
        //updates the text file
        public static void WriteScore()
        {
            if (players.Count > 0)
            {
                StreamWriter writer = new StreamWriter("../../../UserInformation.txt");

                foreach (Player player in players)
                {
                    writer.WriteLine($"{player.UserName}|{player.Password}|{player.WinPercent}|{player.WinNum}|{player.Losses}");
                }
                writer.Close();
            }
        }
    }
}
