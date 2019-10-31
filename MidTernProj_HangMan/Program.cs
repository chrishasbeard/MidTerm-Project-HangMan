using System;

namespace MidTernProj_HangMan
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] word = { "banana", "taco", "boat" };
            //int o;
           
            HangMan.GetUserInput();

            string mysteryWord = word[0];
            char[] guess = new char[mysteryWord.Length];
            //HangMan displayToPlayer = new HangMan();
            for (int i = 0; i < word[0].Length; i++)
                Console.Write(" *");
            while (true)
            {
                char playerGuess = char.Parse(Console.ReadLine());
                for (int j = 0; j < mysteryWord.Length; j++)
                {
                    if (playerGuess == mysteryWord[j])
                        guess[j] = playerGuess;
                }
                Console.WriteLine(guess);
            }
        }
    }
}
