using System;
using System.Linq;
using WordGame.Logic;

namespace WordGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new WordGame())
                game.Run();

            //var ws = new Words();
            //ws.Initialize();
            //var gs = GameState.New(ws);

            //while (true)
            //{
            //    Console.WriteLine($"{gs.StartsWith.Value}...{gs.CharBoard.EndsWith}");
            //    var i = 0;
            //    for (int x = 0; x < 4; x++)
            //    {
            //        for (int y = 0; y < 4; y++)
            //        {
            //            var cell = gs.CharBoard.CharCells.ElementAt(i);
            //            Console.Write($"{cell.Value}({(cell.SelectionIndex.HasValue ? cell.SelectionIndex.Value.ToString() : " ")}) ");
            //            i++;
            //        }
            //        Console.WriteLine();
            //    }
            //    Console.WriteLine();
            //    Console.Write("> ");
            //    var line = Console.ReadLine();

            //    Maybe<GameState> ns;
            //    if (string.IsNullOrWhiteSpace(line))
            //    {
            //        ns = gs.CompleteWord();
            //        if (ns.HasValue)
            //        {
            //            ns.Value.AttemptResults.ToList().ForEach(a =>
            //            {
            //                if (a is AttemptResult.SuccessResult sr)
            //                {
            //                    Console.BackgroundColor = ConsoleColor.Green;
            //                    Console.ForegroundColor = ConsoleColor.Black;
            //                }
            //                else
            //                {
            //                    Console.BackgroundColor = ConsoleColor.Red;
            //                }
            //                Console.WriteLine(a.AttemptedWord);
            //                Console.BackgroundColor = ConsoleColor.Black;
            //                Console.ForegroundColor = ConsoleColor.White;
            //            });
            //        }
            //    }
            //    else
            //    {
            //        var l = line.FirstOrDefault();
            //        ns = gs.MakeAutoLetterSelection(l);
            //    }


            //    if (!ns.HasValue)
            //    {
            //        Console.WriteLine("Try Again");
            //    }
            //    else
            //    {
            //        gs = ns.Value;
            //    }
            //}
        }
    }
}
