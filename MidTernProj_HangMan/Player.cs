using System;
using System.Collections.Generic;
using System.IO;
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

        //public static List<Player> GetPlayers()
        //{
        //    return players;
        //}
        public static void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public static void GrabPlayers()
        {
            StreamReader reader = new StreamReader("../../../UserInformation.txt");
            string line = reader.ReadLine();
            string[] word = line.Split('|');

            while (line != null)
            {
                word = line.Split('|');
                Player.AddPlayer(new Player(word[0], word[1], double.Parse(word[2]), int.Parse(word[3]), int.Parse(word[4])));
                line = reader.ReadLine();
            }
            reader.Close();
            StreamWriter writer = new StreamWriter("../../../UserInformation.txt");
            foreach (Player players in players)
            {
                writer.WriteLine($"{players.UserName}|{players.Password}|{players.WinPercent}|{players.WinNum}|");
            }
            writer.Close();
        }
    }
}
