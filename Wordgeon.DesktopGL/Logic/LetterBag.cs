using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordgeon.Logic
{
    public class LetterBag
    {
        private LetterBag(Random random, IEnumerable<LetterTile> letters)
        {
            Random = random;
            Letters = letters;
        }

        public Random Random { get; }

        public IEnumerable<LetterTile> Letters { get; }

        public static LetterBag New(Random random)
        {
            var letters = MakeSet(random);
            var lb = new LetterBag(random, letters);
            return lb;
        }

        public (LetterBag, IEnumerable<LetterTile>) PullTiles(int howMany)
        {
            var tiles = Letters.Take(howMany).ToArray();
            var left = Letters.Skip(howMany).ToArray();
            if (left.Count() < 98)
            {
                left = left.Concat(MakeSet(Random)).ToArray();
            }
            return (new LetterBag(Random, left), tiles);
        }

        private static IEnumerable<LetterTile> MakeSet(Random random)
        {
            return Enumerable.Empty<char>()
                .Concat(MakeChars('a', 9))
                .Concat(MakeChars('b', 2))
                .Concat(MakeChars('c', 2))
                .Concat(MakeChars('d', 4))
                .Concat(MakeChars('e', 12))
                .Concat(MakeChars('f', 2))
                .Concat(MakeChars('g', 3))
                .Concat(MakeChars('h', 2))
                .Concat(MakeChars('i', 9))
                .Concat(MakeChars('j', 1))
                .Concat(MakeChars('k', 1))
                .Concat(MakeChars('l', 4))
                .Concat(MakeChars('m', 2))
                .Concat(MakeChars('n', 6))
                .Concat(MakeChars('o', 8))
                .Concat(MakeChars('p', 2))
                .Concat(MakeChars('q', 1))
                .Concat(MakeChars('r', 6))
                .Concat(MakeChars('s', 4))
                .Concat(MakeChars('t', 6))
                .Concat(MakeChars('u', 4))
                .Concat(MakeChars('v', 2))
                .Concat(MakeChars('w', 2))
                .Concat(MakeChars('x', 1))
                .Concat(MakeChars('y', 2))
                .Concat(MakeChars('z', 1))
                .OrderBy(_ => random.Next())
                .Select(c => new LetterTile(c));
        }

        private static IEnumerable<char> MakeChars(char val, int count)
        {
            return Enumerable.Range(0, count).Select(_ => val);
        }
    }
}
