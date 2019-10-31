using System;
namespace MidTernProj_HangMan
{
    public class Word
    {
        public string MysteriousName { get; set; }
        public string Difficulty { get; set; }

        public Word(string mysterio, string difficulty)
        {
            MysteriousName = mysterio;
            Difficulty = difficulty;
        }
    }
}
