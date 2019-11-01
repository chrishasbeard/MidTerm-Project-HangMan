using System;

namespace MidTernProj_HangMan
{
    class Program
    {
        static void Main(string[] args)
        {
            string mysteryWord = "bunny";
            char[] guess = new char[mysteryWord.Length];
            bool check = false;
            //HangMan displayToPlayer = new HangMan();
            for (int i = 0; i < mysteryWord.Length; i++)
            {
                Console.Write("*");
                //Console.WriteLine("");
                guess[i] = '*';
            }
            HangMan.GetUserInput();

            while (true)
            {
                Console.WriteLine();
                check = true;
                char playerGuess = char.Parse(Console.ReadLine());
                for (int j = 0; j < mysteryWord.Length; j++)
                {
                    if (playerGuess == mysteryWord[j])
                        guess[j] = playerGuess;
                }
                for (int i = 0; i < guess.Length; i++)
                {
                    if(guess[i] == '*')
                    {
                        check = false;
                    }
                }
                if (check)
                {
                    break;
                }
                Console.WriteLine(guess);
            }
        }
    }
}
