using System;

namespace Wordgeon
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            GameDictionary.Load();
            using (var game = new WordgeonGame())
                game.Run();
        }
    }
}
